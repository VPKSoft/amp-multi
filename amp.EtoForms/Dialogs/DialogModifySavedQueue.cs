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
using amp.EtoForms.Utilities;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog to modify a saved queue snapshot.
/// Implements the <see cref="Eto.Forms.Dialog{T}" />
/// </summary>
/// <seealso cref="Eto.Forms.Dialog{T}" />
internal class DialogModifySavedQueue : Dialog<bool>
{
    private readonly AmpContext context;
    private readonly long queueId;
    private readonly ObservableCollection<QueueSong> queueSongs = new();
    private readonly GridView gvAlbumQueueTracks;
    private readonly Button btnCancel = new() { Text = UI.Close, };
    private readonly Button btnSaveAndClose = new() { Text = UI.SaveClose, };
    private readonly List<long> toDelete = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogModifySavedQueue"/> class.
    /// </summary>
    /// <param name="context">The amp# database context.</param>
    /// <param name="queueId">The queue reference identifier.</param>
    public DialogModifySavedQueue(AmpContext context, long queueId)
    {
        this.context = context;
        this.queueId = queueId;

        MinimumSize = new Size(700, 500);

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
                new ButtonToolItem(MoveUpDownClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_arrow_up_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.MoveUpwardsInQueue,
                    Tag = -1,
                },
                new ButtonToolItem(MoveUpDownClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_arrow_down_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.MoveDownwardsInQueue,
                    Tag = 1,
                },
            },
        };

        gvAlbumQueueTracks = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(QueueSong.QueueIndex)),
                    HeaderText = UI.QueueCreated,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((QueueSong qs) => SongDisplayNameGenerate.GetSongName(qs.Song!))
                            .Convert(s => s)
                            .Cast<string?>(),
                    }, Expand = true,
                    HeaderText = UI.QueueName,
                },
            },
            DataStore = queueSongs,
        };

        Content = new TableLayout
        {
            Rows =
            {
                new TableRow(gvAlbumQueueTracks) { ScaleHeight = true,},
            },
            Padding = Globals.DefaultPadding,
            Spacing = Globals.DefaultSpacing,
        };

        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnSaveAndClose);

        btnCancel.Click += BtnCancel_Click;
        btnSaveAndClose.Click += BtnSaveAndClose_Click;

        Shown += DialogModifySavedQueue_Shown;
    }

    private void MoveUpDownClick(object? sender, EventArgs e)
    {
        var addIndex = (int?)((ButtonToolItem?)sender)?.Tag;
        var switchItem = queueSongs.FirstOrDefault(f => f.QueueIndex == SelectedItem?.QueueIndex + addIndex);
        if (SelectedItem != null && switchItem != null)
        {
            SwitchItems(SelectedItem, switchItem);
        }
    }

    private void SwitchItems(QueueSong itemFirst, QueueSong itemSecond)
    {
        var indexFirst = queueSongs.IndexOf(itemFirst);
        var indexSecond = queueSongs.IndexOf(itemSecond);
        var queueIndexFirst = itemFirst.QueueIndex;
        var queueIndexSecond = itemSecond.QueueIndex;
        itemSecond.ModifiedAtUtc = DateTime.UtcNow;
        itemFirst.ModifiedAtUtc = DateTime.UtcNow;
        itemFirst.QueueIndex = queueIndexSecond;
        itemSecond.QueueIndex = queueIndexFirst;
        queueSongs[indexFirst] = itemSecond;
        queueSongs[indexSecond] = itemFirst;
        gvAlbumQueueTracks.SelectRow(indexSecond);
    }

    private void DeleteClick(object? sender, EventArgs e)
    {
        if (SelectedItem != null)
        {
            var selectedRowIndex = gvAlbumQueueTracks.SelectedRow;
            toDelete.Add(SelectedItem.Id);
            var queueIndex = SelectedItem.QueueIndex;
            queueSongs.Remove(SelectedItem);
            foreach (var queueSong in queueSongs)
            {
                if (queueSong.QueueIndex >= queueIndex)
                {
                    queueSong.QueueIndex--;
                }
            }

            if (queueSongs.Count > 0)
            {
                gvAlbumQueueTracks.ReloadData(new Range<int>(0, queueSongs.Count - 1));
            }

            if (selectedRowIndex < queueSongs.Count - 1)
            {
                gvAlbumQueueTracks.SelectedRow = selectedRowIndex;
            }
        }
    }

    private QueueSong? SelectedItem
    {
        get
        {
            var selectedItem = ((QueueSong?)gvAlbumQueueTracks.SelectedItem);
            return selectedItem;
        }
    }

    private async Task SetTitle()
    {
        var snapshot = await context.QueueSnapshots.FirstOrDefaultAsync(f => f.Id == queueId);
        Title = snapshot != null ? $"amp# {UI._} {UI.ModifyQueue} [{snapshot.SnapshotName}]" : $"amp# {UI._} {UI.ModifyQueue}";
    }

    private async Task ListQueue()
    {
        queueSongs.Clear();
        foreach (var queueSong in await context.QueueSongs.Include(f => f.Song).Where(f => f.QueueSnapshotId == queueId)
                     .OrderBy(f => f.QueueIndex).AsNoTracking().ToListAsync())
        {
            queueSongs.Add(queueSong);
        }
    }

    private async void DialogModifySavedQueue_Shown(object? sender, EventArgs e)
    {
        await SetTitle();
        await ListQueue();
    }

    private async void BtnSaveAndClose_Click(object? sender, EventArgs e)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var itemsToDelete = await context.QueueSongs.Where(f => toDelete.Contains(f.Id)).ToListAsync();
            context.QueueSongs.RemoveRange(itemsToDelete);
            await context.SaveChangesAsync();

            context.QueueSongs.UpdateRange(queueSongs);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

        }, async (_) => await transaction.RollbackAsync());

        Close(true);
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close(false);
    }
}