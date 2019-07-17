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
using amp.UtilityClasses;
using VPKSoft.LangLib;

namespace amp.FormsUtility.Songs
{
    public partial class FormRename : DBLangEngineWinforms
    {
        public FormRename()
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

        string lastName = string.Empty;
        private void tbNewSongName_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = tbNewSongName.Text.Length > 0 && tbNewSongName.Text != lastName;
        }

        public static string Execute(MusicFile mf)
        {
            FormRename rename = new FormRename
            {
                tbNewSongName = {Text = mf.ToString(false)}, lastName = mf.ToString(false)
            };
            if (rename.ShowDialog() == DialogResult.OK)
            {
                return rename.tbNewSongName.Text;
            }

            return string.Empty;
        }

        private void frmRename_Shown(object sender, EventArgs e)
        {
            bOK.Enabled = false;
            tbNewSongName.SelectAll();
            tbNewSongName.Focus();
        }
    }
}
