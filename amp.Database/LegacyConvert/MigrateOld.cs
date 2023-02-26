#region License
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

using System.Globalization;
using System.Text;
using amp.Shared.Enumerations;
using Microsoft.Data.Sqlite;
using VPKSoft.DropOutStack;

namespace amp.Database.LegacyConvert;

/// <summary>
/// A class to migrate data from the old format into the new EF Core database.
/// </summary>
public static class MigrateOld
{
    /// <summary>
    /// Gets statistic numbers from the old database.
    /// </summary>
    /// <param name="fileNameOld">The old database file name.</param>
    /// <returns>A value tuple with the statistics.</returns>
    public static (int audioTracks, int albums, int albumTracks, int queueSnaphots) OldDatabaseStatistics(string fileNameOld)
    {
        using var connectionOld = new SqliteConnection($"Data Source={fileNameOld}");
        connectionOld.Open();
        var audioTracks = GetTableCount(connectionOld, "SONG");
        var albums = GetTableCount(connectionOld, "ALBUM") - 1;
        var albumTracks = GetTableCount(connectionOld, "ALBUMSONGS");
        var queueSnapshots = GetQueueCountTotal(connectionOld);
        return (audioTracks, albums, albumTracks, queueSnapshots);
    }

    private static int GetTableCount(SqliteConnection connection, string tableName)
    {
        using var sqlCommand = new SqliteCommand($"SELECT COUNT(*) FROM {tableName}", connection);
        return Convert.ToInt32(sqlCommand.ExecuteScalar());
    }

    private static int GetQueueCountTotal(SqliteConnection connection)
    {
        using var sqlCommand = new SqliteCommand("SELECT COUNT(*) + COUNT(DISTINCT ID) FROM QUEUE_SNAPSHOT", connection);
        return Convert.ToInt32(sqlCommand.ExecuteScalar());
    }


    /// <summary>
    /// Occurs when the database migration progress changes.
    /// </summary>
    public static event EventHandler<ConvertProgressArgs>? ReportProgress;

    /// <summary>
    /// Occurs when database migration thread has been stopped.
    /// </summary>
    public static event EventHandler<ConvertProgressArgs>? ThreadStopped;

    private static volatile bool stopConvert;

    /// <summary>
    /// Creates a thread which converts the old database into the new EF Core format.
    /// </summary>
    /// <param name="fileNameOld">The old database file name.</param>
    /// <param name="fileNameNew">The new database file name.</param>
    /// <param name="transactionRowLimit">An amount of rows to insert in a single transaction into the database.</param>
    public static void RunConvert(string fileNameOld, string fileNameNew, int transactionRowLimit = 100)
    {
        var totals = OldDatabaseStatistics(fileNameOld);
        var allCount = totals.albumTracks + totals.albums + totals.queueSnaphots + totals.audioTracks;

        var migrations = GenerateMigrationData(fileNameOld, fileNameNew, transactionRowLimit);

        var audioTracks = 0;
        var albums = 0;
        var albumEntries = 0;
        var queueSnapshots = 0;

        var timeStack = new DropOutStack<(DateTime start, DateTime end)>(10);
        var secondsAverage = 0.0;

        stopConvert = false;

        var migrateDataThread = new Thread(new ThreadStart(delegate
        {
            void RiseThreadStopped()
            {
                var entriesLeft = (double)allCount - (audioTracks + albums + queueSnapshots + albumEntries);
                ThreadStopped?.Invoke(null,
                    new ConvertProgressArgs
                    {
                        AudioTracksHandledCount = audioTracks,
                        AudioTracksCountTotal = totals.audioTracks,
                        AlbumsHandledCount = albums,
                        AlbumsCountTotal = totals.albums,
                        AlbumEntriesHandledCount = albumEntries,
                        AlbumEntryCountTotal = totals.albumTracks,
                        QueueEntriesHandledCount = queueSnapshots,
                        QueueEntryCountTotal = totals.queueSnaphots,
                        CountTotal = allCount,
                        Eta = secondsAverage == 0 ? null : DateTime.Now.AddSeconds(entriesLeft * secondsAverage),
                    });
            }

            using var connection = new SqliteConnection($"Data Source={fileNameNew}");
            connection.Open();
            foreach (var migration in migrations)
            {
                for (var i = 0; i < migration.Value.Count; i++)
                {
                    if (stopConvert)
                    {
                        RiseThreadStopped();
                        stopConvert = true;
                        return;
                    }

                    var commandText = migration.Value[i];
                    using var sqlCommand = new SqliteCommand(commandText, connection);
                    var start = DateTime.Now;
                    var affected = sqlCommand.ExecuteNonQuery();
                    timeStack.Push((start, DateTime.Now));
                    secondsAverage = affected == 0
                        ? 0
                        : timeStack.ToArray().Select(f => (f.end - f.start).TotalSeconds).Average() / affected;

                    switch (migration.Key)
                    {
                        case 0:
                            audioTracks += affected;
                            break;
                        case 1:
                            albums += affected;
                            break;
                        case 2:
                            albumEntries += affected;
                            break;
                        case 3:
                            queueSnapshots += affected;
                            break;
                    }

                    var entriesLeft = (double)allCount - (audioTracks + albums + queueSnapshots + albumEntries);
                    ReportProgress?.Invoke(null,
                        new ConvertProgressArgs
                        {
                            AudioTracksHandledCount = audioTracks,
                            AudioTracksCountTotal = totals.audioTracks,
                            AlbumsHandledCount = albums,
                            AlbumsCountTotal = totals.albums,
                            AlbumEntriesHandledCount = albumEntries,
                            AlbumEntryCountTotal = totals.albumTracks,
                            QueueEntriesHandledCount = queueSnapshots,
                            QueueEntryCountTotal = totals.queueSnaphots,
                            CountTotal = allCount,
                            Eta = secondsAverage == 0
                                ? null
                                : DateTime.Now.AddSeconds(entriesLeft * secondsAverage),
                        });

                    if (stopConvert)
                    {
                        RiseThreadStopped();
                        stopConvert = true;
                        return;
                    }
                }
            }

            stopConvert = true;
            RiseThreadStopped();
        }));

        migrateDataThread.Start();
    }

