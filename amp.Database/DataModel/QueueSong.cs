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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amp.Database.Interfaces;

namespace amp.Database.DataModel;

/// <summary>
/// A single song in a queue snapshot.
/// Implements the <see cref="IQueueSong" />
/// </summary>
/// <seealso cref="IQueueSong" />
[Table(nameof(QueueSong))]
public class QueueSong : IQueueSong
{
    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    [Key]
    public long Id { get; set; }

    /// <inheritdoc cref="IQueueSong.SongId"/>
    public long SongId { get; set; }

    /// <inheritdoc cref="IQueueSong.QueueSnapshotId"/>
    public long QueueSnapshotId { get; set; }

    /// <inheritdoc cref="IQueueSong.QueueIndex"/>
    public int QueueIndex { get; set; }

    /// <summary>
    /// Gets or sets the song of this queue song.
    /// </summary>
    /// <value>The song of this queue song.</value>
    [ForeignKey(nameof(SongId))]
    public Song? Song { get; set; }

    /// <summary>
    /// Gets or sets the queue snapshot reference.
    /// </summary>
    /// <value>The queue snapshot reference.</value>
    [ForeignKey(nameof(QueueSnapshotId))]
    public QueueSnapshot? QueueSnapshot { get; set; }
}