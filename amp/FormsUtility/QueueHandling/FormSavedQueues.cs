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
using System.Globalization;
using System.Windows.Forms;
using amp.SQLiteDatabase;
using amp.SQLiteDatabase.DatabaseUtils;
using VPKSoft.LangLib;

namespace amp.FormsUtility.QueueHandling
{
    public partial class FormSavedQueues : DBLangEngineWinforms
    {
        public FormSavedQueues()
        {
            InitializeComponent();
            colQueueSaveTime.Name = "colQueueSaveTime"; // the columns have only a design name.. LangLib doesn't apply on the nameless components 
            colQueueName.Name = "colQueueName"; // the columns have only a design name.. LangLib doesn't apply on the nameless components

            DBLangEngine.DBName = "lang.sqlite";
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("amp.Messages");

            try // as this can be translated to a invalid format :-)
            {
                odExportQueue.Filter = DBLangEngine.GetMessage("msgFileExt_amp_qex", "amp# queue export files (*.amp#_qex)|*.amp#_qex|as in the combo box to select file type from a dialog");
                sdExportQueue.Filter = DBLangEngine.GetMessage("msgFileExt_amp_qex", "amp# queue export files (*.amp#_qex)|*.amp#_qex|as in the combo box to select file type from a dialog");
            }
            catch
            {
                // ignored..
            }

            odExportQueue.Title = DBLangEngine.GetMessage("msgImportQueueFrom", "Import queue from file|As in import a queue snapshot from a file");
            sdExportQueue.Title = DBLangEngine.GetMessage("msgExportQueueTo", "Export queue to file|As in export a queue snapshot to a file");

            fbdDirectory.Description = DBLangEngine.GetMessage("msgQueueCopyFlat",
                "Copy songs into a single directory|A title to a folder select dialog indicating that files in a queue should be copied into a single directory.");
        }

        private SQLiteConnection conn;
        private string albumName = string.Empty;
        private string lastText = string.Empty;

        private bool appendQueue;

        private void RefreshList()
        {
            lvQueues.Items.Clear();
            tsbSave.Enabled = false;
            tsbRemove.Enabled = false;
            tsbCopyAllFlat.Enabled = false;
            lastText = string.Empty;
            bOK.Enabled = false;
            btAppendQueue.Enabled = false;
            tsbModifySavedQueue.Enabled = false;
            tsbExportQueue.Enabled = false;
            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                command.CommandText =
                    string.Format("SELECT COUNT(DISTINCT ID) FROM " + Environment.NewLine +
                                  "QUEUE_SNAPSHOT WHERE ALBUM_ID = (SELECT ID FROM ALBUM WHERE ALBUMNAME = '{0}') ", albumName.Replace("'", "''"));

                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    return;
                }

