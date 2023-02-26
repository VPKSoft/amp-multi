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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using amp.EtoForms.Properties;
using amp.EtoForms.Settings;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Drawing;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using ManagedBass;
using ManagedBass.FftSignalProvider;
using Album = amp.DataAccessLayer.DtoClasses.Album;
using AlbumTrack = amp.DataAccessLayer.DtoClasses.AlbumTrack;

namespace amp.EtoForms;

partial class FormMain
{
    private Control CreateAlbumSelector()
    {
        albums = context.Albums.Select(f => DataAccessLayer.Globals.AutoMapper.Map<Album>(f)).ToList();
        cmbAlbumSelect.DataStore = albums;
        cmbAlbumSelect.SelectedIndex = albums.FindIndex(f => f.Id == CurrentAlbumId);
        cmbAlbumSelect.SelectedIndexChanged += CmbAlbumSelect_SelectedIndexChanged;

        var imageView = new ImageView { Width = 20, Height = 20, };
        imageView.SizeChanged += delegate
        {
            var wh = Math.Min(imageView.Width, imageView.Height);
            var size = new Size(wh, wh);
            imageView.Image = CreateImage(Globals.CustomIconSettings.AlbumLookup, Size16.ic_fluent_music_note_2_16_filled, Color.Parse(Globals.ColorConfiguration.TheMusicNoteColor), size);
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
                btnNextTrack,
                new Panel {Width =  Globals.DefaultPadding,},
                btnShowQueue,
                new Panel {Width =  Globals.DefaultPadding,},
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
        totalVolumeSlider.SpeakerImageSvg =
            Globals.CustomIconSettings.MainVolumeSliderSpeaker?.IconData ?? Size16.ic_fluent_speaker_2_16_filled;
        totalVolumeSlider.ColorSpeaker = MakeColor(Globals.CustomIconSettings.MainVolumeSliderSpeaker,
            Globals.ColorConfiguration.ColorSpeakerMainVolume);

        totalVolumeSlider.SliderMarkerImageSvg = Globals.CustomIconSettings.MainVolumeSliderSpeaker?.IconData ??
                                                 global::EtoForms.Controls.Custom.Properties.Resources.slider_mark;
        totalVolumeSlider.ColorSliderMarker = MakeColor(Globals.CustomIconSettings.MainVolumeSlider,
            Globals.ColorConfiguration.ColorMainVolumeValueIndicator);


        trackVolumeSlider.SpeakerImageSvg =
            Globals.CustomIconSettings.TrackVolumeSliderSpeaker?.IconData ?? Size16.ic_fluent_speaker_2_16_filled;
        trackVolumeSlider.ColorSpeaker = MakeColor(Globals.CustomIconSettings.TrackVolumeSliderSpeaker,
            Globals.ColorConfiguration.ColorSpeakerTrackVolume);

        trackVolumeSlider.SliderMarkerImageSvg = Globals.CustomIconSettings.TrackVolumeSlider?.IconData ??
                                           global::EtoForms.Controls.Custom.Properties.Resources.slider_mark;
        trackVolumeSlider.ColorSliderMarker = MakeColor(Globals.CustomIconSettings.TrackVolumeSlider,
            Globals.ColorConfiguration.ColorTrackVolumeValueIndicator);

        trackRatingSlider.SliderImageUndefinedSvg = Globals.CustomIconSettings.RatingUndefined?.IconData ??
                                                    Size16.ic_fluent_star_16_filled;

        trackRatingSlider.ColorSliderUndefined = MakeColor(Globals.CustomIconSettings.RatingUndefined,
            Globals.ColorConfiguration.ColorRatingSliderValueIndicatorUndefined);

        trackRatingSlider.SliderImageSvg =
            Globals.CustomIconSettings.Rating?.IconData ?? Size16.ic_fluent_star_16_filled;
        trackRatingSlider.ColorSlider = MakeColor(Globals.CustomIconSettings.Rating,
            Globals.ColorConfiguration.ColorRatingSlider);

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

    [MemberNotNull(nameof(playbackPosition), nameof(lbPlaybackPosition), nameof(gvAudioTracks),
        nameof(audioVisualizationControl), nameof(btnClearSearch))]
    private StackLayout CreateMainContent()
    {
        playbackPosition = new PositionSlider
        {
            Height = 20,
            ColorSlider = Color.Parse(Globals.ColorConfiguration.ColorPositionSlider),
            ColorSliderMarker = Color.Parse(Globals.ColorConfiguration.ColorPositionSliderValueIndicator),
        };

        playbackPosition.SliderMarkerImageSvg = Globals.CustomIconSettings.PositionSlider?.IconData ??
                                                global::EtoForms.Controls.Custom.Properties.Resources.slider_mark;
        playbackPosition.ColorSliderMarker = MakeColor(Globals.CustomIconSettings.PositionSlider,
            Globals.ColorConfiguration.ColorPositionSliderValueIndicator);

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
                        new Panel { Width = Globals.DefaultPadding, },
                        new TableCell(lbPlaybackPosition),
                    },
                },
            },
        };

        gvAudioTracks = new GridView
        {
            Columns =
            {
                columnTrackName,
                columnTrackRating,
                columnQueueIndex,
                columnAlternateQueueIndex,
            },
            ShowHeader = Globals.Settings.DisplayPlaylistHeader,
            AllowMultipleSelection = true,
            AllowColumnReordering = true,
        };

        if (!Globals.Settings.DisplayRatingColumn)
        {
            gvAudioTracks.Columns.Remove(columnTrackRating);
        }

        if (Globals.Settings.DisplayRatingColumn)
        {
            painter = new CellPainterRange<AlbumTrack>(gvAudioTracks, columnTrackRating, 1000,
                track => (track.AudioTrack!.Rating ?? 500, track.AudioTrack!.RatingSpecified))
            {
                SvgImageBytes = Globals.CustomIconSettings.RatingPlaylist?.IconData ?? Size16.ic_fluent_star_16_filled,
                ForegroundColor = Color.Parse(Globals.ColorConfiguration.ColorRatingPlaylist),
                ForegroundColorUndefined = Color.Parse(Globals.ColorConfiguration.ColorRatingPlaylistUndefined),
                SvgImageBytesUndefined = Globals.CustomIconSettings.RatingPlaylistUndefined?.IconData ?? Size16.ic_fluent_star_16_filled,
            };
        }

        gvAudioTracks.ColumnHeaderClick += GvAudioTracks_ColumnHeaderClick;

        audioVisualizationControl = CreateAudioVisualization(Globals.Settings.AudioLevelsHorizontal, Globals.Settings.DisplayAudioLevels);

        audioVisualizationControl.Visible = false;
        spectrumAnalyzer.AudioLevelsChanged += SpectrumAnalyzer_AudioLevelsChanged;
        spectrumAnalyzer.RaiseAudioLevelsChanged = true;


        var color = MakeColor(Globals.CustomIconSettings.ClearSearch,
            Globals.ColorConfiguration.ClearSearchButtonColor);

        btnClearSearch = new ImageOnlyButton(ClearSearchClick, Globals.CustomIconSettings.ClearSearch?.IconData ?? Size20.ic_fluent_eraser_20_filled) { ImageColor = color, Size = Globals.SmallImageButtonDefaultSize, ToolTip = UI.ClearSearch, };

        lbTracksTitle.Cursor = Cursors.Pointer;
        lbTracksTitle.MouseDown += LbTracksTitle_MouseDown;

        var result = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = CreateAlbumSelector(), Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = trackAdjustControls, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = stackLayout,}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = lbTracksTitle, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = new TableLayout
                    {
                        Rows =
                        {
                            new TableRow
                            {
                                Cells =
                                {
                                    new TableCell(tbSearch, true),
                                    new Panel { Width = Globals.DefaultPadding,},
                                    btnClearSearch,
                                },
                            },
                        },
                    },
                    Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = gvAudioTracks, Padding = new Padding(Globals.DefaultPadding, 2), }, HorizontalAlignment.Stretch) { Expand = true,},
                new StackLayoutItem(new Panel { Content = audioVisualizationControl,}, HorizontalAlignment.Stretch),
                CreateStatusBar(),
            },
            Padding = new Padding(Globals.WindowBorderWidth, Globals.DefaultPadding),
        };

        if (Globals.Settings.DisplayAudioVisualization)
        {
            spectrumAnalyzer.SignalProvider = new SignalProvider(DataFlags.FFT1024, true, false)
            { WindowType = (WindowType)Globals.Settings.FftWindow, };
        }

        return result;
    }

    [MemberNotNull(nameof(btnPlayPause), nameof(btnShuffleToggle), nameof(btnShowQueue), nameof(btnRepeatToggle), nameof(btnStackQueueToggle), nameof(btnPreviousTrack), nameof(btnNextTrack))]
    private void CreateButtons()
    {
        var icon = Globals.CustomIconSettings.Pause;
        var iconBytes = icon?.IconData ?? Size16.ic_fluent_pause_16_filled;
        var iconColor = MakeColor(icon, Globals.ColorConfiguration.PlayButtonPauseColor);
        icon = Globals.CustomIconSettings.Play;
        var iconBytes2 = icon?.IconData ?? Size16.ic_fluent_play_16_filled;
        var iconColor2 = MakeColor(icon, Globals.ColorConfiguration.PlayButtonPlayColor);

        btnPlayPause = new CheckedButton(iconBytes,
            iconBytes2,
            iconColor,
            iconColor2,
            Globals.ButtonDefaultSize);


        icon = Globals.CustomIconSettings.Shuffle;
        iconBytes = icon?.IconData ?? Resources.shuffle_random_svgrepo_com_modified;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.ShuffleButtonColor);

        btnShuffleToggle = new CheckedButton(iconBytes,
            iconColor,
            Color.Parse(Globals.ColorConfiguration.DisabledButtonImageColor), Globals.ButtonDefaultSize, true);


        icon = Globals.CustomIconSettings.ShowQueue;
        iconBytes = icon?.IconData ?? Resources.queue_three_dots;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.QueueButtonColor);
        btnShowQueue = new CheckedButton(iconBytes,
                iconColor,
                Color.Parse(Globals.ColorConfiguration.DisabledButtonImageColor), Globals.ButtonDefaultSize)
        { ToolTip = UI.ShowQueue, };

        icon = Globals.CustomIconSettings.Repeat;
        iconBytes = icon?.IconData ?? Resources.repeat_svgrepo_com_modified;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.RepeatButtonColor);
        btnRepeatToggle = new CheckedButton(iconBytes,
            iconColor,
            Color.Parse(Globals.ColorConfiguration.DisabledButtonImageColor), Globals.ButtonDefaultSize, true);

        icon = Globals.CustomIconSettings.StackQueue;
        iconBytes = icon?.IconData ?? Resources.stack_queue_three_dots;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.StackQueueButtonColor);
        btnStackQueueToggle = new CheckedButton(iconBytes,
            iconColor,
            Color.Parse(Globals.ColorConfiguration.DisabledButtonImageColor), Globals.ButtonDefaultSize);

        icon = Globals.CustomIconSettings.PreviousTrack;
        iconBytes = icon?.IconData ?? Size16.ic_fluent_previous_16_filled;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.PreviousButtonColor);
        btnPreviousTrack = new SvgImageButton(PlayPreviousClick)
        {
            Image = iconBytes,
            ImageSize = Globals.ButtonDefaultSize,
            SolidImageColor = iconColor,
            Enabled = false,
        };

        icon = Globals.CustomIconSettings.PreviousTrack;
        iconBytes = icon?.IconData ?? Size16.ic_fluent_next_16_filled;
        iconColor = MakeColor(icon, Globals.ColorConfiguration.NextButtonColor);
        btnNextTrack = new Button(PlayNextAudioTrackClick)
        {
            Image = EtoHelpers.ImageFromSvg(iconColor,
                iconBytes, Globals.ButtonDefaultSize),
            Size = Globals.ButtonDefaultSize,
        };
    }

    private Color MakeColor(CustomIcon? customIcon, string defaultColor)
    {
        if (customIcon != null)
        {
            if (customIcon.PreserveOriginalColor)
            {
                return default;
            }

            return customIcon.Color != null ? Color.Parse(customIcon.Color) : Color.Parse(defaultColor);
        }

        return Color.Parse(defaultColor);
    }

    private Image CreateImage(CustomIcon? customIcon, byte[] defaultIconSvgData, Color defaultColor, Size size)
    {
        if (customIcon != null)
        {
            if (customIcon.PreserveOriginalColor)
            {
                return EtoHelpers.ImageFromSvg(customIcon.IconData, size);
            }

            var color = customIcon.Color != null ? Color.Parse(customIcon.Color) : defaultColor;
            return EtoHelpers.ImageFromSvg(color, customIcon.IconData, size);
        }

        return EtoHelpers.ImageFromSvg(defaultColor, defaultIconSvgData, size);
    }

    private void CreateMenu()
    {
        var menuColor = Color.Parse(Globals.ColorConfiguration.MenuItemImageColor);
        var menuColorAlternate = Color.Parse(Globals.ColorConfiguration.MenuItemImageAlternateColor);

        quitCommand.Image = CreateImage(Globals.CustomIconSettings.QuitApplication,
            Size20.ic_fluent_arrow_exit_20_filled, menuColor, Globals.MenuImageDefaultSize);

        aboutCommand.Image = CreateImage(Globals.CustomIconSettings.HelpAbout,
            Size20.ic_fluent_question_circle_20_filled, menuColorAlternate, Globals.MenuImageDefaultSize);

        commandPlayPause.MenuText = UI.Play;
        commandPlayPause.Image = CreateImage(Globals.CustomIconSettings.Play,
            Size16.ic_fluent_play_16_filled, menuColor, Globals.ButtonDefaultSize);

        settingsCommand.Image = CreateImage(Globals.CustomIconSettings.Settings,
            Size16.ic_fluent_settings_16_filled, menuColor, Globals.ButtonDefaultSize);

        updateTrackMetadata.Image = CreateImage(Globals.CustomIconSettings.UpdateTrackMetadata,
            Size16.ic_fluent_arrow_clockwise_16_filled, menuColor, Globals.ButtonDefaultSize);

        colorSettingsCommand.Image = CreateImage(Globals.CustomIconSettings.ColorSettings,
            Size16.ic_fluent_color_16_filled, menuColor, Globals.ButtonDefaultSize);

        manageAlbumsCommand.Image = CreateImage(Globals.CustomIconSettings.Album,
            Size16.ic_fluent_music_note_2_16_filled, menuColor, Globals.ButtonDefaultSize);

        saveQueueCommand.Image = CreateImage(Globals.CustomIconSettings.SaveCurrentQueue,
            Size16.ic_fluent_save_16_filled, menuColor, Globals.ButtonDefaultSize);

        manageSavedQueues.Image = CreateImage(Globals.CustomIconSettings.SavedQueues,
            Resources.queue_three_dots, menuColor, Globals.ButtonDefaultSize);

        clearQueueCommand.Image = CreateImage(Globals.CustomIconSettings.ClearQueue,
            Resources.queue_three_dots_clear, menuColor, Globals.ButtonDefaultSize);

        scrambleQueueCommand.Image = CreateImage(Globals.CustomIconSettings.ScrambleQueue,
            Size16.ic_fluent_re_order_dots_vertical_16_filled, menuColor, Globals.ButtonDefaultSize);

        trackInfoCommand.Image = CreateImage(Globals.CustomIconSettings.TrackInformation,
            Size16.ic_fluent_info_16_filled, menuColor, Globals.ButtonDefaultSize);

        checkUpdates.Image = CreateImage(Globals.CustomIconSettings.CheckNewVersion,
            Size16.ic_fluent_arrow_download_16_filled, menuColor, Globals.ButtonDefaultSize);

        openHelp.Image = CreateImage(Globals.CustomIconSettings.Help,
            Size20.ic_fluent_book_search_20_filled, menuColor, Globals.ButtonDefaultSize);

        stashQueueCommand.Image = CreateImage(Globals.CustomIconSettings.StashQueue,
            Size20.ic_fluent_arrow_down_20_filled, menuColor, Globals.ButtonDefaultSize);

        stashPopQueueCommand.Image = CreateImage(Globals.CustomIconSettings.PopStashedQueue,
            Size20.ic_fluent_arrow_export_up_20_filled, menuColor, Globals.ButtonDefaultSize);

        iconSettingsCommand.Image = CreateImage(Globals.CustomIconSettings.IconSettings,
            Size20.ic_fluent_icons_20_filled, menuColor, Globals.ButtonDefaultSize);

        var addFilesSubMenu = new SubMenuItem
        {
            Image = CreateImage(Globals.CustomIconSettings.AddMusicFiles,
                Size20.ic_fluent_collections_add_20_filled, menuColorAlternate, Globals.MenuImageDefaultSize),
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
                new SubMenuItem { Text = UI.Queue, Items = { saveQueueCommand, manageSavedQueues, clearQueueCommand, scrambleQueueCommand, stashQueueCommand, stashPopQueueCommand, },},
                new SubMenuItem { Text = UI.Tools, Items = { settingsCommand, colorSettingsCommand, iconSettingsCommand, updateTrackMetadata, },},
                new SubMenuItem { Text = UI.Help, Items = { aboutCommand, openHelp, checkUpdates, },},
            },
            ApplicationItems =
            {
                // application (OS X) or file menu (others)
                addFilesSubMenu,
                manageAlbumsCommand,
                trackInfoCommand,
            },
            QuitItem = quitCommand,
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
        colorSettingsCommand.Executed += ColorSettingsCommand_Executed;
        updateTrackMetadata.Executed += UpdateTrackMetadata_Executed;
        checkUpdates.Executed += CheckUpdates_Executed;
        openHelp.Executed += OpenHelp_Executed;
        stashQueueCommand.Executed += StashQueueCommandExecuted;
        stashPopQueueCommand.Executed += StashPopQueueCommand_Executed;
        iconSettingsCommand.Executed += IconSettingsCommand_Executed;
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

    private readonly VolumeSlider trackVolumeSlider = new()
    {
        Maximum = 300,
        ColorSlider = Color.Parse(Globals.ColorConfiguration.ColorTrackVolumeSlider),
        ColorSliderMarker = Color.Parse(Globals.ColorConfiguration.ColorTrackVolumeValueIndicator),
        ColorSpeaker = Color.Parse(Globals.ColorConfiguration.ColorSpeakerTrackVolume),
    };

    private readonly VolumeSlider totalVolumeSlider = new()
    {
        Maximum = 100,
        ColorSlider = Color.Parse(Globals.ColorConfiguration.ColorMainVolumeSlider),
        ColorSliderMarker = Color.Parse(Globals.ColorConfiguration.ColorMainVolumeValueIndicator),
        ColorSpeaker = Color.Parse(Globals.ColorConfiguration.ColorSpeakerMainVolume),
    };

    private readonly RatingSlider trackRatingSlider = new()
    {
        Maximum = 1000,
        Value = 500,
        ColorSlider = Color.Parse(Globals.ColorConfiguration.ColorRatingSlider),
        ColorSliderMarker = Color.Parse(Globals.ColorConfiguration.ColorRatingSliderValueIndicator),
    };

    private Control CreateAudioVisualization(bool horizontal, bool bars)
    {
        Control control;

        if (Globals.Settings.DisplayAudioVisualization)
        {
            if (horizontal)
            {
                levelBarLeft = new LevelBar
                {
                    MinimumSize = new Size(15, 10),
                    Orientation = Orientation.Horizontal,
                    BackgroundColor = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftBackground),
                    ColorStart = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftStart),
                    ColorEnd = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftEnd),
                };

                levelBarRight = new LevelBar
                {
                    MinimumSize = new Size(15, 10),
                    Orientation = Orientation.Horizontal,
                    BackgroundColor = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightBackground),
                    ColorStart = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightStart),
                    ColorEnd = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightEnd),
                };

                control = new TableLayout
                {
                    Rows =
                    {
                        new TableRow(new TableCell(spectrumAnalyzer, true)) { ScaleHeight = true, },
                        bars ? new TableRow(new TableCell(levelBarLeft, true)) { ScaleHeight = false, } : new Panel(),
                        bars ? new TableRow(new TableCell(levelBarRight, true)) { ScaleHeight = false, } : new Panel(),
                    },
                    Height = 100,
                    Padding = new Padding(Globals.DefaultPadding, 0),
                };
            }
            else
            {
                levelBarLeft = new LevelBar
                {
                    MinimumSize = new Size(20, 10),
                    Orientation = Orientation.Vertical,
                    BackgroundColor = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftBackground),
                    ColorStart = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftStart),
                    ColorEnd = Color.Parse(Globals.ColorConfiguration.ColorLevelBarLeftEnd),
                };

                levelBarRight = new LevelBar
                {
                    MinimumSize = new Size(20, 10),
                    Orientation = Orientation.Vertical,
                    BackgroundColor = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightBackground),
                    ColorStart = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightStart),
                    ColorEnd = Color.Parse(Globals.ColorConfiguration.ColorLevelBarRightEnd),
                };

                control = new TableLayout
                {
                    Rows =
                    {
                        new TableRow
                        {
                            Cells =
                            {
                                bars ? levelBarLeft : new Panel(),
                                new TableCell(spectrumAnalyzer, true),
                                bars ? levelBarRight : new Panel(),
                            },
                            ScaleHeight = true,
                        },
                    },
                    Height = 100,
                    Padding = new Padding(Globals.DefaultPadding, 0),
                };
            }
        }
        else
        {
            control = new Panel();
        }

        return control;
    }
}