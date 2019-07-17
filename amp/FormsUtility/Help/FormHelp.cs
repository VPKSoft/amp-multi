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
using VPKSoft.PosLib;

namespace amp.FormsUtility.Help
{
    public partial class FormHelp : DBLangEngineWinforms
    {
        public FormHelp()
        {
            // Add this form to be positioned..
            PositionForms.Add(this);

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

        private static FormHelp thisSingleton;
        private static bool allowDisposal;

        public static void ShowSingleton()
        {
            if (thisSingleton == null)
            {
                thisSingleton = new FormHelp();
                thisSingleton.Show();
            }
            else
            {
                if (!thisSingleton.Visible)
                {
                    thisSingleton.Show();
                }
                thisSingleton.BringToFront();
            }
        }

        private void FormHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowDisposal)
            {
                e.Cancel = true; // prevent the disposing of the form..
                Hide(); // ..so do hide.. :-)
            }
        }

        public static void DisposeSingleton()
        {
            allowDisposal = true;
            if (thisSingleton != null)
            {
                thisSingleton.Close();
                thisSingleton = null;
            }
        }
    }
}
