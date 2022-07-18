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
/// A link between an audio track and an album.
/// Implements the <see cref="IEntity" />
/// </summary>
/// <seealso cref="IEntity" />
public interface IAlbumTrack<TAudioTrack, TAlbum> : IEntity where TAudioTrack : IAudioTrack where TAlbum : IAlbum
{
    /// <summary>
    /// Gets or sets the album reference identifier.
    /// </summary>
    /// <value>The album reference identifier.</value>
    long AlbumId { get; set; }

    /// <summary>
    /// Gets or sets the audio track reference identifier.
    /// </summary>
    /// <value>The audio track reference identifier.</value>
    long AudioTrackId { get; set; }

    /// <summary>
    /// Gets or sets the audio track index in the queue.
    /// </summary>
    /// <value>The audio track index in the queue.</value>
    int QueueIndex { get; set; }

    /// <summary>
    /// Gets or sets the audio track index in the alternate queue.
    /// </summary>
    /// <value>The audio track index in the alternate queue.</value>
    int QueueIndexAlternate { get; set; }

    /// <summary>
    /// Gets or sets the audio track entity.
    /// </summary>
    /// <value>The audio track entity.</value>
    TAudioTrack? AudioTrack { get; set; }

    /// <summary>
    /// Gets or sets the album this album audio track belongs to.
    /// </summary>
    /// <value>The album.</value>
    TAlbum? Album { get; set; }
}