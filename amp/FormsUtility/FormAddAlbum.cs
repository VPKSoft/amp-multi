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

namespace amp.FormsUtility
{
    public partial class FormAddAlbum : DBLangEngineWinforms
    {
        public FormAddAlbum()
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

        public static string Execute(string name = "")
        {
            FormAddAlbum form = new FormAddAlbum();
            form.tbAlbumName.Text = name;
            if (form.ShowDialog() == DialogResult.OK)
            {
                return form.tbAlbumName.Text;
            }

            return string.Empty;
        }

        private void tbAlbumName_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = tbAlbumName.Text.Length > 0;
        }
    }
}
