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

using amp.EtoForms.Forms.EventArguments;
using amp.EtoForms.Models;
using amp.Shared.Localization;
using ATL;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;

namespace amp.EtoForms.Forms;

/// <summary>
/// A dialog to view and edit audio trackTag data.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public class FormDialogTrackInfo : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormDialogTrackInfo"/> class.
    /// </summary>
    /// <param name="audioTrack">The audio trackTag data.</param>
    /// <param name="audioTrackChanged">The event handler for the <see cref="AudioTrackChanged"/> event.</param>
    public FormDialogTrackInfo(AudioTrack audioTrack,
        EventHandler<AudioTrackChangedEventArgs>? audioTrackChanged = null)
    {
        Title = $"amp# - {audioTrack.DisplayName}";
        MinimumSize = new Size(800, 600);
        AudioTrackChanged += audioTrackChanged;
        this.audioTrack = audioTrack;
        imageOff = EtoHelpers.ImageFromSvg(Colors.Teal, Size16.ic_fluent_image_multiple_off_16_filled,
            new Size(300, 300));

        LayoutTagData();
        LayoutAmpData();
        LayoutRest();

        trackTag = new Track(audioTrack.FileName);
        SetData();
    }

    private readonly Image imageOff;
    private readonly List<Image> embeddedImages = new();
    private int embeddedImageIndex = -1;
    private readonly AudioTrack audioTrack;
    private readonly Track trackTag;
    private bool databaseImageExists;

    private void SetData()
    {
        if (audioTrack.TrackImageData is { Length: > 0, })
        {
            using var stream = new MemoryStream(audioTrack.TrackImageData);
            var bitmap = new Bitmap(stream);
            imageViewAmp.Image = bitmap;
            databaseImageExists = true;
        }
        else
        {
            imageViewAmp.Image = imageOff;
        }

        if (trackTag.EmbeddedPictures.Any())
        {
            foreach (var trackTagEmbeddedPicture in trackTag.EmbeddedPictures)
            {
                if (trackTagEmbeddedPicture.PictureData.Length > 0)
                {
                    using var stream = new MemoryStream(trackTagEmbeddedPicture.PictureData);
                    var bitmap = new Bitmap(stream);
                    embeddedImages.Add(bitmap);
                }
            }

            embeddedImageIndex = 0;
            imageViewTag.Image = embeddedImages[embeddedImageIndex];
        }
        else
        {
            imageViewTag.Image = imageOff;
        }

        tbAlbumTag.Text = trackTag.Album;
        tbArtistTag.Text = trackTag.Artist;
        tbTitleTag.Text = trackTag.Title;
        tbCommentTag.Text = trackTag.Comment;
        tbAlbumArtistTag.Text = trackTag.AlbumArtist;
        tbOriginalArtistTag.Text = trackTag.OriginalArtist;
        tbCopyrightTag.Text = trackTag.Copyright;
        tbGenreTag.Text = trackTag.Genre;
        tbConductorTag.Text = trackTag.Conductor;
        tbTrackNumberTag.Text = $"{trackTag.TrackNumber}";
        tbTrackTotalTag.Text = $"{trackTag.TrackTotal}";
        tbYearTag.Text = $"{trackTag.Year}";
        tbBitRate.Text = $"{trackTag.Bitrate}";
        tbAudioChannels.Text = $"{trackTag.ChannelsArrangement.NbChannels}";
        tbSampleRate.Text = $"{trackTag.SampleRate:F0}";
        tbDuration.Text = $"{TimeSpan.FromSeconds(trackTag.Duration):hh\\:mm\\:ss}";
        tbAudioFormat.Text = $"{trackTag.AudioFormat.Name}";
        tbFileName.Text = $"{audioTrack.FileNameNoPath}";
        tbFileNameFull.Text = $"{audioTrack.FileName}";
        tbFileSizeBytes.Text = $"{audioTrack.FileSizeBytes}";
        tbLyricsTag.Text = $"{trackTag.Lyrics.UnsynchronizedLyrics}";

        tbAlbumAmp.Text = audioTrack.Album;
        tbArtistAmp.Text = audioTrack.Artist;
        tbTitleAmp.Text = audioTrack.Title;
        tbTrackNumberAmp.Text = audioTrack.Track;
        tbYearAmp.Text = audioTrack.Year;
        tbLyricsAmp.Text = audioTrack.Lyrics;
    }

    private void LayoutTagData()
    {
        var tlTrackDataTag = new TableLayout
        {
            Rows =
            {
                new TableRow(new Label { Text = UI.Album, }, new TableCell(tbAlbumTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Artist, }, new TableCell(tbArtistTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.AlbumArtist, },
                    new TableCell(tbAlbumArtistTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.OriginalArtist, },
                    new TableCell(tbOriginalArtistTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Title, }, new TableCell(tbTitleTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Comment, }, new TableCell(tbCommentTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Copyright, }, new TableCell(tbCopyrightTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Genre, }, new TableCell(tbGenreTag) { ScaleWidth = true, }),
                // Uncomment if the "Conductor" is found useful: new TableRow(new Label { Text = UI.Conductor, }, new TableCell(tbConductorTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.TrackNumber, },
                    new TableCell(tbTrackNumberTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.TrackTotal, },
                    new TableCell(tbTrackTotalTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Year, }, new TableCell(tbYearTag) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.BitRate, }, new TableCell(tbBitRate) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.AudioChannels, },
                    new TableCell(tbAudioChannels) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.SampleRate, }, new TableCell(tbSampleRate) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Duration, }, new TableCell(tbDuration) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.AudioFormat, }, new TableCell(tbAudioFormat) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.FileName, }, new TableCell(tbFileName) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.FullFileName, },
                    new TableCell(tbFileNameFull) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.FileSizeBytes, },
                    new TableCell(tbFileSizeBytes) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Lyrics, }, new TableCell(tbLyricsTag) { ScaleWidth = true, }),
                new TableRow { ScaleHeight = true, },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };

        var layoutImageTag = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Panel
                        {
                            Content = imageViewTag,
                            Padding = 20,
                            Size = new Size(300, 300),
                        },
                    },
                },
                new TableRow { ScaleHeight = true, },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };

        tabFileTag.Content = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(tlTrackDataTag) { ScaleWidth = true, },
                        layoutImageTag,
                    },
                },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };
    }

    private void LayoutAmpData()
    {
        var tlTrackDataAmp = new TableLayout
        {
            Rows =
            {
                new TableRow(new Label { Text = UI.Album, }, new TableCell(tbAlbumAmp) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Artist, }, new TableCell(tbArtistAmp) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Title, }, new TableCell(tbTitleAmp) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.TrackNumber, },
                    new TableCell(tbTrackNumberAmp) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Year, }, new TableCell(tbYearAmp) { ScaleWidth = true, }),
                new TableRow(new Label { Text = UI.Lyrics, }, new TableCell(tbLyricsAmp) { ScaleWidth = true, }),
                new TableRow { ScaleHeight = true, },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };

        var layoutImageAmp = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Panel
                        {
                            Content = imageViewAmp,
                            Padding = 20,
                            Size = new Size(300, 300),
                        },
                    },
                },
                new TableRow { ScaleHeight = true, },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };

        tabAmpTag.Content = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(tlTrackDataAmp) { ScaleWidth = true, },
                        layoutImageAmp,
                    },
                },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };
    }

    private void LayoutRest()
    {
        tcTagTabs.Pages.Add(tabFileTag);
        tcTagTabs.Pages.Add(tabAmpTag);
        Content = tcTagTabs;

        PositiveButtons.Add(btnClose);

        btnCopyToAmp.Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_arrow_hook_up_right_20_filled, Globals.MenuImageDefaultSize);
        btnCopyToTag.Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_arrow_hook_up_left_20_filled, Globals.MenuImageDefaultSize);
        btnCopyToAmp.Click += CopyToAmpClick;
        btnCopyToTag.Click += CopyToTagClick;

        ToolBar = new ToolBar
        {
            Items =
            {
                btnCopyToAmp,
                btnCopyToTag,
            },
        };

        tcTagTabs.SelectedIndexChanged += TcTagTabs_SelectedIndexChanged;
    }

    /// <summary>
    /// Occurs when audio trackTag data has been changed.
    /// </summary>
    public event EventHandler<AudioTrackChangedEventArgs>? AudioTrackChanged;

    private readonly ButtonToolItem btnCopyToAmp = new() { ToolTip = UI.CopyTagDataToAmp, };
    private readonly ButtonToolItem btnCopyToTag = new() { ToolTip = UI.CopyAmpDataToTag, Visible = false, };

    private void TcTagTabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        btnCopyToAmp.Visible = tcTagTabs.SelectedIndex == 0;
        btnCopyToTag.Visible = tcTagTabs.SelectedIndex == 1;
    }

    private void CopyToAmpClick(object? sender, EventArgs e)
    {
        imageViewAmp.Image = new Bitmap(imageViewTag.Image);
        tbArtistAmp.Text = tbArtistTag.Text;
        tbAlbumAmp.Text = tbAlbumTag.Text;
        tbTrackNumberAmp.Text = tbTrackNumberTag.Text;
        tbYearAmp.Text = tbYearTag.Text;
        tbLyricsAmp.Text = tbLyricsTag.Text;
        tbTitleAmp.Text = tbTitleTag.Text;
        tcTagTabs.SelectedIndex = 1;
    }

    private void CopyToTagClick(object? sender, EventArgs e)
    {
        if (databaseImageExists)
        {
            if (embeddedImageIndex != -1)
            {
                embeddedImages[embeddedImageIndex] = new Bitmap(imageViewAmp.Image);
            }
            else
            {
                embeddedImages.Add(new Bitmap(imageViewAmp.Image));
                embeddedImageIndex = 0;
            }
        }

        if (embeddedImageIndex != -1)
        {
            imageViewTag.Image = embeddedImages[embeddedImageIndex];
        }

        tbArtistTag.Text = tbArtistAmp.Text;
        tbAlbumTag.Text = tbAlbumAmp.Text;
        tbTrackNumberTag.Text = tbTrackNumberAmp.Text;
        tbYearTag.Text = tbYearAmp.Text;
        tbLyricsTag.Text = tbLyricsAmp.Text;
        tbTitleTag.Text = tbTitleAmp.Text;
        tcTagTabs.SelectedIndex = 0;
    }

    private readonly TabControl tcTagTabs = new();
    private readonly TabPage tabFileTag = new() { Text = UI.FileDataTag, };
    private readonly TabPage tabAmpTag = new() { Text = UI.FileDataAmp, };
    private readonly Button btnClose = new() { Text = UI.Close, };
    private readonly ImageView imageViewTag = new();
    private readonly TextBox tbAlbumTag = new();
    private readonly TextBox tbArtistTag = new();
    private readonly TextBox tbTitleTag = new();
    private readonly TextBox tbCommentTag = new();
    private readonly TextBox tbAlbumArtistTag = new();
    private readonly TextBox tbOriginalArtistTag = new();
    private readonly TextBox tbCopyrightTag = new();
    private readonly TextBox tbGenreTag = new();
    private readonly TextBox tbConductorTag = new();
    private readonly NumericMaskedTextBox<int?> tbTrackNumberTag = new();
    private readonly NumericMaskedTextBox<int?> tbTrackTotalTag = new();
    private readonly NumericMaskedTextBox<int> tbYearTag = new();
    private readonly TextBox tbBitRate = new() { ReadOnly = true, };
    private readonly TextBox tbAudioChannels = new() { ReadOnly = true, };
    private readonly TextBox tbSampleRate = new() { ReadOnly = true, };
    private readonly TextBox tbDuration = new() { ReadOnly = true, };
    private readonly TextBox tbAudioFormat = new() { ReadOnly = true, };
    private readonly TextBox tbFileName = new() { ReadOnly = true, };
    private readonly TextBox tbFileNameFull = new() { ReadOnly = true, };
    private readonly TextBox tbFileSizeBytes = new() { ReadOnly = true, };
    private readonly TextBox tbLyricsTag = new() { ReadOnly = true, };

    private readonly ImageView imageViewAmp = new();
    private readonly TextBox tbArtistAmp = new();
    private readonly TextBox tbAlbumAmp = new();
    private readonly NumericMaskedTextBox<int?> tbTrackNumberAmp = new();
    private readonly NumericMaskedTextBox<int?> tbYearAmp = new();
    private readonly TextBox tbLyricsAmp = new();
    private readonly TextBox tbTitleAmp = new();
}