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
using amp.Shared.Interfaces;

namespace amp.Database.DataModel;

/// <summary>
/// A single audio track in a queue snapshot.
/// Implements the <see cref="IQueueTrack" />
/// </summary>
/// <seealso cref="IQueueTrack" />
[Table(nameof(QueueTrack))]
// ReSharper disable once ClassNeverInstantiated.Global, EF Core class
public class QueueTrack : IQueueTrack
{
    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    [Key]
    public long Id { get; set; }

    /// <inheritdoc cref="IQueueTrack.AudioTrackId"/>
    public long AudioTrackId { get; set; }

    /// <inheritdoc cref="IQueueTrack.QueueSnapshotId"/>
    public long QueueSnapshotId { get; set; }

    /// <inheritdoc cref="IQueueTrack.QueueIndex"/>
    public int QueueIndex { get; set; }

    /// <inheritdoc cref="IEntity.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc { get; set; }

    /// <inheritdoc cref="IEntity.CreatedAtUtc"/>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the audio track of this queue track.
    /// </summary>
    /// <value>The audio track of this queue track.</value>
    [ForeignKey(nameof(AudioTrackId))]
    public AudioTrack? AudioTrack { get; set; }

    /// <summary>
    /// Gets or sets the queue snapshot reference.
    /// </summary>
    /// <value>The queue snapshot reference.</value>
    [ForeignKey(nameof(QueueSnapshotId))]
    public QueueSnapshot? QueueSnapshot { get; set; }
}