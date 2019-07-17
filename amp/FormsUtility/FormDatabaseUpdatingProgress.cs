#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp.FormsUtility
{
    public partial class FormDatabaseUpdatingProgress : DBLangEngineWinforms
    {
        public FormDatabaseUpdatingProgress()
        {
            InitializeComponent();

            // ReSharper disable once StringLiteralTypo
            DBLangEngine.DBName = "lang.sqlite";
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("amp.Messages");
        }

        public void SetProgress(int percentage, int maximumValue)
        {
            pbUpdateProgress.Value = percentage;

            int current = maximumValue / 100 * percentage;
            lbItemIndex.Text = $@"({current}/{maximumValue})";
        }

        SQLiteConnection conn;
        private int maximum;

        private void FormDatabaseUpdatingProgress_Shown(object sender, EventArgs e)
        {
            // ReSharper disable once StringLiteralTypo
            conn = new SQLiteConnection("Data Source=" + DBLangEngine.DataDir +
                                        "amp.sqlite;Pooling=true;FailIfMissing=false;Cache Size=10000;"); // PRAGMA synchronous=OFF;PRAGMA journal_mode=OFF
            conn.Open();

            // ReSharper disable once StringLiteralTypo
            if (!ScriptRunner.RunScript(DBLangEngine.DataDir + "amp.sqlite"))
            {
                MessageBox.Show(
                    DBLangEngine.GetMessage("msgErrorInScript",
                    "A script error occurred on the database update|Something failed during running the database update script"),
                    DBLangEngine.GetMessage("msgError", "Error|A message describing that some kind of error occurred."),
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                // at this point there is no reason to continue the program's execution as the database might be in an invalid state..
                throw new Exception(DBLangEngine.GetMessage("msgErrorInScript",
                    "A script error occurred on the database update|Something failed during running the database update script"));
            }
            bwDatabaseUpdate.RunWorkerAsync();
        }

        private void bwDatabaseUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM SONG ";

            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                maximum = (int)Convert.ToInt64(command.ExecuteScalar());
            }

            maximum *= 2;
            int maxValue = maximum;
            int currentValue = 0;

            List<MusicFile> updateFiles = new List<MusicFile>();

            sql = "SELECT FILENAME, ID FROM SONG ";

            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        currentValue++;
                        updateFiles.Add(File.Exists(dr.GetString(0))
                            ? new MusicFile(dr.GetString(0), dr.GetInt32(1))
                            : null);

                        if ((currentValue % 100) == 0)
                        {
                            bwDatabaseUpdate.ReportProgress((int)(currentValue / (double)maxValue * 100));
                        }
                    }
                }
            }

            sql = string.Empty;
            foreach (MusicFile mf in updateFiles)
            {
                if (mf != null)
                {
                    mf.LoadTag(true);
                    sql += $"UPDATE SONG SET TITLE = {Database.QS(mf.Title)} WHERE ID = {mf.ID}; " + Environment.NewLine;
                }
                if ((currentValue % 100) == 0)
                {
                    if (sql != string.Empty)
                    {
                        using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                        sql = string.Empty;
                    }
                    bwDatabaseUpdate.ReportProgress((int)(currentValue / (double)maxValue * 100));
                }
                currentValue++;
            }

            bwDatabaseUpdate.ReportProgress(100);
        }

        private void bwDatabaseUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            using (conn)
            {
                conn.Close();
            }
            canCloseForm = true;
            Close();
        }

        private bool canCloseForm;

        private void bwDatabaseUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetProgress(e.ProgressPercentage, maximum);
        }

        private void FormDatabaseUpdatingProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canCloseForm;
            if (!canCloseForm)
            {
                BringToFront();
            }
        }
    }
}
