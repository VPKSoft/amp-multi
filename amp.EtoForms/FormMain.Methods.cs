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

using amp.Database.ExtensionClasses;
using amp.EtoForms.Dialogs;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Properties;
using amp.Shared.Constants;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
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

    private async Task UpdateQueueFunc(Dictionary<long, int> updateQueueData)
    {
        var modifySongs = songs.Where(f => updateQueueData.ContainsKey(f.Id)).ToList();

        foreach (var albumSong in modifySongs)
        {
            var newIndex = updateQueueData.First(f => f.Key == albumSong.Id).Value;
            albumSong.QueueIndex = newIndex;
            albumSong.ModifiedAtUtc = DateTime.UtcNow;
            context.AlbumSongs.Update(albumSong);

            var songIndex = filteredSongs.FindIndex(f => f.Id == albumSong.Id);
            if (songIndex != -1)
            {
                filteredSongs[songIndex] = albumSong;
            }
        }

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        gvSongs.Invalidate();
    }

    /// <summary>
    /// Refreshes the current album.
    /// </summary>
    private async Task RefreshCurrentAlbum()
    {
        songs = await context.AlbumSongs.Where(f => f.AlbumId == CurrentAlbumId).Include(f => f.Song).AsNoTracking()
            .ToListAsync();

        songs = songs.OrderBy(f => f.DisplayName).ToList();

        filteredSongs = songs;

        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            filteredSongs = songs.Where(f => f.Song!.Match(tbSearch.Text)).ToList();
        }

        gvSongs.DataStore = filteredSongs;
    }

    private void AssignSettings()
    {
        formAlbumImage.AlwaysVisible = !Globals.Settings.AutoHideEmptyAlbumImage;
    }

    private async Task UpdateAlbumDataSource()
    {
        var albums = await context.Albums.GetUnTrackedList(f => f.AlbumName, new long[] { 1, });
        cmbAlbumSelect.DataStore = albums;
        if (albums.Any(f => f.Id == CurrentAlbumId))
        {
            cmbAlbumSelect.SelectedValue = albums.FirstOrDefault(f => f.Id == CurrentAlbumId);
        }
        else
        {
            cmbAlbumSelect.SelectedValue = albums.FirstOrDefault(f => f.Id == 1);
        }
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
}