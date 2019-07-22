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
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace amp.SQLiteDatabase.DatabaseUtils
{
    /// <summary>
    /// A class containing some utilities dealing with the saved queues.
    /// </summary>
    public class QueueUtilities
    {
        /// <summary>
        /// Copies the queue files into a given directory.
        /// </summary>
        /// <param name="queueIndex">Index of the queue.</param>
        /// <param name="toPath">The path to copy the files to.</param>
        /// <param name="conn">A reference to a <see cref="SQLiteConnection"/> class instance.</param>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static void CopyQueueFiles(int queueIndex, string toPath, SQLiteConnection conn)
        {
            string sql =
                string.Join(Environment.NewLine,
                    "SELECT S.ID, Q.QUEUEINDEX, S.FILENAME",
                    "FROM",
                    "SONG S, QUEUE_SNAPSHOT Q",
                    "WHERE",
                    "S.ID = Q.SONG_ID AND",
                    $"Q.ID = {queueIndex}",
                    "ORDER BY Q.QUEUEINDEX;");
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string sourceFile = reader.GetString(2);
                        string fileName = Path.GetFileName(reader.GetString(2));
                        if (fileName != null)
                        {
                            string destinationFile = Path.Combine(toPath,
                                fileName);

                            File.Copy(sourceFile, destinationFile, true);
                        }
                    }
                }
            }

        }
    }
}
