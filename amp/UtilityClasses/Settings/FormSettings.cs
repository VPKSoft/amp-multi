﻿#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using amp.DataMigrate.GUI;
using amp.FormsUtility.Random;
using ReaLTaiizor.Helper;
using VPKSoft.ErrorLogger;
using VPKSoft.LangLib;
using VU = VPKSoft.Utils;

namespace amp.UtilityClasses.Settings
{
    /// <summary>
    /// The main settings form for the software.
    /// </summary>
    public partial class FormSettings : DBLangEngineWinforms
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:amp.UtilityClasses.Settings.FormSettings"/> class.
        /// </summary>
        public FormSettings()
        {
            InitializeComponent();

            // ReSharper disable once StringLiteralTypo
            DBLangEngine.DBName = "lang.sqlite";
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitializeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitializeLanguage("amp.Messages");
            btAssignRemoteControlURI.Image = VU.SysIcons.GetSystemIconBitmap(VU.SysIcons.SystemIconType.Shield, new Size(16, 16));
        }

        private void nudQuietHourPercentage_ValueChanged(object sender, EventArgs e)
        {
            rbDecreaseVolumeQuietHours.Checked = true;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Program.Settings.QuietHours = cbQuietHours.Checked;
            Program.Settings.QuietHoursFrom = dtpFrom.Value.ToString("HH':'mm");
            Program.Settings.QuietHoursTo = dtpTo.Value.ToString("HH':'mm");

            Program.Settings.QuietHoursVolPercentage = (int)nudQuietHourPercentage.Value;
            Program.Settings.QuietHoursPause = rbPauseQuiet.Checked;
            Program.Settings.RemoteControlApiWcfAddress = tbRemoteControlURI.Text;

            Program.Settings.LatencyMs = (int)nudLatency.Value;
            Program.Settings.RemoteControlApiWcf = cbRemoteControlEnabled.Checked;
            Program.Settings.AudioVisualizationStyle = int.Parse(gbAudioVisualizationStyle.Tag.ToString());

            Program.Settings.AudioVisualizationVisualPercentage = (int) nudAudioVisualizationSize.Value;
            Program.Settings.AudioVisualizationCombineChannels = cbAudioVisualizationCombineChannels.Checked;
            Program.Settings.BalancedBars = cbBalancedBars.Checked;
            Program.Settings.BarAmount = (int) nudBarAmount.Value;

            // the user decides if an internet request is allowed..
            Program.Settings.AutoCheckUpdates = cbCheckUpdatesStartup.Checked;

            Program.Settings.Culture = (CultureInfo)cmbSelectLanguageValue.SelectedItem;
            Program.Settings.StackRandomPercentage = tbStackQueueRandom.Value;

            if (cbMemoryBuffering.Checked)
            {
                Program.Settings.LoadEntireFileSizeLimit = (int) nudMemoryBuffering.Value;
            }
            else
            {
                Program.Settings.LoadEntireFileSizeLimit = -1;
            }

            Program.Settings.DisplayVolumeAndPoints = cbDisplayVolumeAndPoints.Checked;

            SaveSettings();
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Calculates the quiet hour start and end based on the current time.
        /// </summary>
        /// <param name="hourFrom">The starting hour formatted as HH:mm.</param>
        /// <param name="hourTo">The ending hour formatted as HH:mm.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> containing the starting and ending <see cref="DateTime"/> of a quiet hour.</returns>
        public static KeyValuePair<DateTime, DateTime> CalculateQuietHour(string hourFrom, string hourTo)
        {
            return CalculateQuietHour(hourFrom, hourTo, DateTime.Now);
        }

        /// <summary>
        /// Calculates the quiet hour start and end based on the given time.
        /// </summary>
        /// <param name="hourFrom">The starting hour formatted as HH:mm.</param>
        /// <param name="hourTo">The ending hour formatted as HH:mm.</param>
        /// <param name="compareDate">A date and time to compare the <paramref name="hourFrom"/> and <paramref name="hourTo"/> values.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> containing the starting and ending <see cref="DateTime"/> of a quiet hour.</returns>
        public static KeyValuePair<DateTime, DateTime> CalculateQuietHour(string hourFrom, string hourTo, DateTime compareDate)
        {
            DateTime dt1 = DateTime.ParseExact(Convert.ToString(hourFrom), "HH':'mm", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(Convert.ToString(hourTo), "HH':'mm", CultureInfo.InvariantCulture);

            dt1 = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, dt1.Hour, dt1.Minute, 0);
            dt2 = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, dt2.Hour, dt2.Minute, 0);

            if (dt2 < dt1)
            {
                dt2 = dt2.AddDays(1);
            }

            return new KeyValuePair<DateTime, DateTime>(dt1, dt2);
        }

