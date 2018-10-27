using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp
{
    public partial class FormDatabaseUpdatingProgress : DBLangEngineWinforms
    {
        public FormDatabaseUpdatingProgress()
        {
            InitializeComponent();

            DBLangEngine.DBName = "lang.sqlite";
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("amp.Messages");
        }

        public void SetProgress(int percentage, int maximum)
        {
            pbUpdateProgress.Value = percentage;

            int current = maximum / 100 * percentage;
            lbItemIndex.Text = $"({current}/{maximum})";
        }

        public void CenterForm(Form frm)
        {
            if (frm != null)
            {
                return;
            }
            Left = frm.Left + (frm.Width - Width) / 2;
            Top = frm.Top + (frm.Height - Height) / 2;
        }

        SQLiteConnection conn = null;
        private int maximum;

        private void FormDatabaseUpdatingProgress_Shown(object sender, EventArgs e)
        {
            conn = new SQLiteConnection("Data Source=" + DBLangEngine.DataDir + "amp.sqlite;Pooling=true;FailIfMissing=false;Cache Size=10000;"); // PRAGMA synchronous=OFF;PRAGMA journal_mode=OFF
            conn.Open();

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
                        if (File.Exists(dr.GetString(0)))
                        {
                            updateFiles.Add(new MusicFile(dr.GetString(0), dr.GetInt32(1)));
                        }
                        else
                        {
                            updateFiles.Add(null); // nice (weird) logic..
                        }

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

        private bool canCloseForm = false;

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
