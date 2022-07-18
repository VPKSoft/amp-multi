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

using System.ComponentModel;
using System.Diagnostics;
using amp.Database.DataModel;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Forms;
using amp.EtoForms.Utilities;
using amp.Playback.Converters;
using amp.Playback.Enumerations;
using amp.Playback.EventArguments;
using amp.Shared.Classes;
using amp.Shared.Localization;
using Eto.Forms;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.UserIdle;
using EtoForms.Controls.Custom.Utilities;

namespace amp.EtoForms;

partial class FormMain
{
    private async void FormMain_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Equals(sender, tbSearch))
        {
            if (e.Key is Keys.Up or Keys.Down or Keys.PageDown or Keys.PageUp or Keys.Equal)
            {
                if (gvSongs.SelectedItem == null && gvSongs.DataStore.Any())
                {
                    gvSongs.SelectedRow = 0;
                }
                gvSongs.Focus();
                e.Handled = true;
            }
            return;
        }

        if (e.Key == Keys.Add)
        {
            var selectedSongs = songs.Where(f => SelectedAlbumSongIds.Contains(f.Id)).Select(f => f.Id);
            await playbackOrder.ToggleQueue(songs, selectedSongs.ToArray());

            gvSongs.Invalidate();

            e.Handled = true;
            return;
        }

        if (e.Key == Keys.Enter)
        {
            if (gvSongs.SelectedItem != null)
            {
                var albumSong = (Models.AlbumTrack)gvSongs.SelectedItem;
                await playbackManager.PlayAudioTrack(albumSong, true);
                e.Handled = true;
                return;
            }
        }

        if (e.Modifiers == Keys.None)
        {
            if (e.IsChar)
            {
                tbSearch.Text = tbSearch.Text.Insert(tbSearch.CaretIndex, e.KeyChar.ToString());
                tbSearch.CaretIndex++;
                tbSearch.Focus();
                e.Handled = true;
            }
        }
    }

    private long SelectedAlbumSongId
    {
        get
        {
            if (gvSongs.SelectedItem != null)
            {
                var songId = ((AlbumTrack)gvSongs.SelectedItem).Id;
                return songId;
            }

            return 0;
        }
    }

    private IEnumerable<long> SelectedAlbumSongIds
    {
        get
        {
            foreach (var gvSongsSelectedItem in gvSongs.SelectedItems)
            {
                var songId = ((Models.AlbumTrack)gvSongsSelectedItem).Id;
                yield return songId;
            }
        }
    }

    private async void NextSongCommand_Executed(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextTrack(true);
    }

    private void PlaybackPosition_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        playbackManager.PlaybackPositionPercentage = e.Value;
    }

    private async void GvSongsMouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var song = songs.First(f => f.Id == SelectedAlbumSongId);
        if (song.AudioTrack?.PlayedByUser != null)
        {
            song.AudioTrack.PlayedByUser++;
            song.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;
            context.AlbumTracks.Update(Globals.AutoMapper.Map<AlbumTrack>(song));
            await context.SaveChangesAsync();
        }
        await playbackManager.PlayAudioTrack(song, true);
    }

    private void FormMain_LocationChanged(object? sender, EventArgs e)
    {
        formAlbumImage.Reposition(this);
    }

    private void TbSearch_TextChanged(object? sender, EventArgs e)
    {
        filteredSongs = songs;

        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            filteredSongs = songs.Where(f => f.AudioTrack!.Match(tbSearch.Text)).ToList();
        }

        gvSongs.DataStore = filteredSongs;
    }

    private void FormMain_Closing(object? sender, CancelEventArgs e)
    {
        playbackManager.Dispose();
        formAlbumImage.DisposeClose();
        formAlbumImage.Dispose();
        idleChecker.Dispose();
    }

    private async Task<Models.AlbumTrack?> GetNextSongFunc()
    {
        AlbumTrack? result = null;
        await Application.Instance.InvokeAsync(async () =>
        {
            var nextSongData = await playbackOrder.NextSong(songs);
            result = Globals.AutoMapper.Map<AlbumTrack>(songs[nextSongData.NextSongIndex]);
            if (result.AudioTrack != null)
            {
                result.AudioTrack.PlayedByRandomize ??= 0;
                result.AudioTrack.PlayedByUser ??= 0;

                result.AudioTrack.PlayedByRandomize += nextSongData.PlayedByRandomize;
                result.AudioTrack.PlayedByUser += nextSongData.PlayedByUser;
                result.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;

                context.Update(result);

                await context.SaveChangesAsync();
            }
        });

        return Globals.AutoMapper.Map<Models.AlbumTrack>(result);
    }

    private async void PlaybackManager_PlaybackStateChanged(object? sender, PlaybackStateChangedArgs e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            var song = songs.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            lbSongsTitle.Text = song?.GetSongName() ?? string.Empty;
            btnPlayPause.CheckedChange -= PlayPauseToggle;
            btnPlayPause.Checked = e.PlaybackState == PlaybackState.Playing;
            btnPlayPause.CheckedChange += PlayPauseToggle;
        });
    }

    private void PlaybackManagerTrackChanged(object? sender, TrackChangedArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            var song = songs.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            songVolumeSlider.SuspendEventInvocation = true;
            songVolumeSlider.Value = song?.AudioTrack?.PlaybackVolume * 100 ?? 100;
            songVolumeSlider.SuspendEventInvocation = false;
            lbSongsTitle.Text = song?.GetSongName() ?? string.Empty;

            var dataSource = gvSongs.DataStore.Cast<AlbumTrack>().ToList();
            var displaySong = dataSource.FindIndex(f => f.AudioTrackId == e.AudioTrackId);
            if (displaySong != -1)
            {
                gvSongs.SelectedRow = displaySong;
                gvSongs.ScrollToRow(displaySong);
            }

            if (song != null)
            {
                formAlbumImage.Show(this, song);
            }

            btnPreviousTrack.Enabled = playbackManager.CanGoPrevious;
        });
    }

    private void PlaybackManagerTrackSkipped(object? sender, TrackSkippedEventArgs e)
    {
        Globals.LoggerSafeInvoke(async () =>
        {
            var albumSong = songs.FirstOrDefault(f => f.AudioTrackId == e.AudioTrackId);
            if (albumSong != null)
            {
                albumSong.AudioTrack!.SkippedEarlyCount = albumSong.AudioTrack.SkippedEarlyCount == null
                    ? 1
                    : albumSong.AudioTrack.SkippedEarlyCount + 1;
                albumSong.AudioTrack.ModifiedAtUtc = DateTime.UtcNow;
                context.Update(albumSong);
                await context.SaveChangesAsync();
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

    private async void PlayNextSongClick(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextTrack(true);
    }

    private async Task<Models.AlbumTrack?> GetSongById(long songId)
    {
        return await Application.Instance.InvokeAsync(Models.AlbumTrack? () =>
        {
            return songs.FirstOrDefault(f => f.AudioTrackId == songId);
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

    private async void AddDirectoryToDatabase_Executed(object? sender, EventArgs e)
    {
        await AddDirectory(sender?.Equals(addDirectoryToAlbum) == true);
    }

    private async void AddFilesToDatabase_Executed(object? sender, EventArgs e)
    {
        await AddAudioFiles(sender?.Equals(addFilesToAlbum) == true);
    }

    private void IdleChecker_UserIdle(object? sender, UserIdleEventArgs e)
    {
        Debug.WriteLine("User idle.");
    }

    private void IdleChecker_UserActivated(object? sender, UserIdleEventArgs e)
    {
        Debug.WriteLine("User woke up.");
    }

    private async void FormMain_Shown(object? sender, EventArgs e)
    {
        await UpdateAlbumDataSource();
        await RefreshCurrentAlbum();
    }

    private void PlaybackManager_PlaybackError(object? sender, PlaybackErrorEventArgs e)
    {
        Globals.Logger?.Error("ManagedBass error occurred '{error}'.", e.Error.ErrorString());
    }

    private void PlaybackManager_PlaybackErrorFileNotFound(object? sender, PlaybackErrorFileNotFoundEventArgs e)
    {
        Globals.Logger?.Error(new FileNotFoundException(string.Format(Messages.File0WasNotFound, e.FileName)), "");
    }

    private async void PlayPreviousClick(object? sender, EventArgs e)
    {
        await playbackManager.PreviousTrack();
    }

    private void ManageSavedQueues_Executed(object? sender, EventArgs e)
    {
        new FormSavedQueues(context).ShowModal(this);
    }

    private async void SaveQueueCommand_Executed(object? sender, EventArgs e)
    {
        string? queueName;
        if (UtilityOS.IsMacOS)
        {
            queueName = new DialogQueryValue<string>("Save current queue", "Queue name", false, Globals.DefaultSpacing,
                Globals.DefaultPadding).ShowModal(this);
        }
        else
        {
            queueName = await new DialogQueryValue<string>("Save current queue", "Queue name", false, Globals.DefaultSpacing,
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

                var queuedSongs = songs.Where(f => f.QueueIndex > 0).Select(f => new QueueTrack
                {
                    QueueSnapshotId = queueSnapshot.Id,
                    AudioTrackId = f.AudioTrackId,
                    CreatedAtUtc = DateTime.UtcNow,
                    QueueIndex = f.QueueIndex,
                }).ToList();

                context.QueueTracks.AddRange(queuedSongs);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
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
}