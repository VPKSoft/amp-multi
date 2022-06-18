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
/// An interface for the songs in a queue snapshot.
/// Implements the <see cref="amp.Database.Interfaces.IEntity" />
/// </summary>
/// <seealso cref="amp.Database.Interfaces.IEntity" />
public interface IQueueSong : IEntity
{
    /// <summary>
    /// Gets or sets the song reference identifier.
    /// </summary>
    /// <value>The song reference identifier.</value>
    long SongId { get; set; }

    /// <summary>
    /// Gets or sets the queue snapshot reference identifier.
    /// </summary>
    /// <value>The queue snapshot reference identifier.</value>
    long QueueSnapshotId { get; set; }

    /// <summary>
    /// Gets or sets the index of song in the queue.
    /// </summary>
    /// <value>The index of song in the queue.</value>
    int QueueIndex { get; set; }
}