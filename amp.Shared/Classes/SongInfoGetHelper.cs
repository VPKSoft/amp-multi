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

using amp.Shared.Interfaces;
using ATL;

namespace amp.Shared.Classes;

/// <summary>
/// Helper class to gather information about the song from the tag and file info.
/// </summary>
public static class SongInfoGetHelper
{
    /// <summary>
    /// Updates the song information.
    /// </summary>
    /// <param name="song">The song to update.</param>
    /// <param name="fileInfo">The file information.</param>
    public static void UpdateSongInfo(this ISong song, FileInfo fileInfo)
    {
        var track = new Track(fileInfo.FullName);

        var updating = song.Id != 0;

        song.Album = track.Album;
        song.Artist = track.Artist;
        song.FileName = fileInfo.FullName;
        song.FileNameNoPath = fileInfo.Name;
        song.FileSizeBytes = fileInfo.Length;
        song.MusicFileType = FileExtensionConvert.FileNameToFileType(fileInfo.FullName);
        song.Lyrics = updating && !string.IsNullOrWhiteSpace(song.Lyrics) ? song.Lyrics : track.Lyrics.UnsynchronizedLyrics;
        song.Title = track.Title ?? Path.GetFileNameWithoutExtension(fileInfo.Name);

        if (!updating)
        {
            song.PlaybackVolume = 1;
            song.Rating = 500;
        }

        song.TagFindString = track.GetTagString();
        song.TagRead = true;

        if (updating)
        {
            song.ModifiedAtUtc = DateTime.UtcNow;
        }
        else
        {
            song.CreatedAtUtc = DateTime.UtcNow;
        }
    }
}