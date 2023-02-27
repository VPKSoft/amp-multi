#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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
using amp.DataAccessLayer.DtoClasses;
using amp.Database;
using amp.Database.ExtensionClasses;
using amp.EtoForms.Utilities;
using amp.Shared.Interfaces;
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
    private readonly ObservableCollection<QueueTrack> queueTracks = new();
    private readonly GridView gvAlbumQueueTracks;
    private readonly Button btnCancel = new() { Text = UI.Close, };
    private readonly Button btnSaveAndClose = new() { Text = UI.SaveClose, };
    private readonly List<long> toDelete = new();
    private readonly DefaultCancelButtonHandler defaultCancelButtonHandler;

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

        var color = Color.Parse(Globals.ColorConfiguration.ColorToolButtonImage);

        ToolBar = new ToolBar
        {
            Items =
            {
                new ButtonToolItem(DeleteClick)
                {
                    Image = EtoHelpers.ImageFromSvg(color, Size16.ic_fluent_delete_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.DeleteSavedQueue,
                },
                new ButtonToolItem(MoveUpDownClick)
                {
                    Image = EtoHelpers.ImageFromSvg(color, Size16.ic_fluent_arrow_up_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.MoveUpwardsInQueue,
                    Tag = -1,
                },
                new ButtonToolItem(MoveUpDownClick)
                {
                    Image = EtoHelpers.ImageFromSvg(color, Size16.ic_fluent_arrow_down_16_filled,
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
                    DataCell = new TextBoxCell(nameof(QueueTrack.QueueIndex)),
                    HeaderText = UI.QueueIndex,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((QueueTrack qs) => TrackDisplayNameGenerate.GetAudioTrackName(qs.AudioTrack))
                            .Convert(s => s)
                            .Cast<string?>(),
                    }, Expand = true,
                    HeaderText = UI.QueueName,
                },
            },
            DataStore = queueTracks,
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
        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithCancelButton(btnCancel);
        Closed += DialogModifySavedQueue_Closed;
    }

    private void DialogModifySavedQueue_Closed(object? sender, EventArgs e)
    {
        defaultCancelButtonHandler.Dispose();
    }

    private void MoveUpDownClick(object? sender, EventArgs e)
    {
        var addIndex = (int?)((ButtonToolItem?)sender)?.Tag;
        var switchItem = queueTracks.FirstOrDefault(f => f.QueueIndex == SelectedItem?.QueueIndex + addIndex);
        if (SelectedItem != null && switchItem != null)
        {
            SwitchItems(SelectedItem, switchItem);
        }
    }

    private void SwitchItems(QueueTrack itemFirst, QueueTrack itemSecond)
    {
        var indexFirst = queueTracks.IndexOf(itemFirst);
        var indexSecond = queueTracks.IndexOf(itemSecond);
        var queueIndexFirst = itemFirst.QueueIndex;
        var queueIndexSecond = itemSecond.QueueIndex;
        itemSecond.ModifiedAtUtc = DateTime.UtcNow;
        itemFirst.ModifiedAtUtc = DateTime.UtcNow;
        itemFirst.QueueIndex = queueIndexSecond;
        itemSecond.QueueIndex = queueIndexFirst;
        queueTracks[indexFirst] = itemSecond;
        queueTracks[indexSecond] = itemFirst;
        gvAlbumQueueTracks.SelectRow(indexSecond);
    }

    private void DeleteClick(object? sender, EventArgs e)
    {
        if (SelectedItem == null)
        {
            return;
        }

        var selectedRowIndex = gvAlbumQueueTracks.SelectedRow;
        toDelete.Add(SelectedItem.Id);
        var queueIndex = SelectedItem.QueueIndex;
        queueTracks.Remove(SelectedItem);
        foreach (var queueTrack in queueTracks)
        {
            if (queueTrack.QueueIndex >= queueIndex)
            {
                queueTrack.QueueIndex--;
            }
        }

        if (queueTracks.Count > 0)
        {
            gvAlbumQueueTracks.ReloadData(new Range<int>(0, queueTracks.Count - 1));
        }

        if (selectedRowIndex < queueTracks.Count - 1)
        {
            gvAlbumQueueTracks.SelectedRow = selectedRowIndex;
        }
    }

    private QueueTrack? SelectedItem
    {
        get
        {
            var selectedItem = (QueueTrack?)gvAlbumQueueTracks.SelectedItem;
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
        queueTracks.Clear();
        foreach (var queueTrack in await context.QueueTracks.Include(f => f.AudioTrack).Where(f => f.QueueSnapshotId == queueId)
                     .OrderBy(f => f.QueueIndex).Select(f => DataAccessLayer.Globals.AutoMapper.Map<QueueTrack>(f)).ToListAsync())
        {
            queueTracks.Add(queueTrack);
        }
    }

    private async void DialogModifySavedQueue_Shown(object? sender, EventArgs e)
    {
        await SetTitle();
        await ListQueue();
    }

    private async void BtnSaveAndClose_Click(object? sender, EventArgs e)
    {
        context.ChangeTracker.Clear();
        await using var transaction = await context.Database.BeginTransactionAsync();

        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var itemsToDelete = await context.QueueTracks.Where(f => toDelete.Contains(f.Id)).ToListAsync();
            context.QueueTracks.RemoveRange(itemsToDelete);
            await context.SaveChangesAsync();

            foreach (var queueTrack in queueTracks)
            {
                await UpdateTrack(context, queueTrack);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            context.ChangeTracker.Clear();
        }, async _ => await transaction.RollbackAsync());

        Close(true);
    }

    private static async Task UpdateTrack(AmpContext context, IQueueTrack queueTrack)
    {
        var track = await context.QueueTracks.FirstAsync(f => f.Id == queueTrack.Id);
        track.AudioTrackId = queueTrack.AudioTrackId;
        track.CreatedAtUtc = queueTrack.CreatedAtUtc;
        track.ModifiedAtUtc = queueTrack.ModifiedAtUtc;
        track.QueueSnapshotId = queueTrack.QueueSnapshotId;
        track.QueueIndex = queueTrack.QueueIndex;

        await context.SaveChangesAndUntrackAsync(track);
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close(false);
    }
}