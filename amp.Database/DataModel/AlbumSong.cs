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
/// A link table to album songs.
/// Implements the <see cref="IAlbumSong{T}" />
/// </summary>
/// <seealso cref="IAlbumSong{T}" />
[Table(nameof(AlbumSong))]
// ReSharper disable once ClassNeverInstantiated.Global, EF Core class
public class AlbumSong : IAlbumSong<Song>
{
    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    [Key]
    public long Id { get; set; }

    /// <inheritdoc cref="IAlbumSong{T}.AlbumId"/>
    public long AlbumId { get; set; }

    /// <inheritdoc cref="IAlbumSong{T}.SongId"/>
    public long SongId { get; set; }

    /// <inheritdoc cref="IAlbumSong{T}.QueueIndex"/>
    public int QueueIndex { get; set; }

    /// <inheritdoc cref="IAlbumSong{T}.QueueIndexAlternate"/>
    public int QueueIndexAlternate { get; set; }

    /// <summary>
    /// Gets or sets the album this album song belongs to.
    /// </summary>
    /// <value>The album.</value>
    [ForeignKey(nameof(AlbumId))]
    public Album? Album { get; set; }

    /// <inheritdoc cref="IAlbumSong{T}.Song"/>
    [ForeignKey(nameof(SongId))]
    public Song? Song { get; set; }
}