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
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp.FormsUtility.QueueHandling
{
    public partial class FormQueueSnapshotName : DBLangEngineWinforms
    {
        public FormQueueSnapshotName()
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

        public static string Execute(string albumName, bool overrideName = false)
        {
            if (albumName == "tmp")
            {
                MessageBox.Show(
                    DBLangEngine.GetStatMessage("msgTmpAlbumError", "The current album is temporary album.{0}Please create an album with a name before continuing.|Album is a temporary one i.e. you clicked a music file and the program started. Can't save a anything against a temporary album.", Environment.NewLine),
                    DBLangEngine.GetStatMessage("msgError", "Error|A common error that should be defined in another message"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            FormQueueSnapshotName queueName = new FormQueueSnapshotName();
            string namePart = DBLangEngine.GetStatMessage("msgQueue", "Queue|As in a queue snapshot");

            if (overrideName)
            {
                queueName.tbQueueName.Text = albumName;
            }
            else
            {
                queueName.tbQueueName.Text = namePart + @": " + albumName + @" - " + DateTime.Now.ToLongDateString() +
                                             @" (" + DateTime.Now.ToShortTimeString() + @")";
            }

            if (queueName.ShowDialog() == DialogResult.OK)
            {
                return queueName.tbQueueName.Text;
            }

            if (overrideName)
            {
                return albumName;
            }

            return string.Empty;
        }

        private void tbQueueName_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = tbQueueName.Text.Trim().Length > 0;
        }

        private void FormQueueSnapshotName_Shown(object sender, EventArgs e)
        {
            tbQueueName.SelectAll();
            tbQueueName.Focus();
        }
    }
}
