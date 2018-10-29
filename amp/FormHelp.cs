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
using VPKSoft.PosLib;

namespace amp
{
    public partial class FormHelp : DBLangEngineWinforms
    {
        public FormHelp()
        {
            // Add this form to be positioned..
            PositionForms.Add(this, PositionCore.SizeChangeMode.MoveTopLeft);

            InitializeComponent();

            DBLangEngine.DBName = "lang.sqlite";
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("amp.Messages");
        }

        private static FormHelp thisSingleton = null;
        private static bool allowDisposal = false;

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
