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

namespace amp.Playback.Classes;

/// <summary>
/// A result data class for the <see cref="NextSongIndex"/> method.
/// Implements the <see cref="IPlayBackStatistics" />
/// </summary>
/// <seealso cref="IPlayBackStatistics" />
public class SongResult : IPlayBackStatistics
{
    /// <summary>
    /// Gets the empty instance of the <see cref="SongResult"/> class.
    /// </summary>
    public static SongResult Empty => new() { SongId = 0, NextSongIndex = -1, };

    /// <summary>
    /// Gets or sets the song identifier indexed by the <see cref="NextSongIndex"/>.
    /// </summary>
    /// <value>The song identifier indexed by the <see cref="NextSongIndex"/>.</value>
    public long SongId { get; init; }

    /// <summary>
    /// Gets or sets the index of the next song.
    /// </summary>
    /// <value>The index of the next song.</value>
    public int NextSongIndex { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByRandomize"/>
    public int? PlayedByRandomize { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByUser"/>
    public int? PlayedByUser { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.SkippedEarlyCount"/>
    public int? SkippedEarlyCount { get; set; }
}