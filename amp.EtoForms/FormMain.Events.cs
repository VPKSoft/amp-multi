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
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms;

partial class FormMain
{
    private void FormMain_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Equals(sender, tbSearch))
        {
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

    private async void NextSongCommand_Executed(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextSong();
    }

    private void CommandPlayPause_Executed(object? sender, EventArgs e)
    {
        if (long.TryParse(lbSongs.SelectedKey, out var songId))
        {
            var song = context.AlbumSongs.First(f => f.Id == songId);
            playbackManager.PlaySong(song, true);
        }
    }

    private async void LbSongs_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var song = context.AlbumSongs.First(f => f.Id == long.Parse(lbSongs.SelectedKey));
        if (song.Song?.PlayedByUser != null)
        {
            song.Song.PlayedByUser++;
            song.Song.ModifiedAtUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
        playbackManager.PlaySong(song, true);
    }

    private async void TbSearch_TextChanged(object? sender, EventArgs e)
    {
        lbSongs.Items.Clear();

        var songs = await context.AlbumSongs.Include(f => f.Song)
            .Where(f => f.AlbumId == 1).ToListAsync();

        songs = songs.Where(f => f.Song!.Match(tbSearch.Text)).ToList();

        var items = songs
            .Select(f => new ListItem { Text = f.GetSongName(), Key = f.Id.ToString(), });

        lbSongs.Items.AddRange(items);
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
            if (songs.Any(f => f.QueueIndex > 0))
            {
                result = songs.Where(f => f.QueueIndex > 0).OrderBy(f => f.QueueIndex).First();
                result.QueueIndex = 0;
                result.ModifiedAtUtc = DateTime.UtcNow;
                foreach (var song in songs.Where(f => f.QueueIndex > 0))
                {
                    song.QueueIndex--;
                    song.ModifiedAtUtc = DateTime.UtcNow;
                }

                await context.SaveChangesAsync();
                return;
            }

            result = songs[playbackOrder.NextSongIndex(songs)];
            if (result.Song != null)
            {
                result.Song.PlayedByRandomize++;
                result.Song.ModifiedAtUtc = DateTime.UtcNow;
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
}