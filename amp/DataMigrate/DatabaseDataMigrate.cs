#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using amp.SQLiteDatabase;
using amp.UtilityClasses;
using Ookii.Dialogs.WinForms;
using VPKSoft.Utils;

namespace amp.DataMigrate
{
    /// <summary>
    /// A helper class for moving the music library contents (collected data) to another computer.
    /// </summary>
    public class DatabaseDataMigrate: DatabaseHelpers
    {
        /// <summary>
        /// Gets the directories of the SONG database table.
        /// </summary>
        /// <param name="connection">The <see cref="SQLiteConnection"/> instance to access the database.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the character case is ignored upon returning the distinct list of directories.</param>
        /// <param name="maxDepth">The maximum depth.</param>
        /// <returns>A distinct list of directories in the software's SONG database table.</returns>
        public static List<string> GetDirectories(SQLiteConnection connection, bool ignoreCase, int maxDepth = -1)
        {
            List<string> result = new List<string>();

            string sql =
                string.Join(Environment.NewLine,
                    // ReSharper disable once StringLiteralTypo
                    "SELECT DISTINCT SUBSTR(FILENAME, 1, LENGTH(FILENAME) - LENGTH(FILENAME_NOPATH) - 1)",
                    // ReSharper disable once StringLiteralTypo
                    "COLLATE NOCASE",
                    "FROM SONG;");

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var path = reader.GetString(0);
                        string addPath = SplitPathStringMax(path, maxDepth);
                        result.Add(addPath);
                    }
                }
            }

            return result.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(f => f).ToList();
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static BackgroundWorker DeleteNonExistingSongs(SQLiteConnection connection)
        {
            string sql =
                string.Join(Environment.NewLine,
                    
                    "SELECT ID, FILENAME, ARTIST, ALBUM,", 
                    "TRACK, YEAR, LYRICS, RATING, NPLAYED_RAND,",
                    "NPLAYED_USER, FILESIZE, VOLUME, OVERRIDE_NAME,",
                    "TAGFINDSTR, TAGREAD, FILENAME_NOPATH,",
                    "SKIPPED_EARLY, TITLE FROM SONG;");

            BackgroundWorker worker = new BackgroundWorker
                {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    }
                }
            }

            worker.DoWork += (sender, args) => { };

            return worker;
        }

        public static BackgroundWorker UpdateSongLocations(
            VistaFolderBrowserDialog folderBrowserDialog, SQLiteConnection connection)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                BackgroundWorker worker = new BackgroundWorker
                    {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

                worker.DoWork += (sender, args) =>
                {
                    List<FileEnumeratorFileEntry> files = new List<FileEnumeratorFileEntry>(FileEnumerator
                        .RecurseFiles(folderBrowserDialog.SelectedPath,
                            Constants.Extensions.ToArray()).ToArray());

                    if (files.Count == 0)
                    {
                        worker.ReportProgress(100);
                        return;
                    }

                    int fileCount = files.Count;

                    long songCount = GetScalar<long>(Song.GenerateCountSentence(false), connection);

                    int progress = 0;

                    List<Song> songs = new List<Song>();
                    using (SQLiteCommand command = new SQLiteCommand(Song.GenerateSelectSentence(false), connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var percentage = progress * 50 / (int) songCount;
                                worker.ReportProgress(percentage);
                                songs.Add(Song.FromSqLiteDataReader(reader));
                                worker.ReportProgress(progress);
                                progress++;

                                if (worker.CancellationPending)
                                {
                                    return;
                                }
                            }
                        }
                    }

                    progress = 0;
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var song = songs.First(f =>
                            f.FileSize == fileInfo.Length && f.FileNameNoPath == file.FileName);

                        worker.ReportProgress(50 + progress * 50 / fileCount);

                        song.Filename = file.FileName;
                        ExecuteSql(song.GenerateInsertUpdateSqlSentence(false), connection);
                        progress++;

                        if (worker.CancellationPending)
                        {
                            return;
                        }
                    }
                };

                return worker;
            }

            return null;
        }

        /// <summary>
        /// Regexp: ^[a-zA-Z]:\\ (for etc: C:\).
        /// </summary>
        private static Regex NormalDirectory { get; set; } = new Regex(@"^[a-zA-Z]:\\", RegexOptions.Compiled);

        /// <summary>
        /// Regexp: ^\\\\ (for UNC-paths).
        /// </summary>
        private static Regex UncDirectory { get; set; } = new Regex(@"^\\\\", RegexOptions.Compiled);

        /// <summary>
        /// Splits a path string to a given <paramref name="max"/> amount of directories.
        /// </summary>
        /// <param name="path">The path to remove directories from the ending point.</param>
        /// <param name="max">The maximum amount of directories to leave.</param>
        /// <returns>A combined path string containing at most the <paramref name="max"/> amount of directories.</returns>
        public static string SplitPathStringMax(string path, int max)
        {
            if (max <= 0)
            {
                return path;
            }

            string pathStart = string.Empty;
            if (NormalDirectory.IsMatch(path))
            {
                pathStart = NormalDirectory.Match(path).Value;
            }
            else if (UncDirectory.IsMatch(path))
            {
                pathStart = UncDirectory.Match(path).Value;
            }

            string tmpString = path.Substring(pathStart.Length);

            List<string> pathParts = new List<string>(tmpString.Split(new[] {Path.DirectorySeparatorChar},
                StringSplitOptions.RemoveEmptyEntries));

            while (pathParts.Count > max)
            {
                pathParts.RemoveAt(pathParts.Count - 1);
            }


            string result = pathStart + string.Join(Path.DirectorySeparatorChar.ToString(), pathParts.ToArray());

            return result;
        }
    }
}
