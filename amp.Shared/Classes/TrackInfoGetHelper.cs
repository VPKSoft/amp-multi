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

using amp.Shared.Interfaces;
using ATL;

namespace amp.Shared.Classes;

/// <summary>
/// Helper class to gather information about the audioTrack from the tag and file info.
/// </summary>
public static class TrackInfoGetHelper
{
    /// <summary>
    /// Updates the audioTrack information.
    /// </summary>
    /// <param name="audioTrack">The audioTrack to update.</param>
    /// <param name="fileInfo">The file information.</param>
    public static void UpdateTrackInfo(this IAudioTrack audioTrack, FileInfo fileInfo)
    {
        var track = new Track(fileInfo.FullName);

        var updating = audioTrack.Id != 0;

        audioTrack.Album = track.Album;
        audioTrack.Artist = track.Artist;
        audioTrack.FileName = fileInfo.Name;
        audioTrack.FilePath = fileInfo.DirectoryName ?? string.Empty;
        audioTrack.FileSizeBytes = fileInfo.Length;
        audioTrack.MusicFileType = FileExtensionConvert.FileNameToFileType(fileInfo.FullName);
        audioTrack.Lyrics = updating && !string.IsNullOrWhiteSpace(audioTrack.Lyrics) ? audioTrack.Lyrics : track.Lyrics.UnsynchronizedLyrics;
        audioTrack.Title = track.Title ?? Path.GetFileNameWithoutExtension(fileInfo.Name);

        if (!updating)
        {
            audioTrack.PlaybackVolume = 1;
            audioTrack.Rating = 500;
        }

        audioTrack.TagFindString = track.GetTagString();
        audioTrack.TagRead = true;

        if (updating)
        {
            audioTrack.ModifiedAtUtc = DateTime.UtcNow;
        }
        else
        {
            audioTrack.CreatedAtUtc = DateTime.UtcNow;
        }
    }
}