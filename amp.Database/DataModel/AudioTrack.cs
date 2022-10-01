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
using amp.Shared.Enumerations;
using amp.Shared.Interfaces;

namespace amp.Database.DataModel;

/// <summary>
/// A single audio track data in the amp# database.
/// Implements the <see cref="IAudioTrack" />
/// </summary>
/// <seealso cref="IAudioTrack" />
[Table(nameof(AudioTrack))]
// ReSharper disable once ClassNeverInstantiated.Global, EF Core class
public class AudioTrack : IAudioTrack, IRowVersionEntity
{
    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    [Key]
    public long Id { get; set; }

    /// <inheritdoc cref="IAudioTrack.FileName"/>
    public string FileName { get; set; } = string.Empty;

    /// <inheritdoc cref="IAudioTrack.FilePath"/>
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc cref="IAudioTrack.Artist"/>
    public string? Artist { get; set; }

    /// <inheritdoc cref="IAudioTrack.Album"/>
    public string? Album { get; set; }

    /// <inheritdoc cref="IAudioTrack.Track"/>
    public string? Track { get; set; }

    /// <inheritdoc cref="IAudioTrack.Year"/>
    public string? Year { get; set; }

    /// <inheritdoc cref="IAudioTrack.Lyrics"/>
    public string? Lyrics { get; set; }

    /// <inheritdoc cref="IAudioTrack.Rating"/>
    public int? Rating { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByRandomize"/>
    public int? PlayedByRandomize { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByUser"/>
    public int? PlayedByUser { get; set; }

    /// <inheritdoc cref="IAudioTrack.FileSizeBytes"/>
    public long? FileSizeBytes { get; set; }

    /// <inheritdoc cref="IAudioTrack.PlaybackVolume"/>
    public double PlaybackVolume { get; set; }

    /// <inheritdoc cref="IAudioTrack.OverrideName"/>
    public string? OverrideName { get; set; }

    /// <inheritdoc cref="IAudioTrack.TagFindString"/>
    public string? TagFindString { get; set; }

    /// <inheritdoc cref="IAudioTrack.TagRead"/>
    public bool? TagRead { get; set; }

    /// <inheritdoc cref="IPlayBackStatistics.SkippedEarlyCount"/>
    public int? SkippedEarlyCount { get; set; }

    /// <inheritdoc cref="IAudioTrack.Title"/>
    public string? Title { get; set; }

    /// <inheritdoc cref="IAudioTrack.TrackImageData"/>
    public byte[]? TrackImageData { get; set; }

    /// <inheritdoc cref="IAudioTrack.MusicFileType"/>
    public MusicFileType MusicFileType { get; set; }

    /// <inheritdoc cref="IModifiedAt.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc { get; set; }

    /// <inheritdoc cref="ICreatedAt.CreatedAtUtc"/>
    public DateTime CreatedAtUtc { get; set; }

    /// <inheritdoc cref="IRowVersionEntity.RowVersion"/>
    [Timestamp]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public byte[]? RowVersion { get; set; }
}