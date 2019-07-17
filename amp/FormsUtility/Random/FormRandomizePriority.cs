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
using amp.UtilityClasses.Settings;
using VPKSoft.LangLib;

namespace amp.FormsUtility.Random
{
    public partial class FormRandomizePriority : DBLangEngineWinforms
    {
        public FormRandomizePriority()
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

            suspendCheckedChanged = true;
            cbModifiedRandomizationEnabled.Checked = Settings.BiasedRandom;
            SetBiasedRandomValue(tbRating, cbRatingEnabled, Settings.BiasedRating, Settings.BiasedRatingEnabled);
            SetBiasedRandomValue(tbPlayedCount, cbPlayedCountEnabled, Settings.BiasedPlayedCount, Settings.BiasedPlayedCountEnabled);
            SetBiasedRandomValue(tbRandomizedCount, cbRandomizedCountEnabled, Settings.BiasedRandomizedCount, Settings.BiasedRandomizedCountEnabled);
            SetBiasedRandomValue(tbSkippedCount, cbSkippedCountEnabled, Settings.BiasedSkippedCount, Settings.BiasedSkippedCountEnabled);
            tbTolerancePercentage.Value = Settings.Tolerance < 0 ? 10 : (int)Settings.Tolerance * 10;
            suspendCheckedChanged = false;
        }

        private readonly bool suspendCheckedChanged;

        private void SetBiasedRandomValue(TrackBar trackBar, CheckBox checkBox, double biasedRating, bool biasedRatingEnabled)
        {
            trackBar.Value = (biasedRating >= 0) ? (int)biasedRating * 10 : 0;
            checkBox.Checked = biasedRatingEnabled;
        }

        private double GetBiasedRandomValue(TrackBar trackBar, CheckBox checkBox, out bool enabled)
        {
            enabled = checkBox.Checked;
            if (!checkBox.Checked)
            {
                return -1;
            }

            return (double)trackBar.Value / 10;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Settings.BiasedRandom = cbModifiedRandomizationEnabled.Checked;

            Settings.BiasedRating = GetBiasedRandomValue(tbRating, cbRatingEnabled, out bool enabled);
            Settings.BiasedRatingEnabled = enabled;

            Settings.BiasedPlayedCount = GetBiasedRandomValue(tbPlayedCount, cbPlayedCountEnabled, out enabled);
            Settings.BiasedPlayedCountEnabled = enabled;

            Settings.BiasedRandomizedCount = GetBiasedRandomValue(tbRandomizedCount, cbRandomizedCountEnabled, out enabled);
            Settings.BiasedRandomizedCountEnabled = enabled;

            Settings.BiasedSkippedCount = GetBiasedRandomValue(tbSkippedCount, cbSkippedCountEnabled, out enabled);
            Settings.BiasedSkippedCountEnabled = enabled;

            Settings.Tolerance = (double)tbTolerancePercentage.Value / 10;

            DialogResult = DialogResult.OK;
        }

        private void btDefault_Click(object sender, EventArgs e)
        {
            tbRating.Value = 500;
            tbPlayedCount.Value = 0;
            tbRandomizedCount.Value = 0;
            tbSkippedCount.Value = 0;
            cbRatingEnabled.Checked = true;
            cbPlayedCountEnabled.Checked = false;
            cbRandomizedCountEnabled.Checked = false;
            cbSkippedCountEnabled.Checked = false;
            tbTolerancePercentage.Value = 100;
            cbModifiedRandomizationEnabled.Checked = false;
        }

        private void tbTolerancePercentage_ValueChanged(object sender, EventArgs e)
        {
            lbTolerancePercentageValue.Text = $@"{tbTolerancePercentage.Value / 10}";
        }

        private void cbCommon_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendCheckedChanged)
            {
                return;
            }

            cbModifiedRandomizationEnabled.Checked =
                cbRatingEnabled.Checked |
                cbPlayedCountEnabled.Checked |
                cbRandomizedCountEnabled.Checked |
                cbSkippedCountEnabled.Checked;
        }
    }
}
