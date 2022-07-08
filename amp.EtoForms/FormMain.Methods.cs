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

using amp.EtoForms.Dialogs;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Localization;
using amp.Shared.Constants;
using Eto.Forms;
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
            DialogAddFilesProgress.ShowModal(this, context, toAlbum ? currentAlbumId : 0, dialog.Filenames.ToArray());
        }

        await RefreshCurrentAlbum();
    }

    private async Task AddDirectory(bool toAlbum)
    {
        using var dialog = new SelectFolderDialog { Title = UI.SelectMusicFolder, };
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            DialogAddFilesProgress.ShowModal(this, context, dialog.Directory, toAlbum ? currentAlbumId : 0);
        }

        await RefreshCurrentAlbum();
    }

    private async Task UpdateQueueFunc(Dictionary<long, int> updateQueueData)
    {
        var modifySongs = songs.Where(f => updateQueueData.ContainsKey(f.Id)).ToList();

        foreach (var albumSong in modifySongs)
        {
            var newIndex = updateQueueData.First(f => f.Key == albumSong.Id).Value;
            albumSong.QueueIndex = newIndex;
            albumSong.ModifiedAtUtc = DateTime.UtcNow;
            context.AlbumSongs.Update(albumSong);
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Refreshes the current album.
    /// </summary>
    private async Task RefreshCurrentAlbum()
    {
        songs = await context.AlbumSongs.Where(f => f.AlbumId == currentAlbumId).Include(f => f.Song).AsNoTracking()
            .ToListAsync();

        var albumSongs = songs;

        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            albumSongs = albumSongs.Where(f => f.Song!.Match(tbSearch.Text)).ToList();
        }

        gvSongs.DataStore = albumSongs;
    }
}