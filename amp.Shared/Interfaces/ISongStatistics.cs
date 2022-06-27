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

namespace amp.Shared.Interfaces;

/// <summary>
/// An interface for song playback statistics with reference indentifier.
/// Implements the <see cref="amp.Shared.Interfaces.IEntityBase{T}" />
/// </summary>
/// <seealso cref="amp.Shared.Interfaces.IEntityBase{T}" />
public interface ISongStatistics : IEntityBase<long>
{
    /// <summary>
    /// Gets or sets the rating for the song.
    /// </summary>
    /// <value>The rating.</value>
    int? Rating { get; set; }

    /// <summary>
    /// Gets or sets the amount the song was played by randomization.
    /// </summary>
    /// <value>The amount the song was played by randomization.</value>
    int? PlayedByRandomize { get; set; }

    /// <summary>
    /// Gets or sets the amount the song was played by the user.
    /// </summary>
    /// <value>The amount the song was played by the user.</value>
    int? PlayedByUser { get; set; }

    /// <summary>
    /// Gets or sets the count the song was skipped by user interaction.
    /// </summary>
    /// <value>The skipped early count.</value>
    int? SkippedEarlyCount { get; set; }
}