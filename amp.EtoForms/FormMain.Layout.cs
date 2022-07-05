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

using System.Diagnostics.CodeAnalysis;
using amp.Database.DataModel;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Utilities;

namespace amp.EtoForms;

partial class FormMain
{
    private StackLayout CreateToolbar()
    {
        var result = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                new Button((_, _) => {  }) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, FluentIcons.Resources.Filled.Size16.ic_fluent_previous_16_filled, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                btnPlayPause,
                new Button(PlayNextSongClick) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, FluentIcons.Resources.Filled.Size16.ic_fluent_next_16_filled, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#502D16"), amp.EtoForms.Properties.Resources.queue_three_dots, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
//                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#D4AA00"), amp.EtoForms.Properties.Resources.shuffle_random_svgrepo_com_modified, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                btnShuffleToggle,
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#FF5555"), amp.EtoForms.Properties.Resources.repeat_svgrepo_com_modified, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Colors.Navy, amp.EtoForms.Properties.Resources.stack_queue_three_dots, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
            },
        };

        return result;
    }

    [MemberNotNull(nameof(songVolumeSlider))]
    private TableLayout CreateVolumeSliders()
    {
        songVolumeSlider = new VolumeSlider((_, args) =>
            {
                playbackManager!.PlaybackVolume = args.Value / 100.0;
            })
        { Maximum = 300, };

        var result = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = amp.EtoForms.Localization.UI.Volume, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(new VolumeSlider(), true),
                    },
                },
                new Panel {Height = Globals.DefaultPadding,},
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = amp.EtoForms.Localization.UI.SongVolume, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(songVolumeSlider, true),
                    },
                },
            },
            Height = 80 + Globals.DefaultPadding * 3,
            Padding = Globals.DefaultPadding,
        };

        return result;
    }

    private void AssignEventListeners()
    {
        btnShuffleToggle.CheckedChange += BtnShuffleToggle_CheckedChange;
        btnPlayPause.CheckedChange += PlayPauseToggle;
        nextSongCommand.Executed += NextSongCommand_Executed;
        tbSearch.TextChanged += TbSearch_TextChanged;
        gvSongs.MouseDoubleClick += GvSongsMouseDoubleClick;
        Closing += FormMain_Closing;
        KeyDown += FormMain_KeyDown;
        gvSongs.KeyDown += FormMain_KeyDown;
        tbSearch.KeyDown += FormMain_KeyDown;
    }

    [MemberNotNull(nameof(playbackPosition), nameof(lbPlaybackPosition), nameof(gvSongs))]
    private StackLayout CreateMainContent()
    {
        playbackPosition = new PositionSlider { Height = 20, };

        playbackPosition.ValueChanged += PlaybackPosition_ValueChanged;

        lbPlaybackPosition = new Label();

        var stackLayout = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(playbackPosition, true),
                        new Panel{Width = Globals.DefaultPadding,},
                        new TableCell(lbPlaybackPosition),
                    },
                },
            },
        };

        gvSongs = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(AlbumSong.DisplayName)), Expand = true,
                },
            },
            ShowHeader = false,
            Height = 650,
            Width = 550,
        };

        var result = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = mainVolumeSlider, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = stackLayout,}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = lbSongsTitle, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = tbSearch, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = gvSongs, Padding = new Padding(Globals.DefaultPadding, 2), }, HorizontalAlignment.Stretch) { Expand = true,},
            },
            Padding = new Padding(Globals.WindowBorderWidth, Globals.DefaultPadding),
        };

        return result;
    }

    [MemberNotNull(nameof(btnPlayPause), nameof(btnShuffleToggle))]
    private void CreateButtons()
    {
        btnPlayPause = new CheckedButton(FluentIcons.Resources.Filled.Size16.ic_fluent_pause_16_filled,
            FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled, Colors.Purple, Colors.Purple,
            Globals.ButtonDefaultSize);

        btnShuffleToggle = new CheckedButton(amp.EtoForms.Properties.Resources.shuffle_random_svgrepo_com_modified,
            Color.Parse("#D4AA00"), Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize, true);
    }

    private GridView gvSongs;
    private readonly TextBox tbSearch = new();
    private CheckedButton btnPlayPause;
    private readonly Label lbSongsTitle = new();
    private VolumeSlider songVolumeSlider;
    private readonly Command commandPlayPause = new();
    private readonly Command nextSongCommand = new();
    private CheckedButton btnShuffleToggle;
    private readonly StackLayout toolBar;
    private readonly TableLayout mainVolumeSlider;
    private PositionSlider playbackPosition;
    private Label lbPlaybackPosition;
}