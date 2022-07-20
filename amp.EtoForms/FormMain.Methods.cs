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
using amp.EtoForms.Dialogs;
using amp.EtoForms.ExtensionClasses;
using amp.EtoForms.Layout;
using amp.EtoForms.Properties;
using amp.EtoForms.Utilities;
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
        var modifyTracks = tracks.Where(f => updateQueueData.ContainsKey(f.Id)).ToList();

        foreach (var albumTrack in modifyTracks)
        {
            var newIndex = updateQueueData.First(f => f.Key == albumTrack.Id).Value;
            albumTrack.QueueIndex = newIndex;
            albumTrack.ModifiedAtUtc = DateTime.UtcNow;
            context.AlbumTracks.Update(Globals.AutoMapper.Map<AlbumTrack>(albumTrack));

            var trackIndex = filteredTracks.FindIndex(f => f.Id == albumTrack.Id);
            if (trackIndex != -1)
            {
                filteredTracks[trackIndex] = albumTrack;
            }
        }

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        gvAudioTracks.Invalidate();
    }

    /// <summary>
    /// Refreshes the current album.
    /// </summary>
    private async Task RefreshCurrentAlbum()
    {
        tracks = await context.AlbumTracks.Where(f => f.AlbumId == CurrentAlbumId).Include(f => f.AudioTrack).Select(f => Globals.AutoMapper.Map<Models.AlbumTrack>(f)).AsNoTracking()
            .ToListAsync();

        tracks = tracks.OrderBy(f => f.DisplayName).ToList();

        filteredTracks = tracks;

        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            filteredTracks = tracks.Where(f => f.AudioTrack!.Match(tbSearch.Text)).ToList();
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
}