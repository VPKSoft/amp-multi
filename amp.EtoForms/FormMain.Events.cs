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

using amp.Database.DataModel;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Utilities;
using amp.Playback.Enumerations;
using Eto.Forms;
using EtoForms.Controls.Custom.EventArguments;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms;

partial class FormMain
{
    private async void FormMain_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Equals(sender, tbSearch))
        {
            if (e.Key is Keys.Up or Keys.Down or Keys.PageDown or Keys.PageUp or Keys.Equal)
            {
                gvSongs.Focus();
            }
            return;
        }

        if (e.Key == Keys.Add)
        {
            var song = songs.First(f => f.Id == SelectedAlbumSongId);
            await playbackOrder.ToggleQueue(songs, song.Id);

            gvSongs.Invalidate();

            e.Handled = true;
            return;
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

    private async void NextSongCommand_Executed(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextSong(true);
    }

    private void CommandPlayPause_Executed(object? sender, EventArgs e)
    {
        if (SelectedAlbumSongId != 0)
        {
            var song = songs.First(f => f.Id == SelectedAlbumSongId);
            playbackManager.PlaySong(song, true);
        }
    }

    private void PlaybackPosition_ValueChanged(object? sender, global::EtoForms.Controls.Custom.EventArguments.ValueChangedEventArgs e)
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
        playbackManager.PlaySong(song, true);
    }

    private void TbSearch_TextChanged(object? sender, EventArgs e)
    {
        var albumSongs = songs;

        albumSongs = albumSongs.Where(f => f.Song!.Match(tbSearch.Text)).ToList();

        var items = albumSongs
            .Select(f => new ListItem { Text = f.GetSongName(), Key = f.Id.ToString(), });

        gvSongs.DataStore = items;
    }

    private void FormMain_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        playbackManager.Dispose();
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
                result.Song.PlayedByRandomize += nextSongData.PlayedByRandomize;
                result.Song.PlayedByUser += nextSongData.PlayedByUser;
                result.Song.ModifiedAtUtc = DateTime.UtcNow;

                context.Update(result);

                await context.SaveChangesAsync();
            }
        });

        return result;
    }

    private async void PlaybackManager_PlaybackStateChanged(object? sender, amp.Playback.EventArguments.PlaybackStateChangedArgs e)
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

    private void PlaybackManager_SongChanged(object? sender, Playback.EventArguments.SongChangedArgs e)
    {
        Application.Instance.Invoke(() =>
        {
            var song = songs.FirstOrDefault(f => f.SongId == e.SongId);
            songVolumeSlider.SuspendEventInvocation = true;
            songVolumeSlider.Value = song?.Song?.PlaybackVolume * 100 ?? 100;
            songVolumeSlider.SuspendEventInvocation = false;
            lbSongsTitle.Text = song?.GetSongName() ?? string.Empty;
        });
    }

    private async void PlayPauseToggle(object? sender, global::EtoForms.Controls.Custom.EventArguments.CheckedChangeEventArguments e)
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

    private void PlaybackManager_PlaybackPositionChanged(object? sender, Playback.EventArguments.PlaybackPositionChangedArgs e)
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
}