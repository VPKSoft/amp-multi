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

using amp.Shared.Constants;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;

namespace amp.EtoForms.Forms;

partial class FormDialogTrackInfo
{
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
            Width = 550,
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
            Width = 550,
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

        var color = Color.Parse(Globals.ColorConfiguration.ColorToolButtonImage);

        btnCopyToAmp.Image = EtoHelpers.ImageFromSvg(color, Size20.ic_fluent_arrow_hook_up_right_20_filled,
            Globals.MenuImageDefaultSize);
        btnCopyToTag.Image = EtoHelpers.ImageFromSvg(color, Size20.ic_fluent_arrow_hook_up_left_20_filled,
            Globals.MenuImageDefaultSize);
        btnSaveChanges.Image =
            EtoHelpers.ImageFromSvg(color, Size20.ic_fluent_save_20_filled, Globals.MenuImageDefaultSize);
        btnLoadImage.Image = EtoHelpers.ImageFromSvg(color, Size20.ic_fluent_image_add_20_filled, Globals.MenuImageDefaultSize);
        btnClearImage.Image = EtoHelpers.ImageFromSvg(color, Size20.ic_fluent_image_off_20_filled, Globals.MenuImageDefaultSize);


        btnCopyToAmp.Click += CopyToAmpClick;
        btnCopyToTag.Click += CopyToTagClick;
        btnSaveChanges.Click += BtnSaveChanges_Click;
        btnLoadImage.Click += BtnLoadImage_Click;
        btnClearImage.Click += BtnClearImage_Click;
        btnClose.Click += BtnClose_Click;

        ToolBar = new ToolBar
        {
            Items =
            {
                btnCopyToAmp,
                btnCopyToTag,
                btnSaveChanges,
                btnLoadImage,
                btnClearImage,
            },
        };

        tcTagTabs.SelectedIndexChanged += TcTagTabs_SelectedIndexChanged;
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BtnClearImage_Click(object? sender, EventArgs e)
    {
        copiedFromTag = null;
        imageViewAmp.Image = imageOff;
        AmpDataChanged = true;
    }

    private void BtnLoadImage_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { MultiSelect = false, };
        dialog.Filters.Add(new FileFilter(UI.ImageFiles, ImageConstants.SupportedExtensionArray));
        if (dialog.ShowDialog(this) != DialogResult.Ok)
        {
            return;
        }

        copiedFromTag = new Bitmap(dialog.FileName);
        AmpDataChanged = true;
        imageViewAmp.Image = new Bitmap(copiedFromTag, 300, 300);
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