    /// <summary>
    /// Stops the database conversion thread.
    /// </summary>
    public static void AbortConversion()
    {
        stopConvert = true;
    }

    /// <summary>
    /// Collects a SQL sentence collection which migrates the existing data into the EF Core database.
    /// </summary>
    /// <param name="fileNameOld">The old database file name.</param>
    /// <param name="fileNameNew">The new database file name.</param>
    /// <param name="transactionRowLimit">The amount of rows to insert in a single transaction.</param>
    private static Dictionary<int, List<string>> GenerateMigrationData(string fileNameOld, string fileNameNew, int transactionRowLimit = 100)
    {
        var result = new Dictionary<int, List<string>>();

        string GetField<T>(SqliteDataReader r, int ordinal)
        {
            try
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
                    return $"{(r.GetFieldValue<long>(ordinal) == 1 ? "1" : "0")}";
                }

                if (typeof(T) == typeof(DateTime))
                {
                    return $"'{r.GetFieldValue<DateTime>(ordinal):yyyy'-'MM'-'dd HH':'mm':'ss}'";
                }

                if (typeof(T) == typeof(long))
                {
                    return r.GetFieldValue<long>(ordinal).ToString();
                }

                return $"{r.GetFieldValue<T>(ordinal)}";
            }
            catch
            {
                return string.Empty;
            }
        }

        void AppendBatch(StringBuilder sqlBatch, int key)
        {
            if (sqlBatch.Length == 0)
            {
                return;
            }

            if (result.ContainsKey(key))
            {
                result[key].Add(sqlBatch.ToString());
                sqlBatch.Clear();
            }
            else
            {
                result.Add(key, new List<string>(new[] { sqlBatch.ToString(), }));
            }

            sqlBatch.Clear();
        }


        using var connectionOld = new SqliteConnection($"Data Source={fileNameOld}");
        connectionOld.Open();
        using var connection = new SqliteConnection($"Data Source={fileNameNew}");
        connection.Open();

        var sql = string.Join(Environment.NewLine,
            "SELECT",
            "ID, ",                 // 0
            "FILENAME, ",           // 1
            "ARTIST, ",             // 2
            "ALBUM, ",              // 3
            "TRACK, ",              // 4
            "YEAR, ",               // 5
            "LYRICS,",              // 6
            "RATING, ",             // 7
            "NPLAYED_RAND, ",       // 8
            "NPLAYED_USER, ",       // 9
            "FILESIZE,",            // 10
            "VOLUME, ",             // 11
            "OVERRIDE_NAME,",       // 12
            "TAGFINDSTR, ",         // 13
            "TAGREAD, ",            // 14
            "FILENAME_NOPATH, ",    // 15
            "SKIPPED_EARLY, ",      // 16
            "TITLE",                // 17
            "FROM",
            "SONG");

        var command = new SqliteCommand(sql, connectionOld);
        var reader = command.ExecuteReader();


        var counter = 0;

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
                _ => fileType,
            };

            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO AudioTrack (" +
                "Id, ",                 // 0
                "FileName, ",           // 1
                "Artist, ",             // 2
                "Album, ",              // 3
                "Track, ",              // 4
                "Year, ",               // 5
                "Lyrics, ",             // 6
                "Rating, ",             // 7
                "PlayedByRandomize,",   // 8
                "PlayedByUser, ",       // 9
                "FileSizeBytes, ",      // 10
                "PlaybackVolume,",      // 11
                "OverrideName, ",       // 12
                "TagFindString, ",      // 13
                "TagRead, ",            // 14
                "FileNameNoPath, ",     // 15
                "SkippedEarlyCount, ",  // 16
                "Title, ",              // 17
                "MusicFileType," +      // 18
                "CreatedAtUtc)",        // 19
                "SELECT",
                $"{GetField<long>(reader, 0)},",    // 9
                $"{GetField<string>(reader, 1)},",  // 1
                $"{GetField<string>(reader, 2)},",  // 2
                $"{GetField<string>(reader, 3)},",  // 3
                $"{GetField<string>(reader, 4)},",  // 4
                $"{GetField<string>(reader, 5)},",  // 5
                $"{GetField<string>(reader, 6)},",  // 6
                $"{GetField<long>(reader, 7)},",    // 7
                $"{GetField<long>(reader, 8)},",    // 8
                $"{GetField<long>(reader, 9)},",    // 9
                $"{GetField<long>(reader, 10)},",   // 10
                $"{GetField<double>(reader, 11)},", // 11
                $"{GetField<string>(reader, 12)},", // 12
                $"{GetField<string>(reader, 13)},", // 13
                $"{GetField<bool>(reader, 14)},",   // 14
                $"{GetField<string>(reader, 15)},", // 15
                $"{GetField<long>(reader, 16)},",   // 16
                $"{GetField<string>(reader, 17)},", // 17
                $"{(int)fileType},",                       // 18
                $"'{DateTime.UtcNow:yyyy'-'MM'-'dd HH':'mm':'ss}';");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();
            sqlBatch.AppendLine();

            counter++;

            if ((counter % transactionRowLimit) == 0)
            {
                AppendBatch(sqlBatch, 0);
            }
        }

        AppendBatch(sqlBatch, 0);

        reader.Dispose();
        command.Dispose();
        counter = 0;

        sqlBatch = new StringBuilder();
        sql = "SELECT ID, ALBUMNAME FROM ALBUM WHERE ID > 0";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO Album (Id, AlbumName, CreatedAtUtc)",
                "SELECT",
                $"{GetField<long>(reader, 0)},",
                $"{GetField<string>(reader, 1)},",
                $"'{DateTime.UtcNow:yyyy'-'MM'-'dd HH':'mm':'ss}';");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();

            counter++;

            if ((counter % transactionRowLimit) == 0)
            {
                AppendBatch(sqlBatch, 1);
            }
        }
        AppendBatch(sqlBatch, 1);

        reader.Dispose();
        command.Dispose();
        counter = 0;

        sqlBatch = new StringBuilder();
        sql = "SELECT ALBUM_ID, SONG_ID, QUEUEINDEX FROM ALBUMSONGS";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            var albumId = reader.GetInt64(0);

            if (albumId == 0) // The temporary album is no longer in use.
            {
                continue;
            }

            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO AlbumTrack (AlbumId, AudioTrackId, QueueIndex, CreatedAtUtc)",
                "SELECT",
                $"{albumId},",
                $"{GetField<long>(reader, 1)},",
                $"{GetField<long>(reader, 2)},",
                $"'{DateTime.UtcNow:yyyy'-'MM'-'dd HH':'mm':'ss}';");

            sqlBatch.Append(sqlNew);
            sqlBatch.AppendLine();


            counter++;

            if ((counter % transactionRowLimit) == 0)
            {
                AppendBatch(sqlBatch, 2);
            }
        }
        AppendBatch(sqlBatch, 2);

        reader.Dispose();
        command.Dispose();
        counter = 0;


        var snapshotId = 0;
        var previousSnapshotName = string.Empty;

        sqlBatch = new StringBuilder();
        sql = "SELECT ID, ALBUM_ID, SONG_ID, QUEUEINDEX, SNAPSHOTNAME, SNAPSHOT_DATE FROM QUEUE_SNAPSHOT";
        command = new SqliteCommand(sql, connectionOld);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (previousSnapshotName != GetField<string>(reader, 4))
            {
                snapshotId++;
                var sqlMain = string.Join(Environment.NewLine,
                    "INSERT INTO QueueSnapshot (Id, AlbumId, SnapshotName, SnapshotDate, CreatedAtUtc)",
                    "SELECT",
                    $"{snapshotId},",
                    $"{GetField<long>(reader, 1)},",
                    $"{GetField<string>(reader, 4)},",
                    $"{GetField<DateTime>(reader, 5)},",
                    $"'{DateTime.UtcNow:yyyy'-'MM'-'dd HH':'mm':'ss}';");

                sqlBatch.Append(sqlMain);
                sqlBatch.AppendLine();
                previousSnapshotName = GetField<string>(reader, 4);
            }

            var sqlNew = string.Join(Environment.NewLine,
                "INSERT INTO QueueTrack (AudioTrackId, QueueSnapshotId, QueueIndex, CreatedAtUtc)",
                "SELECT",
                $"{GetField<long>(reader, 2)},",
                $"{snapshotId},",
                $"{GetField<long>(reader, 3)},",
                $"'{DateTime.UtcNow:yyyy'-'MM'-'dd HH':'mm':'ss}';");

            sqlBatch.Append(sqlNew);

            counter++;

            if ((counter % transactionRowLimit) == 0)
            {
                AppendBatch(sqlBatch, 3);
            }

        }

        AppendBatch(sqlBatch, 3);
        reader.Dispose();
        command.Dispose();

        return result;
    }
}