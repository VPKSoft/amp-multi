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

using amp.Database.LegacyConvert;
using amp.EtoForms.Localization;
using Eto.Drawing;
using Eto.Forms;
using Button = Eto.Forms.Button;
using Control = Eto.Forms.Control;
using Label = Eto.Forms.Label;
using Panel = Eto.Forms.Panel;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog to perform migration of the old database into the new EF Core format. Only used once.
/// Implements the <see cref="Eto.Forms.Dialog{T}" />
/// </summary>
/// <seealso cref="Eto.Forms.Dialog{T}" />
public class DialogDatabaseConvertProgress : Dialog<bool>
{
    public DialogDatabaseConvertProgress()
    {
        MinimumSize = new Size(500, 200);

        lbSongCount = new Label();
        lbAlbumCount = new Label();
        lbAlbumEntryCount = new Label();
        lbQueueEntryCount = new Label();
        lbTotalCount = new Label();
        lbTotalPercentage = new Label();
        lbEtaValue = new Label();
        progressBar = new ProgressBar();

        Title = $"amp# - {UI.DatabaseConversion}";

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
                                    new Label { Text = UI.Songs + ":", },
                                    new Panel { Width = 5, },
                                    lbSongCount,
                                    new Panel { Width = 10, },
                                    new Label { Text = UI.Albums + ":", },
                                    new Panel { Width = 5, },
                                    lbAlbumCount,
                                },
                            },
                            new TableRow
                            {
                                Cells =
                                {
                                    new Label { Text = UI.AlbumEntries + ":", },
                                    new Panel { Width = 5, },
                                    lbAlbumEntryCount,
                                    new Panel { Width = 10, },
                                    new Label { Text = UI.QueueEntries + ":", },
                                    new Panel { Width = 5, },
                                    lbQueueEntryCount,
                                },
                            },
                            new TableRow
                            {
                                Cells =
                                {
                                    new Label { Text = UI.Total + ":", },
                                    new Panel { Width = 5, },
                                    lbTotalCount,
                                    new Panel { Width = 10, },
                                    new Label { Text = UI.TotalPercentage + ":", },
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
                                            new Label { Text = UI.TimeEstimateReady + ":", },
                                            lbEtaValue,
                                        },
                                    },
                                },
                            }, Padding = new Padding(10, 2)
                    },
                    HorizontalAlignment.Center),
            },
            Orientation = Orientation.Vertical,
        };

        btClose = new Button((_, _) => Close()) { Text = UI.Close, Enabled = false, };
        btCancel = new Button((_, _) =>
        {
            MigrateOld.AbortConversion();
        })
        { Text = UI.Cancel, };
        PositiveButtons.Add(btClose);
        NegativeButtons.Add(btCancel);
        DefaultButton = btClose;
        AbortButton = btCancel;
        Closing += DialogDatabaseConvertProgress_Closing;
    }

    private void DialogDatabaseConvertProgress_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!aborted)
        {
            MigrateOld.AbortConversion();
            e.Cancel = true;
        }
    }

    private bool aborted;

    /// <summary>
    /// Shows the dialog modally, blocking the current thread until it is closed.
    /// </summary>
    /// <param name="owner">The owner control that is showing the form</param>
    /// <param name="fileNameOld">The old database file name.</param>
    /// <param name="fileNameNew">The new database file name.</param>
    /// <remarks>The <paramref name="owner" /> specifies the control on the window that will be blocked from user input until
    /// the dialog is closed.
    /// Calling this method is identical to setting the <see cref="P:Eto.Forms.Window.Owner" /> property and calling <see cref="M:Eto.Forms.Dialog.ShowModal" />.</remarks>
    public void ShowModal(Control? owner, string fileNameOld, string fileNameNew)
    {
        MigrateOld.ReportProgress += MigrateOld_ReportProgress;
        MigrateOld.ThreadStopped += MigrateOld_ThreadStopped;
        MigrateOld.RunConvert(fileNameOld, fileNameNew);
        ShowModal();
    }

    private void MigrateOld_ThreadStopped(object? sender, ConvertProgressArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            aborted = true;
            btClose.Enabled = true;
            btCancel.Enabled = false;
        });
    }

    private void MigrateOld_ReportProgress(object? sender, ConvertProgressArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            lbSongCount.Text = $"{e.SongsHandledCount} / {e.SongsCountTotal}";
            lbAlbumCount.Text = $"{e.AlbumsHandledCount} / {e.AlbumsCountTotal}";
            lbAlbumEntryCount.Text = $"{e.AlbumEntriesHandledCount} / {e.AlbumEntryCountTotal}";
            lbQueueEntryCount.Text = $"{e.QueueEntriesHandledCount} / {e.QueueEntryCountTotal}";
            lbTotalCount.Text = $"{e.HandledCountTotal} / {e.CountTotal}";
            var totalPercentage = e.CountTotal == 0 ? 100.0 : (double)e.HandledCountTotal / e.CountTotal * 100.0;
            lbTotalPercentage.Text = $"{totalPercentage:F2}";
            progressBar.Value = (int)Math.Ceiling(totalPercentage);
            lbEtaValue.Text = e.Eta == null ? "-" : $"{e.Eta.Value.ToShortDateString()} {e.Eta.Value.ToLongTimeString()}";
        });
    }

    private readonly Label lbSongCount;
    private readonly Label lbAlbumCount;
    private readonly Label lbAlbumEntryCount;
    private readonly Label lbQueueEntryCount;
    private readonly Label lbTotalCount;
    private readonly Label lbTotalPercentage;
    private readonly Label lbEtaValue;
    private readonly Button btClose;
    private readonly Button btCancel;
    private readonly ProgressBar progressBar;
}