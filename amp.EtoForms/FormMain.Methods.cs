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
using amp.EtoForms.Dialogs;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Forms;
using amp.EtoForms.Layout;
using amp.EtoForms.Models;
using amp.EtoForms.Properties;
using amp.EtoForms.Utilities;
using amp.Shared.Constants;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Helpers;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms;
partial class FormMain
{
    private async Task AddAudioFiles(bool toAlbum)
    {
        using var dialog = new OpenFileDialog { MultiSelect = true, };
        dialog.Filters.Add(new FileFilter(UI.MusicFiles, MusicConstants.SupportedExtensionArray));
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            DialogAddFilesProgress.ShowModal(this, context, toAlbum ? CurrentAlbumId : 0, dialog.Filenames.ToArray());
        }

        await RefreshCurrentAlbum();
    }

    private async Task AddDirectory(bool toAlbum)
    {
        using var dialog = new SelectFolderDialog { Title = UI.SelectMusicFolder, };
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            DialogAddFilesProgress.ShowModal(this, context, dialog.Directory, toAlbum ? CurrentAlbumId : 0);
        }

        await RefreshCurrentAlbum();
    }

    private async Task LoadOrAppendQueue(Dictionary<long, int> queueData, long albumId, bool append)
    {
        if (CurrentAlbumId != albumId)
        {
            await ReusableControls.UpdateAlbumDataSource(cmbAlbumSelect, context, albumId);
            CurrentAlbumId = albumId;
        }

        var queueIndexAdd = append ? tracks.DefaultIfEmpty().Max(f => f?.QueueIndex) + 0 ?? 0 : 0;

        if (!append)
        {
            await playbackOrder.ClearQueue(tracks, false);
        }

        var updateData =
            new Dictionary<long, int>();

        foreach (var data in queueData)
        {
            var track = tracks.FirstOrDefault(f => f.AudioTrackId == data.Key);

            // Skip if already queued.
            if (track is { QueueIndex: > 0, })
            {
                continue;
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
            FilterTracks();
        }

        lbQueueCountValue.Text = $"{tracks.Count(f => f.QueueIndex > 0)}";
    }

    /// <summary>
    /// Refreshes the current album.
    /// </summary>
    private async Task RefreshCurrentAlbum()
    {
        if (!shownCalled)
        {
            return;
        }

        var loadedTracks = await FormLoadProgress<AlbumTrack>.RunWithProgress(this, context.AlbumTracks.Where(f => f.AlbumId == CurrentAlbumId).Include(f => f.AudioTrack)
            .Select(f => Globals.AutoMapper.Map<AlbumTrack>(f)).AsNoTracking(), 100);

        tracks = new ObservableCollection<AlbumTrack>(loadedTracks);

        tracks = new ObservableCollection<AlbumTrack>(tracks.OrderBy(f => f.DisplayName));

        lbQueueCountValue.Text = $"{tracks.Count(f => f.QueueIndex > 0)}";

        filteredTracks = tracks;

        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            filteredTracks =
                new ObservableCollection<AlbumTrack>(tracks.Where(f => f.AudioTrack!.Match(tbSearch.Text))
                    .ToList());
        }

        gvAudioTracks.DataStore = filteredTracks;
    }

    private void AssignSettings()
    {
        formAlbumImage.AlwaysVisible = !Globals.Settings.AutoHideEmptyAlbumImage;
    }

    private async Task UpdateAlbumDataSource()
    {
        await ReusableControls.UpdateAlbumDataSource(cmbAlbumSelect, context, CurrentAlbumId);
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

    private void FilterTracks()
    {
        Application.Instance.Invoke(() =>
        {
            var text = tbSearch.Text;
            var queueOnly = btnShowQueue.Checked;
            var userIdle = idleChecker.IsUserIdle;


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
        btnPlayPause.CheckedChange += PlayPauseToggle;
        nextAudioTrackCommand.Executed += NextAudioTrackCommand_Executed;
        tbSearch.TextChanged += TbSearch_TextChanged;
        gvAudioTracks.MouseDoubleClick += GvAudioTracksMouseDoubleClick;
        Closing += FormMain_Closing;
        KeyDown += FormMain_KeyDown;
        gvAudioTracks.KeyDown += FormMain_KeyDown;
        tbSearch.KeyDown += FormMain_KeyDown;
        playbackManager.PlaybackStateChanged += PlaybackManager_PlaybackStateChanged;
        playbackManager.TrackChanged += PlaybackManagerTrackChanged;
        playbackManager.PlaybackPositionChanged += PlaybackManager_PlaybackPositionChanged;
        playbackManager.TrackSkipped += PlaybackManagerTrackSkipped;
        playbackManager.PlaybackErrorFileNotFound += PlaybackManager_PlaybackErrorFileNotFound;
        playbackManager.PlaybackError += PlaybackManager_PlaybackError;
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
        tmMessageQueueTimer.Start();
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
}