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

using amp.Shared.Enumerations;

namespace amp.Shared.Interfaces;

/// <summary>
/// An interface for the audio track data in the database.
/// Implements the <see cref="IEntity" />
/// </summary>
/// <seealso cref="IEntity" />
public interface IAudioTrack : IEntity, IPlayBackStatistics
{
    /// <summary>
    /// Gets or sets the name of the file of the audio track.
    /// </summary>
    /// <value>The name of the file.</value>
    string FileName { get; set; }

    /// <summary>
    /// Gets or sets the name of the artist of the audio track.
    /// </summary>
    /// <value>The name of the artist.</value>
    string? Artist { get; set; }

    /// <summary>
    /// Gets or sets the album name of the audio track.
    /// </summary>
    /// <value>The album name.</value>
    string? Album { get; set; }

    /// <summary>
    /// Gets or sets the track information of the audio track.
    /// </summary>
    /// <value>The track information.</value>
    string? Track { get; set; }

    /// <summary>
    /// Gets or sets the release year of the audio track.
    /// </summary>
    /// <value>The release year.</value>
    string? Year { get; set; }

    /// <summary>
    /// Gets or sets the lyrics of the audio track.
    /// </summary>
    /// <value>The lyrics.</value>
    string? Lyrics { get; set; }

    /// <summary>
    /// Gets or sets the rating for the audio track.
    /// </summary>
    /// <value>The rating.</value>
    int? Rating { get; set; }

    /// <summary>
    /// Gets or sets the file size in bytes of the audio track.
    /// </summary>
    /// <value>The file size in bytes.</value>
    long? FileSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the playback volume of the audio track.
    /// </summary>
    /// <value>The playback volume.</value>
    double PlaybackVolume { get; set; }

    /// <summary>
    /// Gets or sets the name to override the original audio track name.
    /// </summary>
    /// <value>The name to override the original audio track name.</value>
    string? OverrideName { get; set; }

    /// <summary>
    /// Gets or sets the tag find string. E.g. the tag data in a single string.
    /// </summary>
    /// <value>The tag find string.</value>
    string? TagFindString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the audio track tag data has been read.
    /// </summary>
    /// <value><c>true</c> if audio track tag data has been read; otherwise, <c>false</c>.</value>
    bool? TagRead { get; set; }

    /// <summary>
    /// Gets or sets the name of the file of the audio track without the path.
    /// </summary>
    /// <value>The name of the file without the path.</value>
    string? FileNameNoPath { get; set; }

    /// <summary>
    /// Gets or sets the title of the audio track.
    /// </summary>
    /// <value>The title of the audio track.</value>
    string? Title { get; set; }

    /// <summary>
    /// Gets or sets the audio track image (cover, picture, etc) data.
    /// </summary>
    /// <value>The audio track image data.</value>
    byte[]? TrackImageData { get; set; }

    /// <summary>
    /// Gets or sets the type of the music file.
    /// </summary>
    /// <value>The type of the music file.</value>
    MusicFileType MusicFileType { get; set; }
}