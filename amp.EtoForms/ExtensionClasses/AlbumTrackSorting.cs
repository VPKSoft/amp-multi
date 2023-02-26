#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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
using amp.EtoForms.Enumerations;

namespace amp.EtoForms.ExtensionClasses;

/// <summary>
/// Helper methods for album track sorting.
/// </summary>
internal static class AlbumTrackSorting
{
    /// <summary>
    /// Applies the default filtering and sorting to the specified <see cref="AlbumTrack"/> collection.
    /// </summary>
    /// <param name="tracks">The tracks to filter and sort.</param>
    /// <param name="ratingSorting">The rating sorting order.</param>
    /// <param name="useFuzzy">if set to <c>true</c> use FuzzyWuzzy algorithm for string matching.</param>
    /// <param name="searchText">The search text.</param>
    /// <returns>ObservableCollection&lt;AlbumTrack&gt;.</returns>
    internal static ObservableCollection<AlbumTrack> DefaultFilterSort(
        this IEnumerable<AlbumTrack> tracks, ColumnSorting ratingSorting, bool useFuzzy, string? searchText = null)
    {
        IEnumerable<AlbumTrack>? sortedTracks;

        var albumTracks = tracks.ToList();

        if (searchText != null)
        {
            if (useFuzzy)
            {
                if (ratingSorting == ColumnSorting.Ascending)
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.FuzzyMatchScore(searchText)
                                    >= Globals.Settings.FuzzyWuzzyTolerance)
                        .Take(Globals.Settings.FuzzyWuzzyMaxResults)
                        .OrderBy(f => f.AudioTrack!.Rating)
                        .ThenBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
                else if (ratingSorting == ColumnSorting.Descending)
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.FuzzyMatchScore(searchText)
                                    >= Globals.Settings.FuzzyWuzzyTolerance)
                        .Take(Globals.Settings.FuzzyWuzzyMaxResults)
                        .OrderByDescending(f => f.AudioTrack!.Rating)
                        .ThenBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
                else
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.FuzzyMatchScore(searchText) 
                                    >= Globals.Settings.FuzzyWuzzyTolerance)
                        .Take(Globals.Settings.FuzzyWuzzyMaxResults)
                        .OrderBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
            }
            else
            {
                if (ratingSorting == ColumnSorting.Ascending)
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.Match(searchText))
                        .OrderBy(f => f.AudioTrack!.Rating)
                        .ThenBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
                else if (ratingSorting == ColumnSorting.Descending)
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.Match(searchText))
                        .OrderByDescending(f => f.AudioTrack!.Rating)
                        .ThenBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
                else
                {
                    sortedTracks = albumTracks
                        .Where(f => f.AudioTrack!.Match(searchText))
                        .OrderBy(f => f.AudioTrack!.FuzzyMatchScore(searchText))
                        .ThenBy(f => f.DisplayName);
                }
            }
        }
        else
        {
            if (ratingSorting == ColumnSorting.Ascending)
            {
                sortedTracks = albumTracks
                    .OrderBy(f => f.AudioTrack!.Rating)
                    .ThenBy(f => f.DisplayName);
            }
            else if (ratingSorting == ColumnSorting.Descending)
            {
                sortedTracks = albumTracks
                    .OrderByDescending(f => f.AudioTrack!.Rating)
                    .ThenBy(f => f.DisplayName);
            }
            else
            {
                sortedTracks = albumTracks
                    .OrderBy(f => f.DisplayName);
            }
        }

        return new ObservableCollection<AlbumTrack>(sortedTracks.ToList());
    }
}