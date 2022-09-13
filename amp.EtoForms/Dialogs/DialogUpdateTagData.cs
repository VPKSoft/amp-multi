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

using amp.DataAccessLayer.DtoClasses;
using amp.Database;
using amp.Shared.Classes;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog to rescan tag data for multiple audio track.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public class DialogUpdateTagData : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogUpdateTagData"/> class.
    /// </summary>
    /// <param name="context">The <see cref="AmpContext"/> context.</param>
    /// <param name="tracks">The tracks which tag data to rescan.</param>
    public DialogUpdateTagData(AmpContext context, List<AudioTrack> tracks)
    {
        this.context = context;
        trackIds = tracks.Select(f => f.Id).ToList();
        CreateLayout();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogUpdateTagData"/> class.
    /// </summary>
    /// <param name="context">The <see cref="AmpContext"/> context.</param>
    /// <param name="tracks">The tracks which tag data to rescan.</param>
    public DialogUpdateTagData(AmpContext context, List<Database.DataModel.AudioTrack> tracks)
    {
        this.context = context;
        trackIds = tracks.Select(f => f.Id).ToList();
        CreateLayout();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogUpdateTagData"/> class.
    /// </summary>
    /// <param name="context">The <see cref="AmpContext"/> context.</param>
    public DialogUpdateTagData(AmpContext context)
    {
        this.context = context;
        trackIds = context.AudioTracks.Select(f => f.Id).ToList();
        CreateLayout();
    }

    private void CreateLayout()
    {
        MinimumSize = new Size(500, 200);

        Title = $"amp# - {UI.UpdateTrackMetadata}";

        Content = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(
                    new TableLayout
                    {
                        Rows =
                        {
                            new TableRow
                            {
                                Cells =
                                {
                                    new Label { Text = UI.Tracks + UI.ColonDelimiter, },
                                    new Panel { Width = 5, },
                                    lbTrackCount,
                                },
                            },
                            new TableRow
                            {
                                Cells =
                                {
                                    new Label { Text = UI.Errors + UI.ColonDelimiter, },
                                    new Panel { Width = 5, },
                                    lbErrorsCount,
                                },
                            },
                            new TableRow
                            {
                                Cells =
                                {
                                    new Label { Text = UI.Total + UI.ColonDelimiter, },
                                    new Panel { Width = 5, },
                                    lbTotalCount,
                                    new Panel { Width = 10, },
                                    new Label { Text = UI.TotalPercentage + UI.ColonDelimiter, },
                                    new Panel { Width = 5, },
                                    lbTotalPercentage,
                                },
                            },
                        },
                        Padding = 10,
                    }, HorizontalAlignment.Center),
                new StackLayoutItem(new Panel { Content = progressBar, Padding = new Padding(10, 2), },
                    HorizontalAlignment.Stretch),
                new StackLayoutItem(
                    new Panel
                    {
                        Content =
                            new TableLayout
                            {
                                Rows =
                                {
                                    new TableRow
                                    {
                                        Cells =
                                        {
                                            new Label { Text = UI.TimeEstimateReady + UI.ColonDelimiter, },
                                            lbEtaValue,
                                        },
                                    },
                                },
                            }, Padding = new Padding(10, 2),
                    },
                    HorizontalAlignment.Center),
            },
            Orientation = Orientation.Vertical,
        };

        btnClose.Click += BtnCloseClick;
        btnCancel.Click += BtnCancelClick;

        PositiveButtons.Add(btnClose);
        NegativeButtons.Add(btnCancel);
        DefaultButton = btnClose;
        AbortButton = btnCancel;
        Shown += DialogUpdateTagData_Shown;
        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithCancelButton(btnCancel).WithDefaultButton(btnClose);
        Closed += DialogUpdateTagData_Closed;
    }

    private void DialogUpdateTagData_Closed(object? sender, EventArgs e)
    {
        defaultCancelButtonHandler?.Dispose();
    }

    private void BtnCloseClick(object? sender, EventArgs e)
    {
        Close();
    }

    private async void BtnCancelClick(object? sender, EventArgs e)
    {
        cancel = true;
        if (updateMetadataTask != null)
        {
            await updateMetadataTask.WaitAsync(CancellationToken.None);
        }
        Close();
    }

    private void DialogUpdateTagData_Shown(object? sender, EventArgs e)
    {
        updateMetadataTask = Task.Factory.StartNew(ThreadMethod);
    }

    private async void ThreadMethod()
    {
        var query = context.AudioTracks.Where(f => trackIds.Contains(f.Id));
        var count = await query.CountAsync();
        var errorCount = 0;
        var totalCount = 0;
        var i = 0;

        await Application.Instance.InvokeAsync(() =>
        {
            progressBar.MaxValue = count;
            lbTrackCount.Text = $"{count}";
        });

        while (i < count && !cancel)
        {
            var start = DateTime.Now;
            var tracks = await query.Skip(i).Take(100).ToListAsync();
            foreach (var track in tracks)
            {
                try
                {
                    track.UpdateTrackInfo(new FileInfo(track.FileName));
                }
                catch (Exception ex)
                {
                    errorCount++;
                    Globals.Logger?.Error(ex, "");
                }
                totalCount++;
            }


            estimateCalculator.AddData(start, DateTime.Now, 100);
            i += 100;

            await context.SaveChangesAsync();

            var countCurrent = totalCount;
            var errorCountCurrent = errorCount;
            await Application.Instance.InvokeAsync(() =>
            {
                lbTotalCount.Text = $"{countCurrent}";
                lbErrorsCount.Text = $"{errorCountCurrent}";
                var totalPercentage = count == 0 ? 100.0 : (double)countCurrent / count * 100.0;
                lbTotalPercentage.Text = $"{totalPercentage:F2}";
                progressBar.Value = countCurrent;
                var eta = estimateCalculator.Estimate(count);
                lbEtaValue.Text = eta == null ? UI._ : $"{eta.Value.ToShortDateString()} {eta.Value.ToLongTimeString()}";
            });
        }

        await Application.Instance.InvokeAsync(() =>
        {
            btnClose.Enabled = true;
            btnCancel.Enabled = false;
        });
    }

    private volatile bool cancel;

    private Task? updateMetadataTask;
    private readonly AmpContext context;
    private readonly List<long> trackIds;
    private readonly TimeEstimateCalculator estimateCalculator = new(500);
    private readonly Label lbTrackCount = new();
    private readonly Label lbTotalCount = new();
    private readonly Label lbTotalPercentage = new();
    private readonly Label lbEtaValue = new();
    private readonly Button btnClose = new() { Text = UI.Close, Enabled = false, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
    private readonly ProgressBar progressBar = new();
    private readonly Label lbErrorsCount = new();
    private DefaultCancelButtonHandler? defaultCancelButtonHandler;
}