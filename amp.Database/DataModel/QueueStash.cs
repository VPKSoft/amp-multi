﻿#region License
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amp.Shared.Interfaces;

namespace amp.Database.DataModel;

/// <summary>
/// An entity for stashing and popping the current queue from and into the database.
/// Implements the <see cref="IQueueStash" />
/// </summary>
/// <seealso cref="IQueueStash" />
[Table(nameof(QueueStash))]
public class QueueStash : IQueueStash, IRowVersionEntity, ICreatedAt
{
    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    [Key]
    public long Id { get; set; }

    /// <inheritdoc cref="ICreatedAt.CreatedAtUtc" />
    public DateTime CreatedAtUtc { get; set; }

    /// <inheritdoc cref="IQueueStash.AudioTrackId" />
    public long AudioTrackId { get; set; }

    /// <inheritdoc cref="IQueueStash.AlbumId" />
    public long AlbumId { get; set; }

    /// <inheritdoc cref="IQueueStash.QueueIndex" />
    public int QueueIndex { get; set; }

    /// <inheritdoc cref="IRowVersionEntity.RowVersion" />
    [Timestamp]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public byte[]? RowVersion { get; set; }
}