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

using amp.Database.DataModel;
using amp.Shared.Enumerations;
using amp.Shared.Interfaces;
using FluentMigrator;

namespace amp.Database.Migration;

/// <summary>
/// Performs the initial migration for the EF Core database.
/// Implements the <see cref="Migration" />
/// </summary>
/// <seealso cref="Migration" />
[Migration(1)]
public class CreateInitialDatabase : FluentMigrator.Migration
{
    /// <inheritdoc cref="MigrationBase.Up"/>
    public override void Up()
    {
        Create.Table(nameof(AudioTrack))
            .WithColumn(nameof(AudioTrack.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(AudioTrack.FileName)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Artist)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Album)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Track)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Year)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Lyrics)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.Rating)).AsInt32().Nullable()
            .WithColumn(nameof(AudioTrack.PlayedByRandomize)).AsInt32().Nullable()
            .WithColumn(nameof(AudioTrack.PlayedByUser)).AsInt32().Nullable()
            .WithColumn(nameof(AudioTrack.FileSizeBytes)).AsInt64().Nullable()
            .WithColumn(nameof(AudioTrack.PlaybackVolume)).AsDouble().NotNullable().WithDefaultValue(1.0)
            .WithColumn(nameof(AudioTrack.OverrideName)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.TagFindString)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.TagRead)).AsBoolean().Nullable()
            .WithColumn(nameof(AudioTrack.FileNameNoPath)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.SkippedEarlyCount)).AsInt32().Nullable()
            .WithColumn(nameof(AudioTrack.Title)).AsString().Nullable()
            .WithColumn(nameof(AudioTrack.TrackImageData)).AsBinary().Nullable()
            .WithColumn(nameof(AudioTrack.MusicFileType)).AsInt32().NotNullable().WithDefaultValue((int)MusicFileType.Unknown)
            .WithColumn(nameof(IEntity.ModifiedAtUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(IEntity.CreatedAtUtc)).AsDateTime2().NotNullable();

        Create.Table(nameof(Album))
            .WithColumn(nameof(Album.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(Album.AlbumName)).AsString().NotNullable()
            .WithColumn(nameof(IEntity.ModifiedAtUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(IEntity.CreatedAtUtc)).AsDateTime2().NotNullable();

        Create.Table(nameof(AlbumTrack))
            .WithColumn(nameof(AlbumTrack.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(AlbumTrack.AlbumId)).AsInt64().ForeignKey(nameof(Album), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(AlbumTrack.AudioTrackId)).AsInt64().ForeignKey(nameof(AudioTrack), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(AlbumTrack.QueueIndex)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AlbumTrack.QueueIndexAlternate)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(IEntity.ModifiedAtUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(IEntity.CreatedAtUtc)).AsDateTime2().NotNullable();

        Create.Table(nameof(QueueSnapshot))
            .WithColumn(nameof(QueueSnapshot.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(QueueSnapshot.AlbumId)).AsInt64().ForeignKey(nameof(Album), nameof(IEntity.Id))
            .NotNullable()
            .WithColumn(nameof(QueueSnapshot.SnapshotName)).AsString().NotNullable()
            .WithColumn(nameof(QueueSnapshot.SnapshotDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(IEntity.ModifiedAtUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(IEntity.CreatedAtUtc)).AsDateTime2().NotNullable();

        Create.Table(nameof(QueueTrack))
            .WithColumn(nameof(QueueSnapshot.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(QueueTrack.AudioTrackId)).AsInt64().ForeignKey(nameof(AudioTrack), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(QueueTrack.QueueSnapshotId)).AsInt64().ForeignKey(nameof(QueueSnapshot), nameof(IEntity.Id)).NotNullable()
            .ForeignKey(nameof(QueueSnapshot), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(QueueTrack.QueueIndex)).AsInt32().NotNullable()
            .WithColumn(nameof(IEntity.ModifiedAtUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(IEntity.CreatedAtUtc)).AsDateTime2().NotNullable();
    }

    /// <inheritdoc cref="MigrationBase.Down"/>
    public override void Down()
    {
        Delete.Table(nameof(AlbumTrack));
        Delete.Table(nameof(Album));
        Delete.Table(nameof(QueueTrack));
        Delete.Table(nameof(QueueSnapshot));
        Delete.Table(nameof(AudioTrack));
    }
}
