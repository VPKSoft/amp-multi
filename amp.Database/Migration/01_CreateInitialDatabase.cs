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
using amp.Database.Enumerations;
using amp.Database.Interfaces;
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
        Create.Table(nameof(Song))
            .WithColumn(nameof(Song.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(Song.FileName)).AsString().Nullable()
            .WithColumn(nameof(Song.Artist)).AsString().Nullable()
            .WithColumn(nameof(Song.Album)).AsString().Nullable()
            .WithColumn(nameof(Song.Track)).AsString().Nullable()
            .WithColumn(nameof(Song.Year)).AsString().Nullable()
            .WithColumn(nameof(Song.Lyrics)).AsString().Nullable()
            .WithColumn(nameof(Song.Rating)).AsInt32().Nullable()
            .WithColumn(nameof(Song.PlayedByRandomize)).AsInt32().Nullable()
            .WithColumn(nameof(Song.PlayedByUser)).AsInt32().Nullable()
            .WithColumn(nameof(Song.FileSizeBytes)).AsInt64().Nullable()
            .WithColumn(nameof(Song.PlaybackVolume)).AsDouble().NotNullable().WithDefaultValue(1.0)
            .WithColumn(nameof(Song.OverrideName)).AsString().Nullable()
            .WithColumn(nameof(Song.TagFindString)).AsString().Nullable()
            .WithColumn(nameof(Song.TagRead)).AsBoolean().Nullable()
            .WithColumn(nameof(Song.FileNameNoPath)).AsString().Nullable()
            .WithColumn(nameof(Song.SkippedEarlyCount)).AsInt32().Nullable()
            .WithColumn(nameof(Song.Title)).AsString().Nullable()
            .WithColumn(nameof(Song.SongImageData)).AsBinary().Nullable()
            .WithColumn(nameof(Song.MusicFileType)).AsInt32().NotNullable().WithDefaultValue((int)MusicFileType.Unknown);

        Create.Table(nameof(Album))
            .WithColumn(nameof(Album.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(Album.AlbumName)).AsString().NotNullable();

        Create.Table(nameof(AlbumSong))
            .WithColumn(nameof(AlbumSong.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(AlbumSong.AlbumId)).AsInt64().ForeignKey(nameof(Album), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(AlbumSong.SongId)).AsInt64().ForeignKey(nameof(Song), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(AlbumSong.QueueIndex)).AsInt32().NotNullable().WithDefaultValue(0);

        Create.Table(nameof(QueueSnapshot))
            .WithColumn(nameof(QueueSnapshot.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(QueueSnapshot.AlbumId)).AsInt32().ForeignKey(nameof(Album), nameof(IEntity.Id))
            .NotNullable()
            .WithColumn(nameof(QueueSnapshot.SnapshotName)).AsString().NotNullable()
            .WithColumn(nameof(QueueSnapshot.SnapshotDate)).AsDateTime2().NotNullable();

        Create.Table(nameof(QueueSong))
            .WithColumn(nameof(QueueSnapshot.Id)).AsInt64().Identity().PrimaryKey().NotNullable()
            .WithColumn(nameof(QueueSong.SongId)).AsInt32().ForeignKey(nameof(Song), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(QueueSong.QueueSnapshotId)).AsInt32()
            .ForeignKey(nameof(QueueSnapshot), nameof(IEntity.Id)).NotNullable()
            .WithColumn(nameof(QueueSong.QueueIndex)).AsInt32().NotNullable();
    }

    /// <inheritdoc cref="MigrationBase.Down"/>
    public override void Down()
    {
        Delete.Table(nameof(AlbumSong));
        Delete.Table(nameof(Album));
        Delete.Table(nameof(Song));
    }
}