                command.CommandText =
                    string.Format("SELECT ID, SNAPSHOTNAME, MAX(SNAPSHOT_DATE) AS SNAPSHOT_DATE " + Environment.NewLine +
                                  "FROM QUEUE_SNAPSHOT WHERE ALBUM_ID = (SELECT ALBUM_ID FROM ALBUM WHERE ALBUMNAME = '{0}') " + Environment.NewLine +
                                  "GROUP BY ID, SNAPSHOTNAME " + Environment.NewLine +
                                  "ORDER BY MAX(SNAPSHOT_DATE) ", albumName.Replace("'", "''"));


                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListViewItem lvi = new ListViewItem(dr.GetString(1))
                        {
                            Tag = dr.GetInt32(0)
                        };
                        DateTime dt = DateTime.ParseExact(dr.GetString(2), "yyyy-MM-dd HH':'mm':'ss", CultureInfo.InvariantCulture);
                        lvi.SubItems.Add(dt.ToShortDateString() + " " + dt.ToShortTimeString());
                        lvQueues.Items.Add(lvi);
                    }
                }
            }
        }

        public static int Execute(string albumName, ref SQLiteConnection conn, out bool append)
        {

            FormSavedQueues frm = new FormSavedQueues
            {
                conn = conn,
                albumName = albumName
            };
            frm.RefreshList();
            if (frm.lvQueues.Items.Count == 0)
            {
                append = frm.appendQueue;
                return -1;
            }
            if (frm.ShowDialog() == DialogResult.OK)
            {
                append = frm.appendQueue;
                if (frm.lvQueues.SelectedIndices.Count > 0)
                {
                    return Convert.ToInt32(frm.lvQueues.Items[frm.lvQueues.SelectedIndices[0]].Tag);
                }
                return -1;
            }

            append = frm.appendQueue;
            return -1;
        }

        private void lvQueues_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            bOK.Enabled = lv.SelectedIndices.Count > 0;
            tsbRemove.Enabled = lv.SelectedIndices.Count > 0;
            tsbModifySavedQueue.Enabled = lv.SelectedIndices.Count > 0;
            tsbCopyAllFlat.Enabled = lv.SelectedIndices.Count > 0;
            tsbExportQueue.Enabled = lv.SelectedIndices.Count > 0;
            btAppendQueue.Enabled = lv.SelectedIndices.Count > 0;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvQueues.Items)
            {
                if (lvi.Name != @"MODIFIED")
                {
                    continue;
                }
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText =
                        string.Format("UPDATE QUEUE_SNAPSHOT SET SNAPSHOTNAME = '{0}' WHERE ID = {1} ", lvi.Text, lvi.Tag);
                    command.ExecuteNonQuery();
                    lvi.Name = string.Empty;
                }
            }
            tsbSave.Enabled = false;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void lvQueues_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != lastText)
            {
                tsbSave.Enabled = true;
                lvQueues.Items[e.Item].Name = @"MODIFIED";
            }
            lastText = string.Empty;
        }

        private void lvQueues_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            lastText = e.Label;
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                DBLangEngine.GetMessage("msgRemoveQueueSnapshot", "Are you sure you want to remove queue snapshot {0}?|A confirmation question if a queue snapshot can be removed when asked from the user", lvQueues.Items[lvQueues.SelectedIndices[0]].Text),
                DBLangEngine.GetMessage("msgConfirmation", "Confirm|Used in a dialog title to ask for a confirmation to do something"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "DELETE FROM QUEUE_SNAPSHOT WHERE ID = " + Convert.ToInt32(lvQueues.Items[lvQueues.SelectedIndices[0]].Tag) + " ";
                    command.ExecuteNonQuery();
                }
                RefreshList();
            }
        }

        private void tsbModifySavedQueue_Click(object sender, EventArgs e)
        {
            FormModifySavedQueue.Execute(ref conn, Convert.ToInt32(lvQueues.Items[lvQueues.SelectedIndices[0]].Tag));
        }

        private void tsbExportQueue_Click(object sender, EventArgs e)
        {
            if (sdExportQueue.ShowDialog() == DialogResult.OK)
            {
                Database.SaveQueueSnapshotToFile(conn, Convert.ToInt32(lvQueues.Items[lvQueues.SelectedIndices[0]].Tag), sdExportQueue.FileName);
            }
        }

        private void tsbImportQueue_Click(object sender, EventArgs e)
        {
            if (odExportQueue.ShowDialog() == DialogResult.OK)
            {                
                FormMain wnd = Application.OpenForms[0] as FormMain;

                string queueName = Database.GetQueueSnapshotName(odExportQueue.FileName);

                queueName = FormQueueSnapshotName.Execute(queueName, true);

                if (wnd != null && Database.RestoreQueueSnapshotFromFile(wnd.PlayList, conn, wnd.CurrentAlbum,
                        odExportQueue.FileName, queueName))
                {
                    RefreshList();
                }
            }
        }

        private void btAppendQueue_Click(object sender, EventArgs e)
        {
            appendQueue = true;
            DialogResult = DialogResult.OK;
        }

        private void TsbCopyAllFlat_Click(object sender, EventArgs e)
        {
            if (fbdDirectory.ShowDialog() == DialogResult.OK)
            {
                QueueUtilities.CopyQueueFiles(Convert.ToInt32(lvQueues.Items[lvQueues.SelectedIndices[0]].Tag),
                    fbdDirectory.SelectedPath, conn);
            }
        }
    }
}
