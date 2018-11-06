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
    public partial class FormRandomizePriority : DBLangEngineWinforms
    {
        public FormRandomizePriority()
        {
            InitializeComponent();

            DBLangEngine.DBName = "lang.sqlite";

            if (VPKSoft.LangLib.Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", VPKSoft.LangLib.Utils.ShouldLocalize(), false);
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

        private bool suspendCheckedChanged = false;

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
            else
            {
                return (double)trackBar.Value / 10;
            }
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
            lbTolerancePercentageValue.Text = $"{tbTolerancePercentage.Value / 10}";
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
