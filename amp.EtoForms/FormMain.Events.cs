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
using amp.Shared.Localization;
using Eto.Forms;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.UserIdle;

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
                var albumSong = (AlbumSong)gvSongs.SelectedItem;
                await playbackManager.PlaySong(albumSong, true);
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
                var songId = ((AlbumSong)gvSongs.SelectedItem).Id;
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
                var songId = ((AlbumSong)gvSongsSelectedItem).Id;
                yield return songId;
            }
        }
    }

    private async void NextSongCommand_Executed(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextSong(true);
    }

    private void PlaybackPosition_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        playbackManager.PlaybackPositionPercentage = e.Value;
    }

    private async void GvSongsMouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var song = songs.First(f => f.Id == SelectedAlbumSongId);
        if (song.Song?.PlayedByUser != null)
        {
            song.Song.PlayedByUser++;
            song.Song.ModifiedAtUtc = DateTime.UtcNow;
            context.AlbumSongs.Update(song);
            await context.SaveChangesAsync();
        }
        await playbackManager.PlaySong(song, true);
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
            filteredSongs = songs.Where(f => f.Song!.Match(tbSearch.Text)).ToList();
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

    private async Task<AlbumSong?> GetNextSongFunc()
    {
        AlbumSong? result = null;
        await Application.Instance.InvokeAsync(async () =>
        {
            var nextSongData = await playbackOrder.NextSong(songs);
            result = songs[nextSongData.NextSongIndex];
            if (result.Song != null)
            {
                result.Song.PlayedByRandomize ??= 0;
                result.Song.PlayedByUser ??= 0;

                result.Song.PlayedByRandomize += nextSongData.PlayedByRandomize;
                result.Song.PlayedByUser += nextSongData.PlayedByUser;
                result.Song.ModifiedAtUtc = DateTime.UtcNow;

                context.Update(result);

                await context.SaveChangesAsync();
            }
        });

        return result;
    }

    private async void PlaybackManager_PlaybackStateChanged(object? sender, PlaybackStateChangedArgs e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            var song = songs.FirstOrDefault(f => f.SongId == e.SongId);
            lbSongsTitle.Text = song?.GetSongName() ?? string.Empty;
            btnPlayPause.CheckedChange -= PlayPauseToggle;
            btnPlayPause.Checked = e.PlaybackState == PlaybackState.Playing;
            btnPlayPause.CheckedChange += PlayPauseToggle;
        });
    }

    private void PlaybackManager_SongChanged(object? sender, SongChangedArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            var song = songs.FirstOrDefault(f => f.SongId == e.SongId);
            songVolumeSlider.SuspendEventInvocation = true;
            songVolumeSlider.Value = song?.Song?.PlaybackVolume * 100 ?? 100;
            songVolumeSlider.SuspendEventInvocation = false;
            lbSongsTitle.Text = song?.GetSongName() ?? string.Empty;

            var dataSource = gvSongs.DataStore.Cast<AlbumSong>().ToList();
            var displaySong = dataSource.FindIndex(f => f.SongId == e.SongId);
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

    private void PlaybackManager_SongSkipped(object? sender, SongSkippedEventArgs e)
    {
        Globals.LoggerSafeInvoke(async () =>
        {
            var albumSong = songs.FirstOrDefault(f => f.SongId == e.SongId);
            if (albumSong != null)
            {
                albumSong.Song!.SkippedEarlyCount = albumSong.Song.SkippedEarlyCount == null
                    ? 1
                    : albumSong.Song.SkippedEarlyCount + 1;
                albumSong.Song.ModifiedAtUtc = DateTime.UtcNow;
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
        await playbackManager.PlayNextSong(true);
    }

    private async Task<AlbumSong?> GetSongById(long songId)
    {
        return await Application.Instance.InvokeAsync(AlbumSong? () =>
        {
            return songs.FirstOrDefault(f => f.SongId == songId);
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
        await playbackManager.PreviousSong();
    }
}