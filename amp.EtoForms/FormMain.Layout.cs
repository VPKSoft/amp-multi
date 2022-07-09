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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using amp.Database.DataModel;
using amp.EtoForms.Localization;
using amp.EtoForms.Properties;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Button = Eto.Forms.Button;
using ComboBox = Eto.Forms.ComboBox;
using Control = Eto.Forms.Control;
using Expander = Eto.Forms.Expander;
using GridView = Eto.Forms.GridView;
using Label = Eto.Forms.Label;
using Orientation = Eto.Forms.Orientation;
using Panel = Eto.Forms.Panel;
using TextBox = Eto.Forms.TextBox;

namespace amp.EtoForms;

partial class FormMain
{
    [MemberNotNull(nameof(cmbAlbumSelect))]
    private StackLayout CreateAlbumSelector()
    {
        cmbAlbumSelect = new ComboBox { ReadOnly = false, AutoComplete = true, };
        cmbAlbumSelect.ItemTextBinding = new PropertyBinding<string>(nameof(Album.AlbumName));

        cmbAlbumSelect.SelectedValueChanged += async (sender, args) =>
        {
            var id = ((Album?)cmbAlbumSelect.SelectedValue)?.Id;

            if (id != null)
            {
                CurrentAlbumId = id.Value;
                await RefreshCurrentAlbum();
            }
        };

        var imageView = new ImageView { Width = 20, Height = 20, };
        imageView.SizeChanged += delegate
        {
            var wh = Math.Min(imageView.Width, imageView.Height);
            var size = new Size(wh, wh);
            imageView.Image = EtoHelpers.ImageFromSvg(Colors.Orange, Size16.ic_fluent_music_note_2_16_filled,
                size);
        };

        var result = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                imageView,
                new Label { Text = "Album", },
                new Panel { Width = Globals.DefaultPadding, },
                new StackLayoutItem(cmbAlbumSelect, true),
            },
        };

        return result;
    }

    private StackLayout CreateToolbar()
    {
        var result = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                new Button((_, _) => {  }) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size16.ic_fluent_previous_16_filled, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                btnPlayPause,
                new Button(PlayNextSongClick) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size16.ic_fluent_next_16_filled, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#502D16"), Resources.queue_three_dots, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
//                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#D4AA00"), amp.EtoForms.Properties.Resources.shuffle_random_svgrepo_com_modified, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                btnShuffleToggle,
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#FF5555"), Resources.repeat_svgrepo_com_modified, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Colors.Navy, Resources.stack_queue_three_dots, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
            },
        };

        return result;
    }

    [MemberNotNull(nameof(songVolumeSlider), nameof(totalVolumeSlider))]
    private Control CreateValueSliders()
    {
        songVolumeSlider = new VolumeSlider((_, args) =>
            {
                playbackManager.PlaybackVolume = args.Value / 100.0;
            })
        { Maximum = 300, };

        totalVolumeSlider = new VolumeSlider((_, args) =>
            {
                var volume = args.Value / 100.0;
                playbackManager.MasterVolume = volume;
                Globals.Settings.MasterVolume = volume;
                Globals.SaveSettings();
            })
        { Maximum = 100, ColorSlider = Colors.CornflowerBlue, };

        var tableLayout = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = UI.Volume, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(totalVolumeSlider, true),
                    },
                },
                new Panel {Height = Globals.DefaultPadding,},
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = UI.SongVolume, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(songVolumeSlider, true),
                    },
                },
                new Panel {Height = Globals.DefaultPadding,},
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = UI.Rating, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(new RatingSlider { Value = 50,}, true),
                    },
                },
            },
            Height = 120 + Globals.DefaultPadding * 4,
            Padding = Globals.DefaultPadding,
        };

        var result = new Expander
        {
            Content = tableLayout,
            Header = new Label { Text = UI.SoundRating, },
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

    [MemberNotNull(nameof(playbackPosition), nameof(lbPlaybackPosition), nameof(gvSongs), nameof(cmbAlbumSelect))]
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
            AllowMultipleSelection = true,
        };

        var result = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = CreateAlbumSelector(), Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = songAdjustControls, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
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
        btnPlayPause = new CheckedButton(Size16.ic_fluent_pause_16_filled,
            Size16.ic_fluent_play_16_filled, Colors.Purple, Colors.Purple,
            Globals.ButtonDefaultSize);

        btnShuffleToggle = new CheckedButton(Resources.shuffle_random_svgrepo_com_modified,
            Color.Parse("#D4AA00"), Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize, true);
    }

    private void CreateMenu()
    {
        quitCommand.Image =
            EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_arrow_exit_20_filled, Globals.MenuImageDefaultSize);

        aboutCommand.Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_question_circle_20_filled,
            Globals.MenuImageDefaultSize);

        // create menu
        base.Menu = new MenuBar
        {
            Items =
            {
                // File submenu
                new SubMenuItem { Text = UI.TestStuff, Items = { testStuff, }, Visible = Debugger.IsAttached, },
            },
            ApplicationItems =
            {
                // application (OS X) or file menu (others)
                new SubMenuItem
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_collections_add_20_filled, Globals.MenuImageDefaultSize),
                    Text = UI.AddMusicFiles,
                    Items =
                    {
                        addFilesToDatabase,
                        addFilesToAlbum,
                        addDirectoryToDatabase,
                        addDirectoryToAlbum,
                    },
                },
            },
            QuitItem = quitCommand,
            AboutItem = aboutCommand,
        };

        testStuff.Executed += TestStuff_Executed;
        aboutCommand.Executed += (_, _) => new AboutDialog().ShowDialog(this);
        quitCommand.Executed += (_, _) => Application.Instance.Quit();
        addFilesToDatabase.Executed += AddFilesToDatabase_Executed;
        addFilesToAlbum.Executed += AddFilesToDatabase_Executed;
        addDirectoryToDatabase.Executed += AddDirectoryToDatabase_Executed;
        addDirectoryToAlbum.Executed += AddDirectoryToDatabase_Executed;
    }

    private GridView gvSongs;
    private readonly TextBox tbSearch = new();
    private CheckedButton btnPlayPause;
    private readonly Label lbSongsTitle = new();
    private VolumeSlider songVolumeSlider;
    private VolumeSlider totalVolumeSlider;
    private readonly Command commandPlayPause = new();
    private readonly Command nextSongCommand = new();
    private CheckedButton btnShuffleToggle;
    private readonly StackLayout toolBar;
    private readonly Control songAdjustControls;
    private PositionSlider playbackPosition;
    private Label lbPlaybackPosition;
    private readonly Command quitCommand = new() { MenuText = UI.Quit, Shortcut = Application.Instance.CommonModifier | Keys.Q, };
    private readonly Command aboutCommand = new() { MenuText = UI.About, };
    private readonly Command testStuff = new() { MenuText = UI.TestStuff, };
    private readonly Command addFilesToDatabase = new() { MenuText = UI.AddFiles, };
    private readonly Command addFilesToAlbum = new() { MenuText = UI.AddFilesToAlbum, };
    private readonly Command addDirectoryToDatabase = new() { MenuText = UI.AddFolderContents, };
    private readonly Command addDirectoryToAlbum = new() { MenuText = UI.AddFolderContentsToAlbum, };
    private ComboBox cmbAlbumSelect;
}