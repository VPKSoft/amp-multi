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

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using amp.Database.QueryHelpers;
using amp.EtoForms.Dialogs;
using amp.EtoForms.DtoClasses;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Forms.Enumerations;
using amp.EtoForms.Properties;
using amp.EtoForms.Utilities;
using amp.Playback.Classes;
using amp.Shared.Classes;
using amp.Shared.Constants;
using amp.Shared.Localization;
using amp.Shared.UpdateCheck;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Helpers;
using EtoForms.Controls.Custom.Utilities;
using EtoForms.SpectrumVisualizer;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms;
partial class FormMain
{
    private void AddAudioFiles(bool toAlbum)
    {
        using var dialog = new OpenFileDialog { MultiSelect = true, };
        dialog.Filters.Add(new FileFilter(UI.MusicFiles, MusicConstants.SupportedExtensionArray));
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            DialogAddFilesProgress.ShowModal(this, context, toAlbum ? CurrentAlbumId : 0, dialog.Filenames.ToArray());
        }

        RefreshCurrentAlbum();
    }

    private void AddDirectory(bool toAlbum)
    {
        using var dialog = new SelectFolderDialog { Title = UI.SelectMusicFolder, };
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            DialogAddFilesProgress.ShowModal(this, context, dialog.Directory, toAlbum ? CurrentAlbumId : 0);
        }

        RefreshCurrentAlbum();
    }

    private async Task LoadOrAppendQueue(Dictionary<long, int> queueData, long albumId, QueueAppendInsertMode mode)
    {
        var updateData =
            new Dictionary<long, int>();

        CurrentAlbumId = albumId;

        RefreshCurrentAlbum();
        Thread.Sleep(100);

        //// This async selection update needs to be waited to finish.
        queryDivider?.WaitForCurrentQuery();

        var queueIndexAdd = mode is QueueAppendInsertMode.Append
            ? tracks.DefaultIfEmpty().Max(f => f?.QueueIndex) + 0 ?? 0
            : 0;

        if (mode == QueueAppendInsertMode.Load)
        {
            await playbackOrder.ClearQueue(tracks, false);
        }

        if (mode == QueueAppendInsertMode.Insert)
        {
            await playbackOrder.ShiftQueueDown(tracks, queueData.Count, false);
        }

        foreach (var data in queueData)
        {
            var track = tracks.FirstOrDefault(f => f.AudioTrackId == data.Key);

            // Skip if already queued.
            if (track is { QueueIndex: > 0, })
            {
                if (mode is QueueAppendInsertMode.Append or QueueAppendInsertMode.Insert)
                {
                    continue;
                }
            }


            var id = track?.Id;

            if (id != null)
            {
                updateData.Add(id.Value, data.Value + queueIndexAdd);
            }
        }

        await UpdateQueueFunc(updateData, false);
    }


    private async Task UpdateQueueFunc(Dictionary<long, int> updateQueueData, bool alternate)
    {
        var keys = updateQueueData.Select(f => f.Key).ToList();

        var modifyTracks = await context.AlbumTracks.Where(f => keys.Contains(f.Id)).ToListAsync();
        var modifyTracksView = tracks.Where(f => keys.Contains(f.Id)).ToList();

        foreach (var albumTrack in modifyTracksView)
        {
            var newIndex = updateQueueData.First(f => f.Key == albumTrack.Id).Value;
            if (alternate)
            {
                albumTrack.QueueIndexAlternate = newIndex;

            }
            else
            {
                albumTrack.QueueIndex = newIndex;
            }

            albumTrack.ModifiedAtUtc = DateTime.UtcNow;

            albumTrack.UpdateDataModel(modifyTracks.FirstOrDefault(f => f.Id == albumTrack.Id));
        }

        var count = await context.SaveChangesAsync();

        gvAudioTracks.ReloadKeepSelection();

        if (btnShowQueue.Checked && count > 0)
        {
            FilterTracks(false);
        }

        UpdateCounters();
    }

    /// <summary>
    /// Refreshes the current album.
    /// </summary>
    private void RefreshCurrentAlbum()
    {
        if (!shownCalled)
        {
            return;
        }

        lbLoadingText.Visible = true;
        progressLoading.Visible = true;
        Enabled = false;


        var query = context.AlbumTracks.Where(f => f.AlbumId == CurrentAlbumId).Include(f => f.AudioTrack)
            .Select(f => Globals.AutoMapper.Map<AlbumTrack>(f)).AsNoTracking();

        if (queryDivider is { QueryRunning: true, })
        {
            return;
        }

        queryDivider = new QueryDivider<AlbumTrack>(query, 100, ExceptionProvider.Instance, false);
        queryDivider.ProgressChanged += QueryDivider_ProgressChanged;
        queryDivider.QueryCompleted += QueryDivider_QueryCompleted;
        queryDivider.RunQueryTasksLinear();
        queryDivider.WaitForStart();
    }

    private void AssignSettings()
    {
        formAlbumImage.AlwaysVisible = !Globals.Settings.AutoHideEmptyAlbumImage;
        playbackManager.MasterVolume = Globals.Settings.MasterVolume;
        trackAdjustControls.Expanded = Globals.Settings.AudioAndRatingControlsExpanded;

        Globals.LoggerSafeInvoke(() =>
        {
            var newSpectrumColors = new List<GradientColors>();
            var i = 0;
            while (i < Globals.ColorConfiguration.ColorsSpectrumVisualizerChannels.Length)
            {
                newSpectrumColors.Add(new GradientColors(
                    Color.Parse(Globals.ColorConfiguration.ColorsSpectrumVisualizerChannels[i]),
                    Color.Parse(Globals.ColorConfiguration.ColorsSpectrumVisualizerChannels[i + 1])));
                i += 2;
            }

            spectrumAnalyzer.VisualizeColors = newSpectrumColors;
        });
    }

    private void FillAboutDialogData()
    {
        aboutDialog.License = @"
MIT License

Copyright(c) 2022 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
";

        aboutDialog.Logo = SvgToImage.ImageFromSvg(Resources.music_note_svgrepo_com_modified, new Size(64, 64));
        aboutDialog.Title = $"amp# {UI._} {UI.About}";
        aboutDialog.ProgramName = "amp#";
        aboutDialog.Website = new Uri("https://github.com/VPKSoft/amp");
        aboutDialog.WebsiteLabel = "GitHub/amp";
    }

    private void SetupInitialSettings()
    {
        var formula = string.IsNullOrWhiteSpace(Globals.Settings.TrackNameFormula)
            ? TrackDisplayNameGenerate.FormulaDefault
            : Globals.Settings.TrackNameFormula;

        var formulaRenamed = string.IsNullOrWhiteSpace(Globals.Settings.TrackNameFormulaRenamed)
            ? TrackDisplayNameGenerate.FormulaTrackRenamedDefault
            : Globals.Settings.TrackNameFormulaRenamed;

        TrackDisplayNameGenerate.Formula = formula;
        TrackDisplayNameGenerate.FormulaTrackRenamed = formulaRenamed;
        TrackDisplayNameGenerate.MinimumTrackLength = Globals.Settings.TrackNamingMinimumTitleLength;
        TrackDisplayNameGenerate.TrackNamingFallbackToFileNameWhenNoLetters = Globals.Settings.TrackNamingFallbackToFileNameWhenNoLetters;
    }

    private void FilterTracks(bool fromUserIdleEvent)
    {
        Application.Instance.Invoke(() =>
        {
            if (!fromUserIdleEvent)
            {
                userIdleSelectedRows.Clear();
                userIdleSelectedRow = -1;
            }

            var text = tbSearch.Text;
            var queueOnly = btnShowQueue.Checked;
            var userIdle = idleChecker.IsUserIdle;

            // The user went idle, save the current selection.
            if (userIdle && fromUserIdleEvent)
            {
                userIdleSelectedRows = gvAudioTracks.SelectedRows.ToList();
                userIdleSelectedRow = gvAudioTracks.SelectedRow;
            }

            filteredTracks = tracks;

            // These filters only apply when the user is active.
            if (!userIdle)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    filteredTracks = new ObservableCollection<AlbumTrack>(tracks.Where(f => f.AudioTrack!.Match(text)));
                }
            }

            if (queueOnly)
            {
                filteredTracks = new ObservableCollection<AlbumTrack>(filteredTracks.Where(f => f.QueueIndex > 0).OrderBy(f => f.QueueIndex));
            }

            gvAudioTracks.DataStore = filteredTracks;

            if (!userIdle && userIdleSelectedRow != -1 && fromUserIdleEvent)
            {
                gvAudioTracks.SelectedRows = userIdleSelectedRows;
                gvAudioTracks.SelectedRow = userIdleSelectedRow;
                gvAudioTracks.ScrollToRow(userIdleSelectedRow);
            }

            UpdateCounters();

            if (userIdle && !gvAudioTracks.HasFocus)
            {
                gvAudioTracks.Focus();
            }
        });
    }

    private void LoadLayout()
    {
        var min = Math.Min(gvAudioTracks.Columns.Count,
            Globals.PositionAndLayoutSettings.TrackGridColumnDisplayIndices.Length);

        for (var i = 0; i < min; i++)
        {
            gvAudioTracks.Columns[i].DisplayIndex = Globals.PositionAndLayoutSettings.TrackGridColumnDisplayIndices[i];
        }
    }

    private void AssignEventListeners()
    {
        btnShuffleToggle.CheckedChange += BtnShuffleToggle_CheckedChange;
        btnRepeatToggle.CheckedChange += BtnRepeatToggle_CheckedChange;
        btnPlayPause.CheckedChange += PlayPauseToggle;
        nextAudioTrackCommand.Executed += NextAudioTrackCommand_Executed;
        tbSearch.TextChanged += TbSearch_TextChanged;
        gvAudioTracks.MouseDoubleClick += GvAudioTracksMouseDoubleClick;
        Closing += FormMain_Closing;
        Closed += OnClosed;
        AttachDetachKeyDownHandler(true);
        playbackManager.PlaybackStateChanged += PlaybackManager_PlaybackStateChanged;
        playbackManager.TrackChanged += PlaybackManagerTrackChanged;
        playbackManager.PlaybackPositionChanged += PlaybackManager_PlaybackPositionChanged;
        playbackManager.TrackSkipped += PlaybackManagerTrackSkipped;
        playbackManager.PlaybackErrorFileNotFound += PlaybackManager_PlaybackErrorFileNotFound;
        playbackManager.PlaybackError += PlaybackManager_PlaybackError;
        playbackManager.TrackVolumeChanged += PlaybackManager_TrackVolumeChanged;
        playbackManager.TrackRatingChanged += PlaybackManager_TrackRatingChanged;
        LocationChanged += FormMain_LocationChanged;
        idleChecker.UserIdle += IdleChecker_UserIdleChanged;
        idleChecker.UserActivated += IdleChecker_UserIdleChanged;
        settingsCommand.Executed += SettingsCommand_Executed;
        gvAudioTracks.SizeChanged += GvAudioTracksSizeChanged;
        Shown += FormMain_Shown;
        btnShowQueue.CheckedChange += BtnShowQueue_CheckedChange;
        gvAudioTracks.ColumnOrderChanged += GvAudioTracks_ColumnOrderChanged;
        btnStackQueueToggle.CheckedChange += BtnStackQueueToggle_CheckedChange;
        tmMessageQueueTimer.Elapsed += TmMessageQueueTimer_Elapsed;
        trackVolumeSlider.ValueChanged += TrackVolumeSlider_ValueChanged;
        totalVolumeSlider.ValueChanged += TotalVolumeSlider_ValueChanged;
        trackRatingSlider.ValueChanged += TrackRatingSlider_ValueChanged;
        SizeChanged += FormMain_SizeLocationChanged;
        LocationChanged += FormMain_SizeLocationChanged;
        WindowStateChanged += FormMain_SizeLocationChanged;
        timerSavePositionCheck.Elapsed += TimerSavePositionCheckElapsed;
        timerQuietHourChecker.Elapsed += TimerQuietHourChecker_Elapsed;
        timerCheckUpdates.Elapsed += TimerCheckUpdates_Elapsed;
        timerSavePositionCheck.Interval = 2;
        if (Globals.Settings.QuietHours)
        {
            timerQuietHourChecker.Start();
        }
        tmMessageQueueTimer.Start();

        if (Globals.Settings.AutoCheckUpdates)
        {
            timerCheckUpdates.Start();
        }
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (UtilityOS.IsMacOS)
        {
            Application.Instance.Quit();
        }
    }

    private async void TmMessageQueueTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            tmMessageQueueTimer.Enabled = false;
            if (previousMessageTime == null || (DateTime.Now - previousMessageTime.Value).TotalSeconds > 30)
            {
                if (DisplayMessageQueue.TryDequeue(out var messagePair))
                {
                    await Application.Instance.InvokeAsync(() =>
                    {
                        lbStatusMessage.Text = messagePair.Key;
                        previousMessageTime = DateTime.Now;
                    });
                }
                else
                {
                    previousMessageTime = null;
                }
            }

            if (previousMessageTime == null)
            {
                await Application.Instance.InvokeAsync(() =>
                {
                    if (lbStatusMessage.Text != string.Empty)
                    {
                        lbStatusMessage.Text = string.Empty;
                    }
                });
            }

            tmMessageQueueTimer.Enabled = true;
        });
    }

    private void UpdateCounters()
    {
        lbTrackCountValue.Text = string.Format(UI.NumberOfNumber, filteredTracks.Count, tracks.Count);
        lbQueueCountValue.Text = $"{tracks.Count(f => f.QueueIndex > 0)}";
    }

    private void AttachDetachKeyDownHandler(bool attach)
    {
        AttachDetachKeyDownListeners(this, FormMain_KeyDown, attach);
        foreach (var control in Children)
        {
            AttachDetachKeyDownListeners(control, FormMain_KeyDown, attach);
        }
    }

    private static void AttachDetachKeyDownListeners(Control control, EventHandler<KeyEventArgs> eventHandler, bool attach)
    {
        if (attach)
        {
            control.KeyDown += eventHandler;
        }
        else
        {
            control.KeyDown -= eventHandler;
        }
    }

    [MemberNotNull(nameof(quietHourHandler))]
    private void InitAdditionalFields()
    {
        quietHourHandler = new QuietHourHandler<AudioTrack, AlbumTrack, Album>(Globals.Settings);
    }

    private void SetTitle()
    {
        var album = albums.FirstOrDefault(f => f.Id == CurrentAlbumId);

        if (quietHoursSet)
        {
            var times = quietHourHandler.QuietHourTimes;
            Title = $"amp# {UI._} [{album?.AlbumName}] {string.Format(UI.QuietTime01, times.start.ToShortTimeString(), times.end.ToShortTimeString())}";
        }
        else
        {
            Title = $"amp# {UI._} [{album?.AlbumName}]";
        }
    }

    private async Task UpdateCheck(bool autoCheck)
    {
        UpdateChecker.SkipVersion = string.Empty;
        if (autoCheck)
        {
            UpdateChecker.SkipVersion = Globals.Settings.ForgerVersionUpdate;
        }

        var result = await DialogCheckNewVersion.CheckNewVersion(this, Assembly.GetEntryAssembly()!.GetName().Version!,
            autoCheck, string.IsNullOrWhiteSpace(Resources.VersionTag) ? null : Resources.VersionTag);

        if (!result && !autoCheck)
        {
            MessageBox.Show(this, Messages.YouAreAlreadyUsingTheLatestVersionOfTheSoftware, Messages.Information,
                MessageBoxButtons.OK);
        }

        if (autoCheck)
        {
            Globals.SaveSettings();
        }
    }
}