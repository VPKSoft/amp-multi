﻿#region License
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

using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp.FormsUtility
{
    public partial class FormPsycho : DBLangEngineWinforms
    {
        private static FormPsycho form;

        public FormPsycho()
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

        public static void Execute(Form frm)
        {
            if (form != null)
            {
                return;
            }
            form = new FormPsycho();
            form.Left = frm.Left + (frm.Width - form.Width) / 2;
            form.Top = frm.Top + (frm.Height - form.Height) / 2;
            form.Show();
            form.Refresh();
        }

        public static void SetStatusText(string text)
        {
            if (form != null)
            {
                form.lbStatus.Text = text;
                form.lbStatus.Refresh();
            }            
        }

        public static void UnExecute()
        {
            form.Close();
            form = null;
        }
    }
}
