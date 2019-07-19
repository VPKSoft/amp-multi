using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void CopyQueueFiles(int queueIndex, string toPath, SQLiteConnection conn)
        {
            string sql =
                string.Join(Environment.NewLine,
                    $"SELECT S.ID, Q.QUEUEINDEX, S.FILENAME",
                    $"FROM",
                    $"SONG S, QUEUE_SNAPSHOT Q",
                    $"WHERE",
                    $"S.ID = Q.SONG_ID AND",
                    $"Q.ID = {queueIndex}",
                    $"ORDER BY Q.QUEUEINDEX;");
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
