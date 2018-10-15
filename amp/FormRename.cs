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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp
{
    public partial class FormRename : DBLangEngineWinforms
    {
        public FormRename()
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

        string lastName = string.Empty;
        private void tbNewSongName_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = tbNewSongName.Text.Length > 0 && tbNewSongName.Text != lastName;
        }

        public static string Execute(MusicFile mf)
        {
            FormRename rename = new FormRename();
            rename.tbNewSongName.Text = mf.ToString(false);
            rename.lastName = mf.ToString(false);
            if (rename.ShowDialog() == DialogResult.OK)
            {
                return rename.tbNewSongName.Text;
            }
            else
            {
                return string.Empty;
            }
        }

        private void frmRename_Shown(object sender, EventArgs e)
        {
            bOK.Enabled = false;
            tbNewSongName.SelectAll();
            tbNewSongName.Focus();
        }
    }
}