        /// <summary>
        /// Gets a value indicating based on the settings whether the current time is a quiet hour.
        /// </summary>
        /// <returns>True if the current time is a quiet hour; otherwise false.</returns>
        public static bool IsQuietHour()
        {
            // if the setting is not enabled the return false..
            if (!Program.Settings.QuietHours)
            {
                return false;
            }

            KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(Program.Settings.QuietHoursFrom, Program.Settings.QuietHoursTo);
            bool result = (DateTime.Now >= span.Key && DateTime.Now < span.Value);
            return result;
        }

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Saves the settings to a .vnml file.
        /// </summary>
        private void SaveSettings()
        {
            Program.Settings.SaveToFile();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            cbQuietHours.Checked = Program.Settings.QuietHours;

            dtpFrom.Value = DateTime.ParseExact(Program.Settings.QuietHoursFrom, "HH':'mm", CultureInfo.InvariantCulture);
            dtpTo.Value = DateTime.ParseExact(Program.Settings.QuietHoursTo, "HH':'mm", CultureInfo.InvariantCulture);
            nudQuietHourPercentage.Value = (decimal) Program.Settings.QuietHoursVolPercentage;
            rbPauseQuiet.Checked = true;
            rbPauseQuiet.Checked = Program.Settings.QuietHoursPause;
            rbDecreaseVolumeQuietHours.Checked = !rbPauseQuiet.Checked;
            tbRemoteControlURI.Text = Program.Settings.RemoteControlApiWcfAddress;
            nudLatency.Value = Program.Settings.LatencyMs;
            cbRemoteControlEnabled.Checked = Program.Settings.RemoteControlApiWcf;

            nudBarAmount.Value = Program.Settings.BarAmount < 20 ? 92 : Program.Settings.BarAmount;

            tbStackQueueRandom.Value = Program.Settings.StackRandomPercentage;

            switch (Program.Settings.AudioVisualizationStyle)
            {
                case 0: rbAudioVisualizationOff.Checked = true; break;
                case 1: rbAudioVisualizationBars.Checked = true; break;
                case 2: rbAudioVisualizationLines.Checked = true; break;
                default: rbAudioVisualizationOff.Checked = true; break;
            }

            nudAudioVisualizationSize.Value = Program.Settings.AudioVisualizationVisualPercentage;

            cbAudioVisualizationCombineChannels.Checked = Program.Settings.AudioVisualizationCombineChannels;
            cbBalancedBars.Checked = Program.Settings.BalancedBars;

            // the user decides if an internet request is allowed..
            cbCheckUpdatesStartup.Checked = Program.Settings.AutoCheckUpdates;

            cbMemoryBuffering.Checked = Program.Settings.LoadEntireFileSizeLimit > 0;
            if (Program.Settings.LoadEntireFileSizeLimit > 0)
            {
                nudMemoryBuffering.Value = Program.Settings.LoadEntireFileSizeLimit;
                nudMemoryBuffering.Enabled = true;
            }
            else
            {
                nudMemoryBuffering.Value = 100;
                nudMemoryBuffering.Enabled = false;
            }

            List<CultureInfo> cultures = DBLangEngine.GetLocalizedCultures();

            if (cultures != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                cmbSelectLanguageValue.Items.AddRange(cultures.ToArray());
            }
            cmbSelectLanguageValue.SelectedItem = Program.Settings.Culture;

            bool? netShResult = VU.NetSH.IsNetShUrlReserved(lbRemoteControlURIVValue.Text);
            if (netShResult != null)
            {
                btAssignRemoteControlURI.Enabled = netShResult == false;
            }

            cbDisplayVolumeAndPoints.Checked = Program.Settings.DisplayVolumeAndPoints;
        }

