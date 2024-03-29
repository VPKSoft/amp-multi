﻿#region License
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

using amp.DataAccessLayer.DtoClasses;
using amp.Database.QueryHelpers;
using amp.EtoForms.Enumerations;
using amp.EtoForms.Forms;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Drawing;
using EtoForms.SpectrumVisualizer;

namespace amp.EtoForms;
partial class FormMain
{
    // Main layout
    private readonly Command commandPlayPause = new();
    private readonly Command nextAudioTrackCommand = new();
    private CheckedButton btnShuffleToggle;
    private CheckedButton btnRepeatToggle;
    private CheckedButton btnShowQueue;
    private GridView gvAudioTracks;
    private readonly TextBox tbSearch = new();
    private ImageOnlyButton btnClearSearch;
    private CheckedButton btnPlayPause;
    private SvgImageButton btnPreviousTrack;
    private Button btnNextTrack;
    private readonly Label lbTracksTitle = new();

    private readonly SpectrumVisualizer spectrumAnalyzer = new(true)
    {
        Width = 50,
        Height = 50,
        BackgroundColor = Color.Parse(Globals.ColorConfiguration.ColorSpectrumVisualizerBackground),
        SpectrumType = Globals.Settings.AudioVisualizationBars ? SpectrumType.Bar : SpectrumType.Line,
    };

    private LevelBar? levelBarLeft;
    private LevelBar? levelBarRight;

    private readonly StackLayout toolBar;
    private readonly Expander trackAdjustControls;
    private PositionSlider playbackPosition;
    private Label lbPlaybackPosition;
    private readonly Command clearQueueCommand = new()
    { MenuText = UI.ClearQueue, Shortcut = Application.Instance.CommonModifier | Keys.D, };
    private readonly Command quitCommand = new() { MenuText = UI.Quit, Shortcut = Application.Instance.CommonModifier | Keys.Q, };
    private readonly Command aboutCommand = new() { MenuText = UI.About, };
    private readonly Command settingsCommand = new() { MenuText = UI.Settings, };
    private readonly Command colorSettingsCommand = new() { MenuText = UI.ColorSettings, };
    private readonly Command testStuff = new() { MenuText = UI.TestStuff, };
    private readonly Command addFilesToDatabase = new() { MenuText = UI.AddFiles, };
    private readonly Command addFilesToAlbum = new() { MenuText = UI.AddFilesToAlbum, };
    private readonly Command addDirectoryToDatabase = new() { MenuText = UI.AddFolderContents, };
    private readonly Command addDirectoryToAlbum = new() { MenuText = UI.AddFolderContentsToAlbum, };
    private readonly Command manageAlbumsCommand = new() { MenuText = UI.Albums, };
    private readonly Command saveQueueCommand = new() { MenuText = UI.SaveCurrentQueue, Shortcut = Application.Instance.CommonModifier | Keys.S, };
    private readonly Command manageSavedQueues = new() { MenuText = UI.SavedQueues, Shortcut = Keys.F3, };
    private readonly Command scrambleQueueCommand = new() { MenuText = UI.ScrambleQueue, Shortcut = Keys.F7, };
    private readonly Command trackInfoCommand = new() { MenuText = UI.TrackInformation, Shortcut = Keys.F4, };
    private readonly Command updateTrackMetadata = new() { MenuText = UI.UpdateTrackMetadata, };
    private readonly Command checkUpdates = new() { MenuText = UI.CheckForNewVersion, };
    private readonly Command openHelp = new() { MenuText = UI.Help, Shortcut = Keys.F1, };
    private readonly Command iconSettingsCommand = new() { MenuText = UI.IconSettings, };

    private readonly Command stashQueueCommand = new()
    { MenuText = UI.StashCurrentQueue, Shortcut = Application.Instance.CommonModifier | Keys.T, };

    private readonly Command stashPopQueueCommand = new()
    { MenuText = UI.PopStashedQueue, Shortcut = Application.Instance.CommonModifier | Keys.P, };

    // The album select combo box.
    private readonly ComboBox cmbAlbumSelect = new();
    private List<Album> albums = new();
    private bool suspendAlbumChange;

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
    private readonly UITimer timerSavePositionCheck = new();
    private DateTime positionLastChanged;

    // Other
    private WindowState previousWindowState;
    private readonly FormAlbumImage formAlbumImage = new();
    private QueryDivider<AlbumTrack>? queryDivider;
    private long currentTrackId;

    // About
    private readonly AboutDialog aboutDialog = new();

    // A queue of notification messages to show in the main window
    private readonly Queue<KeyValuePair<string, DateTime>> displayMessageQueue = new();

    // Thread locking
    private readonly object lockObject = new();

    // Quit hours
    private readonly UITimer timerQuietHourChecker = new() { Interval = 1, };
    private bool quietHoursSet;

    // Update check
    private readonly UITimer timerCheckUpdates = new() { Interval = 15, };

    // User idle
    private List<int> userIdleSelectedRows = new();
    private int userIdleSelectedRow = -1;

    // Track list filtering.
    private bool filterAlternateQueue;

    // Grid (track list) columns
    #region GridColumns
    private readonly GridColumn columnTrackName = new()
    {
        DataCell = new TextBoxCell(nameof(AlbumTrack.DisplayName)), Expand = true,
        HeaderText = UI.Track,
        Resizable = false,
    };

    private readonly GridColumn columnQueueIndex = new()
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
    };

    private readonly GridColumn columnAlternateQueueIndex = new()
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
    };

    private ColumnSorting ratingSort;

    private CellPainterRange<AlbumTrack>? painter;

    private readonly GridColumn columnTrackRating = new()
    {
        DataCell = new DrawableCell(),
        HeaderText = UI.RatingSortNone,
        Sortable = true,
    };
    #endregion
}