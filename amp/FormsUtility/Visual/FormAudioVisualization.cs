using System;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace amp.FormsUtility.Visual
{
    /// <summary>
    /// A form to display audio visualization.
    /// Implements the <see cref="VPKSoft.LangLib.DBLangEngineWinforms" />
    /// </summary>
    /// <seealso cref="VPKSoft.LangLib.DBLangEngineWinforms" />
    public partial class FormAudioVisualization : DBLangEngineWinforms
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAudioVisualization"/> class.
        /// </summary>
        public FormAudioVisualization()
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

        /// <summary>
        /// The main form of the application (for to activate the main form in case this form was activated).
        /// </summary>
        private static FormMain activateWindow;

        /// <summary>
        /// Gets a singleton instance of this form.
        /// </summary>
        public static FormAudioVisualization ThisInstance;

        // a flag indicating whether the form was shown the first time..
        private static bool firstShow = true;

        /// <summary>
        /// Repositions the <see cref="ThisInstance"/> of this form.
        /// </summary>
        /// <param name="formMain">The main form's instance.</param>
        /// <param name="top">The top position for the instance of this form.</param>
        public static void Reposition(FormMain formMain, int top)
        {
            if (ThisInstance != null)
            {
                activateWindow = formMain;
                ThisInstance.Left = formMain.Left + formMain.Width;
                ThisInstance.Top = top;
            }
        }

        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="useBarGraph">if set to <c>true</c> the bar graph is used.</param>
        /// <param name="antiAliasing">if set to <c>true</c> anti aliasing is used in case of line graph.</param>
        /// <param name="useGradient">if set to <c>true</c> gradient bars are used in case of a bar graph.</param>
        /// <param name="formMain">The main form's instance.</param>
        /// <param name="top">The top position for the instance of this form.</param>
        public static void ShowForm(FormMain formMain, int top, bool useBarGraph, bool antiAliasing, bool useGradient)
        {
            if (ThisInstance == null)
            {
                ThisInstance = new FormAudioVisualization {Owner = formMain, Visible = true};
            }
            if (firstShow && ThisInstance.Visible)
            {
                formMain.BringToFront();
            }

            if (useBarGraph)
            {
                ThisInstance.avBars.Visible = true;
                ThisInstance.avBars.Start();
                ThisInstance.avLine.Visible = false;
                ThisInstance.avLine.Stop();
                ThisInstance.avBars.Dock = DockStyle.Fill;
                ThisInstance.avLine.Dock = DockStyle.None;
            }
            else
            {
                ThisInstance.avLine.Visible = true;
                ThisInstance.avLine.Start();
                ThisInstance.avBars.Visible = false;
                ThisInstance.avBars.Stop();
                ThisInstance.avLine.Dock = DockStyle.Fill;
                ThisInstance.avBars.Dock = DockStyle.None;
            }

            ThisInstance.avBars.DrawWithGradient = useGradient;
            ThisInstance.avLine.UseAntiAliasing = antiAliasing;

            Reposition(formMain, top);
        }

        // re-activate the main form in case this form was activated..
        private void FormAudioVisualization_Activated(object sender, EventArgs e)
        {
            activateWindow?.Activate();
        }
    }
}
