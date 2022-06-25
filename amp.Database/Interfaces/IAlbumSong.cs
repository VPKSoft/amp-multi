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

namespace amp.Database.Interfaces;

/// <summary>
/// A ling between a song and an album.
/// Implements the <see cref="amp.Database.Interfaces.IEntity" />
/// </summary>
/// <seealso cref="amp.Database.Interfaces.IEntity" />
public interface IAlbumSong<TSong> : IEntity where TSong : ISong
{
    /// <summary>
    /// Gets or sets the album reference identifier.
    /// </summary>
    /// <value>The album reference identifier.</value>
    long AlbumId { get; set; }

    /// <summary>
    /// Gets or sets the song reference identifier.
    /// </summary>
    /// <value>The song reference identifier.</value>
    long SongId { get; set; }

    /// <summary>
    /// Gets or sets the song index in the queue.
    /// </summary>
    /// <value>The song index in the queue.</value>
    int QueueIndex { get; set; }

    /// <summary>
    /// Gets or sets the song index in the alternate queue.
    /// </summary>
    /// <value>The song index in the alternate queue.</value>
    int QueueIndexAlternate { get; set; }

    /// <summary>
    /// Gets or sets the song entity.
    /// </summary>
    /// <value>The song entity.</value>
    TSong? Song { get; set; }
}