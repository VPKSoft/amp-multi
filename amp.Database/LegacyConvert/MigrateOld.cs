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

using System.Globalization;
using System.Text;
using amp.Database.Enumerations;
using Microsoft.Data.Sqlite;

namespace amp.Database.LegacyConvert;

/// <summary>
/// A class to migrate data from the old format into the new EF Core database.
/// </summary>
public class MigrateOld
{
    /// <summary>
    /// Migrates the existing data into the EF Core database.
    /// </summary>
    /// <param name="fileNameOld">The old database file name.</param>
    /// <param name="fileNameNew">The new database file name.</param>
    public static void MigrateExistingData(string fileNameOld, string fileNameNew)
    {
        string GetField<T>(SqliteDataReader r, int ordinal)
        {
            if (r.IsDBNull(ordinal))
            {
                return "NULL";
            }

            if (typeof(T) == typeof(string))
            {
                return $"'{r.GetFieldValue<string>(ordinal).Replace("'", "''")}'";
            }

            if (typeof(T) == typeof(double))
            {
                return $"{r.GetFieldValue<double>(ordinal).ToString(CultureInfo.InvariantCulture)}";
            }

            if (typeof(T) == typeof(bool))
            {
                return $"{(r.GetFieldValue<bool>(ordinal) ? "1" : "0")}";
            }

            if (typeof(T) == typeof(DateTime))
            {
                return $"'{r.GetFieldValue<DateTime>(ordinal):yyyy'-'MM'-'dd HH':'mm':'ss}'";
            }

            return $"{r.GetFieldValue<T>(ordinal)}";
        }

        using var connectionOld = new SqliteConnection($"Data Source={fileNameOld}");
        connectionOld.Open();
        using var connection = new SqliteConnection($"Data Source={fileNameNew}");
        connection.Open();

        var sql = string.Join(Environment.NewLine,
            "SELECT ID, FILENAME, ARTIST, ALBUM, TRACK, YEAR, LYRICS,",
            "RATING, NPLAYED_RAND, NPLAYED_USER, FILESIZE, VOLUME, OVERRIDE_NAME,",
            "TAGFINDSTR, TAGREAD, FILENAME_NOPATH, SKIPPED_EARLY, TITLE",
            "FROM",
            "SONG");

        var command = new SqliteCommand(sql, connectionOld);
        var reader = command.ExecuteReader();


        var sqlBatch = new StringBuilder();

        while (reader.Read())
        {
            var fileType = MusicFileType.Unknown;

            var extension = Path.GetExtension(GetField<string>(reader, 1));

            extension = extension.ToUpperInvariant().Trim('.').Trim('\'');
            fileType = extension switch
            {
                "MP3" => MusicFileType.Mp3,
                "OGG" => MusicFileType.Ogg,
                "WAV" => MusicFileType.Wav,
                "WMA" => MusicFileType.Wma,
                "M4A" => MusicFileType.M4a,
                "AAC" => MusicFileType.Aac,
                "AIF" => MusicFileType.Aif,
                "AIFF" => MusicFileType.Aif,
                "FLAC" => MusicFileType.Flac,
                _ => fileType
            };

            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO Song (Id, FileName, Artist, Album, Track, Year, Lyrics,",
                "PlayedByRandomize, PlayedByUser, FileSizeBytes, PlaybackVolume,",
                "OverrideName, TagFindString, TagRead, FileNameNoPath, SkippedEarlyCount, Title, MusicFileType)",
                "SELECT",
                $"{GetField<long>(reader, 0)},",
                $"{GetField<string>(reader, 1)},",
                $"{GetField<string>(reader, 2)},",
                $"{GetField<string>(reader, 3)},",
                $"{GetField<string>(reader, 4)},",
                $"{GetField<string>(reader, 5)},",
                $"{GetField<string>(reader, 6)},",

                $"{GetField<int>(reader, 7)},",
                $"{GetField<int>(reader, 8)},",
                $"{GetField<long>(reader, 9)},",
                $"{GetField<double>(reader, 10)},",

                $"{GetField<string>(reader, 11)},",
                $"{GetField<string>(reader, 12)},",
                $"{GetField<bool>(reader, 13)},",
                $"{GetField<string>(reader, 14)},",
                $"{GetField<int>(reader, 15)},",
                $"{GetField<string>(reader, 16)},",
                $"{(int)fileType};");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();
        }

        reader.Dispose();
        command.Dispose();

        var commandText = sqlBatch.ToString();
        command = new SqliteCommand(commandText, connection);
        command.ExecuteNonQuery();
        command.Dispose();



        sqlBatch = new StringBuilder();
        sql = "SELECT ID, ALBUMNAME FROM ALBUM WHERE ID > 0";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO Album (Id, AlbumName)",
                "SELECT",
                $"{GetField<long>(reader, 0)},",
                $"{GetField<string>(reader, 1)};");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();
        }


        commandText = sqlBatch.ToString();
        command = new SqliteCommand(commandText, connection);
        command.ExecuteNonQuery();
        reader.Dispose();
        command.Dispose();

        sqlBatch = new StringBuilder();
        sql = "SELECT ALBUM_ID, SONG_ID, QUEUEINDEX FROM ALBUMSONGS";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO AlbumSong (AlbumId, SongId, QueueIndex)",
                "SELECT",
                $"{GetField<long>(reader, 0)},",
                $"{GetField<long>(reader, 1)},",
                $"{GetField<int>(reader, 2)};");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();
        }

        commandText = sqlBatch.ToString();
        command = new SqliteCommand(commandText, connection);
        command.ExecuteNonQuery();
        reader.Dispose();
        command.Dispose();


        var snapshotId = 0;
        var previousSnapshotName = string.Empty;

        sqlBatch = new StringBuilder();
        var sqlSubBatch = new StringBuilder();
        sql = "SELECT ID, ALBUM_ID, SONG_ID, QUEUEINDEX, SNAPSHOTNAME, SNAPSHOT_DATE FROM QUEUE_SNAPSHOT";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (previousSnapshotName != GetField<string>(reader, 4))
            {
                snapshotId++;
                var sqlMain = string.Join(Environment.NewLine,
                    "INSERT INTO QueueSnapshot (Id, AlbumId, SnapshotName, SnapshotDate)",
                    "SELECT",
                    $"{snapshotId},",
                    $"{GetField<long>(reader, 1)},",
                    $"{GetField<string>(reader, 4)},",
                    $"{GetField<DateTime>(reader, 5)};");

                sqlBatch.Append(sqlMain);
                sqlBatch.AppendLine();
                previousSnapshotName = GetField<string>(reader, 4);
            }

            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO QueueSong (SongId, QueueSnapshotId, QueueIndex)",
                "SELECT",
                $"{GetField<long>(reader, 2)},",
                $"{snapshotId},",
                $"{GetField<string>(reader, 3)};");

            sqlSubBatch.Append(sqlNew);
            sqlSubBatch.AppendLine();
        }

        commandText = sqlBatch.ToString();
        command = new SqliteCommand(commandText, connection);
        command.ExecuteNonQuery();
        reader.Dispose();
        command.Dispose();

        commandText = sqlSubBatch.ToString();
        command = new SqliteCommand(commandText, connection);
        command.ExecuteNonQuery();
        command.Dispose();
    }
}