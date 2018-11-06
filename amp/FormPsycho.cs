#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp
{
    public partial class FormPsycho : DBLangEngineWinforms
    {
        private static FormPsycho form = null;

        public FormPsycho()
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
