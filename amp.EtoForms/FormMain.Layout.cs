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
using amp.EtoForms.Layout;
using amp.EtoForms.Models;
using amp.EtoForms.Properties;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Utilities;
using EtoForms.FormPositions;
using EtoForms.SpectrumVisualizer;
using FluentIcons.Resources.Filled;
using ManagedBass;
using ManagedBass.FftSignalProvider;

namespace amp.EtoForms;

partial class FormMain
{
    [MemberNotNull(nameof(cmbAlbumSelect))]
    private StackLayout CreateAlbumSelector()
    {
        cmbAlbumSelect = ReusableControls.CreateAlbumSelectCombo((id) =>
        {
            if (id != null)
            {
                CurrentAlbumId = id.Value;
                RefreshCurrentAlbum();
                playbackManager.ResetPlaybackHistory();
            }

            return Task.CompletedTask;
        }, context, Globals.Settings.SelectedAlbum);

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
                new Panel { Width = Globals.DefaultPadding, },
                new StackLayoutItem(new Label { Text = UI.Album, }, VerticalAlignment.Center),
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
                btnPreviousTrack,
                btnPlayPause,
                new Button(PlayNextAudioTrackClick) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size16.ic_fluent_next_16_filled, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                new Panel {Width =  Globals.DefaultPadding,},
                btnShowQueue,
                new Panel {Width =  Globals.DefaultPadding,},
//                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#D4AA00"), amp.EtoForms.Properties.Resources.shuffle_random_svgrepo_com_modified, Globals.ButtonDefaultSize), Size = Globals.ButtonDefaultSize, },
                btnShuffleToggle,
                btnRepeatToggle,
                new Panel {Width =  Globals.DefaultPadding,},
                btnStackQueueToggle,
            },
        };

        return result;
    }

    private Expander CreateValueSliders()
    {
        var tableLayout = new TableLayout
        {
            Rows =
            {
                new Panel {Height = Globals.DefaultPadding,},
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
                        new TableCell(new Label { Text = UI.TrackVolume, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(trackVolumeSlider, true),
                    },
                },
                new Panel {Height = Globals.DefaultPadding,},
                new TableRow
                {
                    Cells =
                    {
                        new TableCell(new Label { Text = UI.Rating, VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                        new Panel { Width = Globals.DefaultPadding,},
                        new TableCell(trackRatingSlider, true),
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

        result.ExpandedChanged += Result_ExpandedChanged;

        return result;
    }

    [MemberNotNull(nameof(playbackPosition), nameof(lbPlaybackPosition), nameof(gvAudioTracks), nameof(cmbAlbumSelect), nameof(audioVisualizationControl))]
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

        gvAudioTracks = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(AlbumTrack.DisplayName)), Expand = true,
                    HeaderText = UI.Track,
                    Resizable = false,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((AlbumTrack s) => s.QueueIndex)
                            .Convert(q => q == 0 ? null : q.ToString())
                            .Cast<string?>(),
                    },
                    HeaderText = UI.QueueShort,
                    Resizable = false,
                },
                new GridColumn
                {
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding
                            .Property((AlbumTrack s) => s.QueueIndexAlternate)
                            .Convert(qa => qa == 0 ? null : qa.ToString())
                            .Cast<string?>(),
                    },
                    HeaderText = UI.StarChar,
                    Resizable = false,
                },
            },
            ShowHeader = Globals.Settings.DisplayPlaylistHeader,
            Height = 650,
            Width = 550,
            AllowMultipleSelection = true,
            AllowColumnReordering = true,
        };

        audioVisualizationControl = Globals.Settings.DisplayAudioVisualization
            ? new Panel
            {
                Content = spectrumAnalyzer,
                Height = 100,
                Padding = new Padding(Globals.DefaultPadding, 2),
            }
            : new Panel();

        audioVisualizationControl.Visible = false;

        var result = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = CreateAlbumSelector(), Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = trackAdjustControls, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = stackLayout,}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = lbTracksTitle, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = tbSearch, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = gvAudioTracks, Padding = new Padding(Globals.DefaultPadding, 2), }, HorizontalAlignment.Stretch) { Expand = true,},
                new StackLayoutItem(new Panel { Content = audioVisualizationControl,}, HorizontalAlignment.Stretch),
                CreateStatusBar(),
            },
            Padding = new Padding(Globals.WindowBorderWidth, Globals.DefaultPadding),
        };

        if (Globals.Settings.DisplayAudioVisualization)
        {
            spectrumAnalyzer.SignalProvider = new SignalProvider(DataFlags.FFT1024, true, false)
            { WindowType = WindowType.Hanning, };
        }

        return result;
    }

    private readonly SpectrumVisualizer spectrumAnalyzer = new(true) { Width = 50, Height = 100, BackgroundColor = Colors.Black, };

    [MemberNotNull(nameof(btnPlayPause), nameof(btnShuffleToggle), nameof(btnShowQueue), nameof(btnRepeatToggle), nameof(btnStackQueueToggle), nameof(btnPreviousTrack))]
    private void CreateButtons()
    {
        btnPlayPause = new CheckedButton(Size16.ic_fluent_pause_16_filled,
            Size16.ic_fluent_play_16_filled, Colors.Purple, Colors.Purple,
            Globals.ButtonDefaultSize);

        btnShuffleToggle = new CheckedButton(Resources.shuffle_random_svgrepo_com_modified,
            Color.Parse("#D4AA00"), Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize, true);

        btnShowQueue = new CheckedButton(Resources.queue_three_dots,
                Color.Parse("#502D16"), Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize)
        { ToolTip = UI.ShowQueue, };

        btnRepeatToggle = new CheckedButton(Resources.repeat_svgrepo_com_modified,
            Color.Parse("#FF5555"), Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize, true);

        btnStackQueueToggle = new CheckedButton(Resources.stack_queue_three_dots,
            Colors.Navy, Color.Parse("#B6BCB6"), Globals.ButtonDefaultSize);

        btnPreviousTrack = new SvgImageButton(PlayPreviousClick)
        {
            Image = Size16.ic_fluent_previous_16_filled,
            ImageSize = Globals.ButtonDefaultSize,
            SolidImageColor = Colors.Teal,
            Enabled = false,
        };
    }

    private void CreateMenu()
    {
        quitCommand.Image =
            EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_arrow_exit_20_filled, Globals.MenuImageDefaultSize);

        aboutCommand.Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_question_circle_20_filled,
            Globals.MenuImageDefaultSize);

        commandPlayPause.MenuText = UI.Play;
        commandPlayPause.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_play_16_filled, Globals.ButtonDefaultSize);

        settingsCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_settings_16_filled, Globals.ButtonDefaultSize);

        manageAlbumsCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_music_note_2_16_filled, Globals.ButtonDefaultSize);

        saveQueueCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_save_16_filled, Globals.ButtonDefaultSize);

        manageSavedQueues.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Resources.queue_three_dots, Globals.ButtonDefaultSize);

        clearQueueCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Resources.queue_three_dots_clear, Globals.ButtonDefaultSize);

        scrambleQueueCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_re_order_dots_vertical_16_filled, Globals.ButtonDefaultSize);

        trackInfoCommand.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            Size16.ic_fluent_info_16_filled, Globals.ButtonDefaultSize);

        var addFilesSubMenu = new SubMenuItem
        {
            Image = EtoHelpers.ImageFromSvg(Colors.Teal, Size20.ic_fluent_collections_add_20_filled,
                Globals.MenuImageDefaultSize),
            Text = UI.AddMusicFiles,
        };


        if (!Globals.Settings.HideAddFilesToNonAlbum)
        {
            addFilesSubMenu.Items.Add(addFilesToDatabase);
        }
        addFilesSubMenu.Items.Add(addFilesToAlbum);
        if (!Globals.Settings.HideAddFilesToNonAlbum)
        {
            addFilesSubMenu.Items.Add(addDirectoryToDatabase);
        }
        addFilesSubMenu.Items.Add(addDirectoryToAlbum);

        // create menu
        base.Menu = new MenuBar
        {
            Items =
            {
                // File submenu
                new SubMenuItem { Text = UI.TestStuff, Items = { testStuff, }, Visible = Debugger.IsAttached, },
                new SubMenuItem { Text = UI.Queue, Items = { saveQueueCommand, manageSavedQueues, clearQueueCommand, scrambleQueueCommand,},},
            },
            ApplicationItems =
            {
                // application (OS X) or file menu (others)
                addFilesSubMenu,
                manageAlbumsCommand,
                trackInfoCommand,
                settingsCommand,
            },
            QuitItem = quitCommand,
            AboutItem = aboutCommand,
        };

        FillAboutDialogData();

        testStuff.Executed += TestStuff_Executed;
        aboutCommand.Executed += (_, _) => aboutDialog.ShowDialog(this);
        quitCommand.Executed += (_, _) => Application.Instance.Quit();
        addFilesToDatabase.Executed += AddFilesToDatabase_Executed;
        addFilesToAlbum.Executed += AddFilesToDatabase_Executed;
        addDirectoryToDatabase.Executed += AddDirectoryToDatabase_Executed;
        addDirectoryToAlbum.Executed += AddDirectoryToDatabase_Executed;
        manageAlbumsCommand.Executed += ManageAlbumsCommand_Executed;
        saveQueueCommand.Executed += SaveQueueCommand_Executed;
        manageSavedQueues.Executed += ManageSavedQueues_Executed;
        clearQueueCommand.Executed += ClearQueueCommand_Executed;
        scrambleQueueCommand.Executed += ScrambleQueueCommand_Executed;
        trackInfoCommand.Executed += TrackInfoCommand_Executed;
    }

    private Control CreateStatusBar()
    {
        return new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        lbQueueCountText,
                        new Panel { Width = Globals.DefaultPadding,},
                        lbQueueCountValue,
                        new Panel { Width = Globals.DefaultPadding,},
                        lbTrackCount,
                        new Panel { Width = Globals.DefaultPadding,},
                        lbTrackCountValue,
                        new Panel { Width = Globals.DefaultPadding,},
                        lbLoadingText,
                        progressLoading,
                        new TableCell(lbStatusMessage) { ScaleWidth = true,},
                    },
                },
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };
    }

    private readonly AboutDialog aboutDialog = new();
    private GridView gvAudioTracks;
    private readonly TextBox tbSearch = new();
    private CheckedButton btnPlayPause;
    private SvgImageButton btnPreviousTrack;
    private readonly Label lbTracksTitle = new();
    private readonly VolumeSlider trackVolumeSlider = new() { Maximum = 300, };
    private readonly VolumeSlider totalVolumeSlider = new() { Maximum = 100, ColorSlider = Colors.CornflowerBlue, };
    private readonly RatingSlider trackRatingSlider = new() { Maximum = 1000, Value = 500, };
    private readonly Command commandPlayPause = new();
    private readonly Command nextAudioTrackCommand = new();
    private CheckedButton btnShuffleToggle;
    private CheckedButton btnRepeatToggle;
    private CheckedButton btnShowQueue;
    private readonly StackLayout toolBar;
    private readonly Expander trackAdjustControls;
    private PositionSlider playbackPosition;
    private Label lbPlaybackPosition;
    private readonly Command clearQueueCommand = new()
    { MenuText = UI.ClearQueue, Shortcut = Application.Instance.CommonModifier | Keys.D, };
    private readonly Command quitCommand = new() { MenuText = UI.Quit, Shortcut = Application.Instance.CommonModifier | Keys.Q, };
    private readonly Command aboutCommand = new() { MenuText = UI.About, };
    private readonly Command settingsCommand = new() { MenuText = UI.Settings, };
    private readonly Command testStuff = new() { MenuText = UI.TestStuff, };
    private readonly Command addFilesToDatabase = new() { MenuText = UI.AddFiles, };
    private readonly Command addFilesToAlbum = new() { MenuText = UI.AddFilesToAlbum, };
    private readonly Command addDirectoryToDatabase = new() { MenuText = UI.AddFolderContents, };
    private readonly Command addDirectoryToAlbum = new() { MenuText = UI.AddFolderContentsToAlbum, };
    private readonly Command manageAlbumsCommand = new() { MenuText = UI.Albums, };
    private readonly Command saveQueueCommand = new() { MenuText = UI.SaveCurrentQueue, };
    private readonly Command manageSavedQueues = new() { MenuText = UI.SavedQueues, Shortcut = Keys.F3, };
    private readonly Command scrambleQueueCommand = new() { MenuText = UI.ScrambleQueue, Shortcut = Keys.F7, };
    private readonly Command trackInfoCommand = new() { MenuText = UI.TrackInformation, Shortcut = Keys.F4, };
    private ComboBox cmbAlbumSelect = new();
    private CheckedButton btnStackQueueToggle;
    private Control audioVisualizationControl;

    // Status bar controls:
    private readonly Label lbQueueCountText = new() { Text = UI.QueueCount, };
    private readonly Label lbQueueCountValue = new();
    private readonly Label lbStatusMessage = new();
    private readonly Label lbLoadingText = new() { Text = Messages.LoadingPercentage, Visible = false, };
    private readonly ProgressBar progressLoading = new() { Visible = false, };
    private readonly Label lbTrackCount = new() { Text = UI.Tracks, };
    private readonly Label lbTrackCountValue = new();

    // Position
    private readonly FormSaveLoadPosition positionSaveLoad;
    private bool loadingPosition;
    private bool positionsLoaded;
    private readonly UITimer timer = new();
    private DateTime positionLastChanged;
}