        private void tbRemoteControlURI_TextChanged(object sender, EventArgs e)
        {
            if (!VU.UriUrlUtils.ValidHttpUrl(tbRemoteControlURI.Text, true))
            {
                tbRemoteControlURI.BackColor = Color.Red;
            }
            else
            {

                tbRemoteControlURI.BackColor = SystemColors.Window;
                lbRemoteControlURIVValue.Text = VU.UriUrlUtils.MakeWildCardUrl(tbRemoteControlURI.Text, true);
            }
        }

        private void btAssignRemoteControlURI_Click(object sender, EventArgs e)
        {
            // ReSharper disable once IdentifierTypo
            bool? netshRet = VU.NetSH.IsNetShUrlReserved(lbRemoteControlURIVValue.Text);
            if (netshRet != null && netshRet == false)
            {
                VU.NetSH.ReserveNetShUrl(
                    lbRemoteControlURIVValue.Text, 
                    VU.BuiltInWindowsAccountsLocalize.GetUserNameBySID(VU.BuiltInWindowsAccountsLocalize.Everyone));
            }
        }

        private void btAlbumNaming_Click(object sender, EventArgs e)
        {
            using (var formAlbumNaming = new FormAlbumNaming())
            {
                formAlbumNaming.ShowDialog();
            }
        }

        private void btnTestQuietHour_Click(object sender, EventArgs e)
        {
            DateTime dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dt2 = dt1.AddDays(2);

            tbTestQuietHour.Clear();

            while (dt1 < dt2)
            {
                KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(Program.Settings.QuietHoursFrom, Program.Settings.QuietHoursTo, dtCompare);
                dt1 = dt1.AddMinutes(1);
                bool isQuietHour =(dt1 >= span.Key && dt1 < span.Value);
                tbTestQuietHour.Text += isQuietHour + @": " + dt1.ToString("HH':'mm dd'.'MM'.'yyyy") + Environment.NewLine;
            }
        }

        private void btnModifiedRandomization_Click(object sender, EventArgs e)
        {
            using (var formRandomizePriority = new FormRandomizePriority())
            {
                formRandomizePriority.ShowDialog();
            }
        }

        private void MnuLocalization_Click(object sender, EventArgs e)
        {
            try
            {
                string args = "--localize=\"" +
                              Path.Combine(
                                  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                  "amp#",
                                  // ReSharper disable once StringLiteralTypo
                                  "lang.sqlite") + "\"";

                Process.Start(Application.ExecutablePath, args);
            }
            catch (Exception ex)
            {
                // log the exception..
                ExceptionLogger.LogError(ex, "Localization");
            }
        }

        private void MnuDumpLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Application.ExecutablePath, "--dblang");
            }
            catch (Exception ex)
            {
                // log the exception..
                ExceptionLogger.LogError(ex, "Localization dump");
            }
        }

        private void RbAudioVisualization_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = (RadioButton) sender;
            gbAudioVisualizationStyle.Tag = radioButton.Tag;
        }

        private void MnuDatabaseMigration_Click(object sender, EventArgs e)
        {
            FormDatabaseMigrate.ShowDialog(FormMain.Connection);

            if (FormMain.RestartRequired)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void cbMemoryBuffering_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox) sender;
            nudMemoryBuffering.Value = 100;
            nudMemoryBuffering.Enabled = checkBox.Checked;
        }

        private void tbStackQueueRandom_ValueChanged(object sender, EventArgs e)
        {
            lbStackQueueRandomValue.Text = DBLangEngine.GetMessage("msgPercentageNumber",
                "{0} %|A message describing a percentage number value", tbStackQueueRandom.Value);
        }

        private void mnuThemeSettings_Click(object sender, EventArgs e)
        {
            new FormThemeSettings(new ThemeSettings(new CrownHelper.DarkTheme())).ShowDialog();
        }
    }
}
