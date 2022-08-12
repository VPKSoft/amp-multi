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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amp.Database;
using amp.EtoForms.Models;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog to rescan tag data for multiple audio track.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public class DialogUpdateTagData : Dialog
{
    private readonly AmpContext context;
    private readonly List<long> trackIds;

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
                                    new Label { Text = UI.Tracks + ":", },
                                    new Panel { Width = 5, },
                                    lbTrackCount,
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
                            }, Padding = new Padding(10, 2),
                    },
                    HorizontalAlignment.Center),
            },
            Orientation = Orientation.Vertical,
        };

        btClose.Click += delegate { Close(); };
        btCancel.Click += delegate { cancel = true; };

        PositiveButtons.Add(btClose);
        NegativeButtons.Add(btCancel);
        DefaultButton = btClose;
        AbortButton = btCancel;
    }

    private volatile bool cancel;

    private readonly Label lbTrackCount = new();
    private readonly Label lbTotalCount = new();
    private readonly Label lbTotalPercentage = new();
    private readonly Label lbEtaValue = new();
    private readonly Button btClose = new() { Text = UI.Close, Enabled = false, };
    private readonly Button btCancel = new() { Text = UI.Cancel, };
    private readonly ProgressBar progressBar = new();
}