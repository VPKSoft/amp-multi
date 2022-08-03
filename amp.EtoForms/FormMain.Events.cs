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
using System.ComponentModel;
using amp.Database.DataModel;
using amp.Database.QueryHelpers;
using amp.EtoForms.Classes;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Forms;
using amp.EtoForms.Forms.EventArguments;
using amp.EtoForms.Utilities;
using amp.Playback.Converters;
using amp.Playback.Enumerations;
using amp.Playback.EventArguments;
using amp.Shared.Classes;
using amp.Shared.Extensions;
using amp.Shared.Localization;
using Eto.Forms;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.Helpers;
using EtoForms.Controls.Custom.UserIdle;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;
using AudioTrack = amp.EtoForms.Models.AudioTrack;

namespace amp.EtoForms;

partial class FormMain
{
    private async void FormMain_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Equals(sender, tbSearch))
        {
            if (e.Key is Keys.Up or Keys.Down or Keys.PageDown or Keys.PageUp or Keys.Equal or Keys.F2)
            {
                if (gvAudioTracks.SelectedItem == null && gvAudioTracks.DataStore.Any())
                {
                    gvAudioTracks.SelectedRow = 0;
                }
                gvAudioTracks.Focus();
                e.Handled = true;
            }
            return;
        }

        if (e.Key == Keys.Escape)
        {
            tbSearch.Focus();
            tbSearch.Text = string.Empty;
            e.Handled = true;
            return;
        }

        if (e.Key is Keys.Add or Keys.Multiply)
        {
            var selectedTracks = tracks.Where(f => SelectedAlbumTrackIds.Contains(f.Id)).Select(f => f.Id);
            await playbackOrder.ToggleQueue(tracks, e.Key == Keys.Multiply, (e.Modifiers == Application.Instance.CommonModifier),
                selectedTracks.ToArray());

            e.Handled = true;
            gvAudioTracks.Focus();
            return;
        }

        if (e.Key is Keys.PageUp or Keys.PageDown && e.Modifiers.HasFlag(Application.Instance.CommonModifier))
        {
            var shift = e.Modifiers.HasFlag(Keys.Shift);
            await playbackOrder.MoveToQueueTopOrBottom(tracks, shift, e.Key == Keys.PageUp,
                SelectedAlbumTrackIds.ToArray());

            e.Handled = true;
            return;
        }

        if (e.Key == Keys.Enter)
        {
            if (gvAudioTracks.SelectedItem != null)
            {
                var albumTrack = (Models.AlbumTrack)gvAudioTracks.SelectedItem;
                await playbackManager.PlayAudioTrack(albumTrack, true);
                e.Handled = true;
                return;
            }
        }

        if (e.Key == Keys.Delete)
        {
            await gvAudioTracks.DeleteSongs(context, tracks, FilterTracks);

            e.Handled = true;
            return;
        }

        if (e.Key == Keys.F2)
        {
            await gvAudioTracks.RenameTrack(context, this);

            e.Handled = true;
            return;
        }

        if (e.Modifiers == Keys.None)
        {
            if (e.IsChar)
            {
                tbSearch.Text = e.KeyChar.ToString();
                tbSearch.CaretIndex++;
                tbSearch.Focus();
                e.Handled = true;
            }
        }
    }

    private async void NextAudioTrackCommand_Executed(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextTrack(true);
    }

    private void PlaybackPosition_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        playbackManager.PlaybackPositionPercentage = e.Value;
    }

    private async void GvAudioTracksMouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var track = tracks.First(f => f.Id == SelectedAlbumTrackId);
        if (track.AudioTrack?.PlayedByUser != null)
        {
            track.AudioTrack.PlayedByUser++;
            track.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;
            track.AudioTrack.UpdateDataModel(context.AudioTracks.FirstOrDefault(f => f.Id == track.AudioTrackId));
            await context.SaveChangesAsync();
        }
        await playbackManager.PlayAudioTrack(track, true);
    }

    private void FormMain_LocationChanged(object? sender, EventArgs e)
    {
        formAlbumImage.Reposition(this);
    }

    private void TbSearch_TextChanged(object? sender, EventArgs e)
    {
        FilterTracks();
    }

    private void BtnShowQueue_CheckedChange(object? sender, CheckedChangeEventArguments e)
    {
        FilterTracks();
    }

    private void FormMain_Closing(object? sender, CancelEventArgs e)
    {
        tmMessageQueueTimer.Stop();
        tmMessageQueueTimer.Dispose();
        Globals.SaveSettings();
        playbackManager.Dispose();
        formAlbumImage.DisposeClose();
        formAlbumImage.Dispose();
        idleChecker.Dispose();
    }

    private async Task<Models.AlbumTrack?> GetNextAudioTrackFunc()
    {
        Models.AlbumTrack? result = null;
        await Application.Instance.InvokeAsync(async () =>
        {
            var nextTrackData = await playbackOrder.NextTrack(tracks);
            result = tracks[nextTrackData.NextTrackIndex];
            if (result.AudioTrack != null)
            {
                result.AudioTrack.PlayedByRandomize ??= 0;
                result.AudioTrack.PlayedByUser ??= 0;

                result.AudioTrack.PlayedByRandomize += nextTrackData.PlayedByRandomize;
                result.AudioTrack.PlayedByUser += nextTrackData.PlayedByUser;
                result.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;

                var updateEntity = await context.AudioTracks.FirstOrDefaultAsync(f => f.Id == result.AudioTrackId);
                result.AudioTrack.UpdateDataModel(updateEntity);

                await context.SaveChangesAsync();
            }
        });

        return result;
    }

    private async void PlaybackManager_PlaybackStateChanged(object? sender, PlaybackStateChangedArgs e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            var track = tracks.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            lbTracksTitle.Text = track?.GetAudioTrackName() ?? string.Empty;
            btnPlayPause.CheckedChange -= PlayPauseToggle;
            btnPlayPause.Checked = e.PlaybackState == PlaybackState.Playing;
            btnPlayPause.CheckedChange += PlayPauseToggle;
            audioVisualizationControl.Visible = e.PlaybackState == PlaybackState.Playing;
        });
    }

    private void PlaybackManagerTrackChanged(object? sender, TrackChangedArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            var track = tracks.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            trackVolumeSlider.SuspendEventInvocation = true;
            trackRatingSlider.SuspendEventInvocation = true;
            trackVolumeSlider.Value = track?.AudioTrack?.PlaybackVolume * 100 ?? 100;
            trackRatingSlider.Value = track?.AudioTrack?.Rating ?? 500;
            trackVolumeSlider.SuspendEventInvocation = false;
            trackRatingSlider.SuspendEventInvocation = false;
            lbTracksTitle.Text = track?.GetAudioTrackName() ?? string.Empty;

            var dataSource = gvAudioTracks.DataStore.Cast<Models.AlbumTrack>().ToList();
            var displayTrack = dataSource.FindIndex(f => f.AudioTrackId == e.AudioTrackId);
            if (displayTrack != -1)
            {
                gvAudioTracks.SelectedRow = displayTrack;
                gvAudioTracks.ScrollToRow(displayTrack);
                gvAudioTracks.Focus();
            }

            if (track != null)
            {
                formAlbumImage.Show(this, track);
            }

            btnPreviousTrack.Enabled = playbackManager.CanGoPrevious;

            spectrumAnalyzer.SignalProvider?.SetChannel(e.BassHandle);
        });
    }

    private void PlaybackManagerTrackSkipped(object? sender, TrackSkippedEventArgs e)
    {
        Globals.LoggerSafeInvoke(async () =>
        {
            var albumTrack = tracks.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            if (albumTrack != null)
            {
                albumTrack.AudioTrack!.SkippedEarlyCount = albumTrack.AudioTrack.SkippedEarlyCount == null
                    ? 1
                    : albumTrack.AudioTrack.SkippedEarlyCount + 1;
                albumTrack.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;
                context.Update(albumTrack);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            }
        });
    }

    private readonly FormAlbumImage formAlbumImage = new();

    private async void PlayPauseToggle(object? sender, CheckedChangeEventArguments e)
    {
        btnPlayPause.CheckedChange -= PlayPauseToggle;
        if (e.Checked)
        {
            await playbackManager.PlayOrResume();
        }
        else
        {
            if (playbackManager.PlaybackState == PlaybackState.Playing)
            {
                playbackManager.Pause();
            }
        }

        btnPlayPause.CheckedChange += PlayPauseToggle;
    }

    private void BtnShuffleToggle_CheckedChange(object? sender, CheckedChangeEventArguments e)
    {
        playbackManager.Shuffle = e.Checked;
    }

    private void BtnRepeatToggle_CheckedChange(object? sender, CheckedChangeEventArguments e)
    {
        playbackManager.Repeat = e.Checked;
    }

    private async void PlayNextAudioTrackClick(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextTrack(true);
    }

    private async Task<Models.AlbumTrack?> GetTrackById(long trackId)
    {
        return await Application.Instance.InvokeAsync(Models.AlbumTrack? () =>
        {
            return tracks.FirstOrDefault(f => f.AudioTrackId == trackId);
        });
    }

    private void PlaybackManager_PlaybackPositionChanged(object? sender, PlaybackPositionChangedArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            playbackPosition.SuspendEventInvocation = true;
            playbackPosition.Value = e.CurrentPosition / e.PlaybackLength * 100;
            playbackPosition.SuspendEventInvocation = false;
            lbPlaybackPosition.Text =
                "-" + TimeSpan.FromSeconds(e.PlaybackLength - e.CurrentPosition).ToString(@"hh\:mm\:ss");
        });
    }

    // Eto.Forms localization.
    private void Instance_LocalizeString(object? sender, LocalizeEventArgs e)
    {
        try
        {
            e.LocalizedText = e.Text switch
            {
                null => null,
                "&File" => Shared.Localization.EtoForms.File,
                "&Help" => Shared.Localization.EtoForms.Help,
                "About" => Shared.Localization.EtoForms.About,
                "Hide amp.EtoForms" => Mac.HideAmpEtoForms,
                "Hide" => UI.Hide,
                "Hides the main amp.EtoForms window" => Mac.HidesTheMainAmpEtoFormsWindow,
                "Hide Others" => Mac.HideOthers,
                "Hides all other application windows" => Mac.HidesAllOtherApplicationWindows,
                "Show All" => Mac.ShowAll,
                "Show All Windows" => Mac.ShowAllWindows,
                "Minimize" => UI.Minimize,
                "Zoom" => UI.Zoom,
                "Close" => UI.Close,
                "Bring All To Front" => Mac.BringAllToFront,
                "Cut" => UI.Cut,
                "Copy" => UI.Copy,
                "Paste" => UI.Paste,
                "Paste and Match Style" => Mac.PasteAndMatchStyle,
                "Delete" => UI.Delete,
                "Select All" => UI.SelectAll,
                "Undo" => UI.Undo,
                "Redo" => UI.Redo,
                "Enter Full Screen" => Mac.EnterFullScreen,
                "Page Setup..." => UI.PageSetup,
                "Print..." => UI.Print,
                "&Edit" => UI.Edit,
                "&Window" => UI.Window,
                _ => throw new ArgumentOutOfRangeException(nameof(e.LocalizedText)),
            };
        }
        catch (ArgumentOutOfRangeException)
        {
            Globals.Logger?.Information("Localization needed: '{text}'.", e.Text);
        }
    }

    private void AddDirectoryToDatabase_Executed(object? sender, EventArgs e)
    {
        AddDirectory(sender?.Equals(addDirectoryToAlbum) == true);
    }

    private void AddFilesToDatabase_Executed(object? sender, EventArgs e)
    {
        AddAudioFiles(sender?.Equals(addFilesToAlbum) == true);
    }

    private void IdleChecker_UserIdleChanged(object? sender, UserIdleEventArgs e)
    {
        FilterTracks();
    }

    private async void FormMain_Shown(object? sender, EventArgs e)
    {
        await UpdateAlbumDataSource();
        LoadLayout();
        CreateAlbumSelector();
        shownCalled = true;
        RefreshCurrentAlbum();
        loadingPosition = true;
        positionSaveLoad.Load();
        loadingPosition = false;
    }

    private void PlaybackManager_PlaybackError(object? sender, PlaybackErrorEventArgs e)
    {
        DisplayMessageQueue.Enqueue(new KeyValuePair<string, DateTime>(string.Format(UI.PlaybackError0, e.Error.ErrorString()), DateTime.Now));
        Globals.Logger?.Error("ManagedBass error occurred '{error}'.", e.Error.ErrorString());
    }

    private void PlaybackManager_PlaybackErrorFileNotFound(object? sender, PlaybackErrorFileNotFoundEventArgs e)
    {
        DisplayMessageQueue.Enqueue(new KeyValuePair<string, DateTime>(string.Format(Messages.File0WasNotFound, e.FileName), DateTime.Now));
        Globals.Logger?.Error(new FileNotFoundException(string.Format(Messages.File0WasNotFound, e.FileName)), "");
    }

    private async void PlayPreviousClick(object? sender, EventArgs e)
    {
        await playbackManager.PreviousTrack();
    }

    private void ManageSavedQueues_Executed(object? sender, EventArgs e)
    {
        new FormSavedQueues(context, LoadOrAppendQueue).ShowModal(this);
    }

    private async void SaveQueueCommand_Executed(object? sender, EventArgs e)
    {
        string? queueName;
        if (UtilityOS.IsMacOS)
        {
            queueName = new DialogQueryValue<string>(UI.SaveCurrentQueue, UI.QueueName, false, Globals.DefaultSpacing,
                // ReSharper disable once MethodHasAsyncOverload, Eto.Mac doesn't fire shown event.
                Globals.DefaultPadding).ShowModal(this);
        }
        else
        {
            queueName = await new DialogQueryValue<string>(UI.SaveCurrentQueue, UI.QueueName, false, Globals.DefaultSpacing,
                Globals.DefaultPadding).ShowModalAsync(this);
        }

        if (queueName != null)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await Globals.LoggerSafeInvokeAsync(async () =>
            {
                var queueSnapshot = new QueueSnapshot
                {
                    AlbumId = CurrentAlbumId,
                    CreatedAtUtc = DateTime.UtcNow,
                    SnapshotName = queueName,
                    SnapshotDate = DateTime.Now,
                };

                context.ChangeTracker.Clear();

                context.QueueSnapshots.Add(queueSnapshot);
                await context.SaveChangesAsync();

                List<QueueTrack> queueTracks;

                if (AlternateQueuedItemsInSelection)
                {
                    queueTracks = tracks.Where(f => f.QueueIndexAlternate > 0).Select(f => new QueueTrack
                    {
                        QueueSnapshotId = queueSnapshot.Id,
                        AudioTrackId = f.AudioTrackId,
                        CreatedAtUtc = DateTime.UtcNow,
                        QueueIndex = f.QueueIndexAlternate,
                    }).ToList();

                    foreach (var albumTrack in tracks.Where(f => f.QueueIndexAlternate > 0))
                    {
                        albumTrack.QueueIndexAlternate = 0;
                    }
                    gvAudioTracks.ReloadKeepSelection();
                }
                else
                {
                    queueTracks = tracks.Where(f => f.QueueIndex > 0).Select(f => new QueueTrack
                    {
                        QueueSnapshotId = queueSnapshot.Id,
                        AudioTrackId = f.AudioTrackId,
                        CreatedAtUtc = DateTime.UtcNow,
                        QueueIndex = f.QueueIndex,
                    }).ToList();
                }

                context.QueueTracks.AddRange(queueTracks);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                context.ChangeTracker.Clear();
            }, async (_) => { await transaction.RollbackAsync(); });
        }
    }

    private async void ManageAlbumsCommand_Executed(object? sender, EventArgs e)
    {
        bool modalResult;

        if (UtilityOS.IsMacOS)
        {
            // ReSharper disable once MethodHasAsyncOverload, Shown-event won't fire on macOS.
            modalResult = new FormAlbums(context).ShowModal(this);
        }
        else
        {
            modalResult = await new FormAlbums(context).ShowModalAsync(this);
        }

        if (modalResult)
        {
            await UpdateAlbumDataSource();
        }
    }

    private void GvAudioTracks_ColumnOrderChanged(object? sender, GridColumnEventArgs e)
    {
        Globals.PositionAndLayoutSettings.TrackGridColumnDisplayIndices =
            gvAudioTracks.Columns.Select(f => f.DisplayIndex).ToArray();
    }

    private void GvAudioTracksSizeChanged(object? sender, EventArgs e)
    {
        gvAudioTracks.Columns[0].Width = gvAudioTracks.Width - 80;
        gvAudioTracks.Columns[1].Width = 30;
        gvAudioTracks.Columns[2].Width = 30;
    }

    private void SettingsCommand_Executed(object? sender, EventArgs e)
    {
        using var settingsForm = new FormSettings();
        playbackOrder.StackQueueRandomPercentage = Globals.Settings.StackQueueRandomPercentage;
        settingsForm.ShowModal(this);
    }

    private async void ScrambleQueueCommand_Executed(object? sender, EventArgs e)
    {
        var selectedTracks = tracks.Where(f => SelectedAlbumTrackIds.Contains(f.Id)).Select(f => f.Id);
        if (QueuedItemsInSelection)
        {
            await playbackOrder.Scramble(tracks, false, selectedTracks.ToArray());
        }
        else if (AlternateQueuedItemsInSelection)
        {
            await playbackOrder.Scramble(tracks, true, selectedTracks.ToArray());
        }
        else
        {
            await playbackOrder.Scramble(tracks, false);
        }
    }

    private async void ClearQueueCommand_Executed(object? sender, EventArgs e)
    {
        await playbackOrder.ClearQueue(tracks, false);
    }

    private void BtnStackQueueToggle_CheckedChange(object? sender, CheckedChangeEventArguments e)
    {
        playbackOrder.StackQueueMode = e.Checked;
    }

    private void TrackInfoCommand_Executed(object? sender, EventArgs e)
    {
        var track = (Models.AlbumTrack?)gvAudioTracks.SelectedItem;
        if (track != null)
        {
            using var dialog = new FormDialogTrackInfo(track.AudioTrack!, AudioTrackChanged);
            dialog.ShowModal(this);
        }
    }

    private async void AudioTrackChanged(object? sender, AudioTrackChangedEventArgs e)
    {
        var track = await context.AudioTracks.FirstOrDefaultAsync(f => f.Id == e.AudioTrack.Id);
        if (track != null)
        {
            e.AudioTrack.UpdateDataModel(track);
            var count = await context.SaveChangesAsync();
            e.SaveSuccess = count > 0;
            if (e.SaveSuccess)
            {
                var index = tracks.FindIndex(f => f.AudioTrackId == e.AudioTrack.Id);
                if (index != -1)
                {
                    tracks[index].AudioTrack = Globals.AutoMapper.Map<AudioTrack>(track);
                }
            }
        }
    }

    private async void QueryDivider_QueryCompleted(object? sender, QueryCompletedEventArgs<Models.AlbumTrack> e)
    {
        tracks = new ObservableCollection<Models.AlbumTrack>(e.ResultList);

        tracks = new ObservableCollection<Models.AlbumTrack>(tracks.OrderBy(f => f.DisplayName));

        await Application.Instance.InvokeAsync(() =>
        {
            filteredTracks = tracks;

            if (!string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                filteredTracks =
                    new ObservableCollection<Models.AlbumTrack>(tracks.Where(f => f.AudioTrack!.Match(tbSearch.Text))
                        .ToList());
            }

            gvAudioTracks.DataStore = filteredTracks;
            lbLoadingText.Visible = false;
            progressLoading.Visible = false;
            if (queryDivider != null)
            {
                queryDivider.ProgressChanged -= QueryDivider_ProgressChanged;
                queryDivider.QueryCompleted -= QueryDivider_QueryCompleted;
            }

            UpdateCounters();
            Enabled = true;
        });
    }

    private async void QueryDivider_ProgressChanged(object? sender, QueryProgressChangedEventArgs e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            lbLoadingText.Text = string.Format(Messages.LoadingPercentage, e.CurrentPercentage);
            progressLoading.Value = e.CurrentCount;
        });
    }

    private void TrackVolumeSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        playbackManager.PlaybackVolume = e.Value / 100.0;
    }

    private void TotalVolumeSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var volume = e.Value / 100.0;
        playbackManager.MasterVolume = volume;
        Globals.Settings.MasterVolume = volume;
        Globals.SaveSettings();
    }

    private async void PlaybackManager_TrackVolumeChanged(object? sender, TrackVolumeChangedEventArgs e)
    {
        var track = tracks.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
        if (track != null)
        {
            await AlbumTrackMethods.UpdateTrackVolume(track, context, e.TrackVolume);
        }
    }

    private void TrackRatingSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        playbackManager.Rating = (int)e.Value;
    }

    private async void PlaybackManager_TrackRatingChanged(object? sender, TrackRatingChangedEventArgs e)
    {
        var track = tracks.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
        if (track != null)
        {
            await AlbumTrackMethods.UpdateTrackRating(track, context, e.TrackRating);
        }
    }

    private void Result_ExpandedChanged(object? sender, EventArgs e)
    {
        Globals.Settings.AudioAndRatingControlsExpanded = trackAdjustControls.Expanded;
        Globals.SaveSettings();
    }

    private void FormMain_SizeLocationChanged(object? sender, EventArgs e)
    {
        if (loadingPosition)
        {
            return;
        }
        positionLastChanged = DateTime.Now;
        timer.Start();
    }

    private void Timer_Elapsed(object? sender, EventArgs e)
    {
        if ((DateTime.Now - positionLastChanged).TotalSeconds > 10)
        {
            if (shownCalled && !loadingPosition)
            {
                timer.Stop();
                positionSaveLoad.Save();
            }
        }
    }
}