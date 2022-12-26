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
using amp.DataAccessLayer.DtoClasses;
using amp.Database;
using amp.Database.ExtensionClasses;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms.Classes;

/// <summary>
/// Extensions methods for the track list operations.
/// </summary>
internal static class AlbumTrackMethods
{
    /// <summary>
    /// Deletes the selected songs from the track list.
    /// </summary>
    /// <param name="gridView">The grid view with <see cref="AudioTrack"/> items.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="tracks">The data source of all the <see cref="AudioTrack"/>s in the album.</param>
    /// <param name="filterAction">The filter action to refresh the list filtering.</param>
    public static async Task DeleteSongs(this GridView gridView, AmpContext context, ObservableCollection<AlbumTrack> tracks, Action filterAction)
    {
        if (gridView.SelectedItems.Any())
        {
            var selectedTracks = gridView.SelectedItems.Cast<AlbumTrack>().ToList();
            var result = await Globals.LoggerSafeInvokeAsync(async () =>
            {
                var selectedTrackIds = selectedTracks.Select(track => track.Id).ToArray();
                var toRemove = await context.AlbumTracks.Where(f => selectedTrackIds.Contains(f.Id)).ToListAsync();
                context.AlbumTracks.RemoveRange(toRemove);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            });

            if (result)
            {
                selectedTracks.ForEach(f => tracks.Remove(f));
                filterAction();
            }
        }
    }

    /// <summary>
    /// Renames the selected track from the track list.
    /// </summary>
    /// <param name="gridView">The grid view with <see cref="AudioTrack"/> items.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="owner">The owner for the rename <see cref="Dialog.ShowModal(Control)"/> method.</param>
    public static async Task RenameTrack(this GridView gridView, AmpContext context, Control owner)
    {
        if (gridView.SelectedItem != null)
        {
            var albumTrack = (AlbumTrack)gridView.SelectedItem;
            var dialog = new DialogQueryValue<string>("Rename track", "Track name", false, Globals.DefaultSpacing,
                Globals.DefaultPadding, albumTrack.DisplayName);
            // ReSharper disable once MethodHasAsyncOverload, Wait for MacOS bug fix, see: https://github.com/picoe/Eto/issues/2254
            var newName = dialog.ShowModal(owner);
            var updateTrack = await context.AudioTracks.FirstAsync(f => f.Id == albumTrack.AudioTrackId);
            updateTrack.OverrideName = newName;
            updateTrack.ModifiedAtUtc = DateTime.UtcNow;
            albumTrack.AudioTrack!.OverrideName = newName;

            gridView.ReloadData(gridView.SelectedRow);

            await context.SaveChangesAndUntrackAsync(updateTrack);
        }
    }

    /// <summary>
    /// Updates the track volume.
    /// </summary>
    /// <param name="albumTrack">The album track which audio track volume to update.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="volume">The new volume value.</param>
    public static async Task UpdateTrackVolume(AlbumTrack albumTrack, AmpContext context, double volume)
    {
        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var updateTrack = await context.AudioTracks.FirstAsync(f => f.Id == albumTrack.AudioTrackId);
            updateTrack.PlaybackVolume = volume;
            updateTrack.ModifiedAtUtc = DateTime.UtcNow;
            albumTrack.AudioTrack!.PlaybackVolume = volume;
            await context.SaveChangesAndUntrackAsync(updateTrack);
        });
    }

    /// <summary>
    /// Updates the track rating.
    /// </summary>
    /// <param name="albumTrack">The album track which audio track rating to update.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="trackRating">The new track rating.</param>
    public static async Task UpdateTrackRating(AlbumTrack albumTrack, AmpContext context, int trackRating)
    {
        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var updateTrack = await context.AudioTracks.FirstAsync(f => f.Id == albumTrack.AudioTrackId);
            updateTrack.Rating = trackRating;
            updateTrack.ModifiedAtUtc = DateTime.UtcNow;
            albumTrack.AudioTrack!.Rating = trackRating;
            await context.SaveChangesAndUntrackAsync(updateTrack);
        });
    }

    public static async Task IncrementUserPlayed(AlbumTrack albumTrack, AmpContext context)
    {
        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var updateTrack = await context.AudioTracks.FirstAsync(f => f.Id == albumTrack.AudioTrackId);
            updateTrack.PlayedByUser++;
            updateTrack.ModifiedAtUtc = DateTime.UtcNow;
            if (albumTrack.AudioTrack != null)
            {
                albumTrack.AudioTrack.PlayedByUser = updateTrack.PlayedByUser;
                albumTrack.AudioTrack.ModifiedAtUtc = updateTrack.ModifiedAtUtc;
            }
            await context.SaveChangesAndUntrackAsync(updateTrack);
        });
    }
}