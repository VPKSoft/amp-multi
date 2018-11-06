#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp
{
    public partial class FormQueueSnapshotName : DBLangEngineWinforms
    {
        public FormQueueSnapshotName()
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
                queueName.tbQueueName.Text = namePart + ": " + albumName + " - " + DateTime.Now.ToLongDateString() + " (" + DateTime.Now.ToShortTimeString() + ")";
            }

            if (queueName.ShowDialog() == DialogResult.OK)
            {
                return queueName.tbQueueName.Text;
            }
            else
            {
                if (overrideName)
                {
                    return albumName;
                }
                else
                {
                    return string.Empty;
                }
            }
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
