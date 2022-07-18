#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System.Collections.ObjectModel;
using amp.Database;
using amp.Database.DataModel;
using amp.EtoForms.Dialogs;
using amp.EtoForms.Layout;
using amp.EtoForms.Utilities;
using amp.Shared.Extensions;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms.Forms;

/// <summary>
/// A dialog to manage saved queue snapshots.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public class FormSavedQueues : Dialog<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormSavedQueues"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public FormSavedQueues(AmpContext context)
    {
        this.context = context;

        MinimumSize = new Size(700, 500);
        Title = $"amp# {UI._} {UI.SavedQueues}";

        Padding = Globals.DefaultPadding;

        ToolBar = new ToolBar
        {
            Items =
            {
                new ButtonToolItem(DeleteClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_delete_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.DeleteSavedQueue,
                },
                new ButtonToolItem(EditClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_edit_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.EditTheQueueTracks,
                },
                new ButtonToolItem(CopyToFolderClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_copy_arrow_right_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.CopyTrackFilesToFolder,
                },
            },
        };

        cmbAlbumSelect = ReusableControls.CreateAlbumSelectCombo(SelectedValueChanged, context);

        gvAlbumQueues = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(QueueSnapshot.SnapshotName)), Expand = true,
                    Editable = true,
                    HeaderText = UI.QueueName,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((QueueSnapshot qs) => qs.SnapshotDate)
                            .Convert(s => string.Concat(s.ToShortDateString(), " ", s.ToShortTimeString()))
                            .Cast<string?>(),
                    },
                    HeaderText = UI.QueueCreated,
                },
            },
            DataStore = queueSnapshots,
        };

        gvAlbumQueues.SelectionChanged += GvAlbumQueues_SelectionChanged;

        var gvAlbumQueueTracks = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(QueueTrack.QueueIndex)),
                    HeaderText = UI.QueueCreated,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((QueueTrack qs) => SongDisplayNameGenerate.GetSongName(qs.AudioTrack!))
                            .Convert(s => s)
                            .Cast<string?>(),
                    }, Expand = true,
                    Editable = true,
                    HeaderText = UI.QueueName,
                },
            },
            DataStore = queueSongs,
        };

        Content = new TableLayout
        {
            Rows =
            {
                new TableRow(cmbAlbumSelect),
                new TableRow(
                new Splitter
                {
                    Panel1 = gvAlbumQueues,
                    Panel2 = gvAlbumQueueTracks,
                    RelativePosition = 0.5,
                    Orientation = Orientation.Vertical,
                    FixedPanel = SplitterFixedPanel.None,
                }) { ScaleHeight = true,},
            },
            Padding = Globals.DefaultPadding,
            Spacing = Globals.DefaultSpacing,
        };

        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnSaveAndClose);

        btnCancel.Click += BtnCancel_Click;
        btnSaveAndClose.Click += BtnSaveAndClose_Click;
        gvAlbumQueues.CellEdited += GvAlbumQueues_CellEdited;
    }

    private void CopyToFolderClick(object? sender, EventArgs e)
    {
        var fileNames = queueSongs.Select(f => f.AudioTrack!.FileName).ToList();
        if (selectFolderDialog.ShowDialog(this) == DialogResult.Ok)
        {
            Globals.LoggerSafeInvoke(() =>
            {
                foreach (var fileName in fileNames)
                {
                    FileHelpers.CopyTo(fileName, selectFolderDialog.Directory);
                }
            });
        }
    }

    private void GvAlbumQueues_CellEdited(object? sender, GridViewCellEventArgs e)
    {
        var album = (QueueSnapshot?)e.Item;
        if (album != null)
        {
            album.ModifiedAtUtc = DateTime.UtcNow;
        }
    }

    private async void BtnSaveAndClose_Click(object? sender, EventArgs e)
    {
        await Save();
        Close(true);
    }

    private async Task Save()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var removeSongs = await context.QueueTracks.Where(f => queuesToDelete.Contains(f.QueueSnapshotId))
                .ToListAsync();
            context.QueueTracks.RemoveRange(removeSongs);
            await context.SaveChangesAsync();

            var removeQueues = await context.QueueSnapshots.Where(f => queuesToDelete.Contains(f.Id)).ToListAsync();

            context.QueueSnapshots.RemoveRange(removeQueues);
            await context.SaveChangesAsync();

            context.QueueSnapshots.UpdateRange(queueSnapshots);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
            removeQueues.Clear();
        }, async (_) => { await transaction.RollbackAsync(); });
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close(false);
    }

    private void GvAlbumQueues_SelectionChanged(object? sender, EventArgs e)
    {
        var queueId = ((QueueSnapshot?)gvAlbumQueues.SelectedItem)?.Id;

        queueSongs.Clear();

        foreach (var queueSnapshot in context.QueueTracks.Include(f => f.AudioTrack).Where(f => f.QueueSnapshotId == queueId)
                     .OrderBy(f => f.QueueIndex).AsNoTracking())
        {
            queueSongs.Add(queueSnapshot);
        }
    }

    private void RefreshQueueSnapshots(long? arg)
    {
        arg ??= ((Models.Album?)(cmbAlbumSelect.SelectedValue))?.Id;

        queueSnapshots.Clear();

        foreach (var queueSnapshot in context.QueueSnapshots.Where(f => !queuesToDelete.Contains(f.Id) && f.AlbumId == arg).OrderBy(f => f.SnapshotName).AsNoTracking())
        {
            queueSnapshots.Add(queueSnapshot);
        }
    }

    private Task SelectedValueChanged(long? arg)
    {
        RefreshQueueSnapshots(arg);
        return Task.CompletedTask;
    }

    private void EditClick(object? sender, EventArgs e)
    {
        new DialogModifySavedQueue(context, SelectedQueueId).ShowModal(this);
    }

    private long SelectedQueueId => ((QueueSnapshot?)gvAlbumQueues.SelectedItem)?.Id ?? 0;

    private void DeleteClick(object? sender, EventArgs e)
    {
        if (SelectedQueueId != 0)
        {
            queuesToDelete.Add(SelectedQueueId);
        }
        RefreshQueueSnapshots(null);
    }

    private readonly List<long> queuesToDelete = new();
    private readonly ComboBox cmbAlbumSelect;
    private readonly AmpContext context;
    private readonly ObservableCollection<QueueSnapshot> queueSnapshots = new();
    private readonly ObservableCollection<QueueTrack> queueSongs = new();
    private readonly GridView gvAlbumQueues;
    private readonly Button btnCancel = new() { Text = UI.Close, };
    private readonly Button btnSaveAndClose = new() { Text = UI.SaveClose, };
    private readonly SelectFolderDialog selectFolderDialog = new() { Title = UI.SelectDestinationDirectory, };
}