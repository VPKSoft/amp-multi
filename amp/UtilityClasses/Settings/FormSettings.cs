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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using amp.FormsUtility;
using amp.FormsUtility.Random;
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
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("amp.Messages");
            btAssignRemoteControlURI.Image = VU.SysIcons.GetSystemIconBitmap(VU.SysIcons.SystemIconType.Shield, new Size(16, 16));
        }

        private void nudQuietHourPercentage_ValueChanged(object sender, EventArgs e)
        {
            rbDecreaseVolumeQuietHours.Checked = true;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Get the main form settings.
        /// </summary>
        public static void SetMainWindowSettings()
        {
            // ReSharper disable once IdentifierTypo
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            // ReSharper disable once StringLiteralTypo
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            FormMain.QuietHours = Convert.ToBoolean(vnml["quietHour", "enabled", false]); // this is gotten from the settings

            FormMain.QuietHoursFrom = vnml["quietHour", "start", "23:00"].ToString();
            FormMain.QuietHoursTo = vnml["quietHour", "end", "08:00"].ToString();
            FormMain.QuietHoursPause = Convert.ToBoolean(vnml["quietHour", "pause", true]);
            FormMain.QuietHoursVolPercentage = (100.0 - Convert.ToDouble(vnml["quietHour", "percentage", 70])) / 100.0;
            FormMain.LatencyMs = Convert.ToInt32(vnml["latency", "value", 300]);
            FormMain.RemoteControlApiWcf = Convert.ToBoolean(vnml["remote", "enabled", false]);
            FormMain.AutoCheckUpdates = Convert.ToBoolean(vnml["autoUpdate", "enabled", false]);
            FormMain.RemoteControlApiWcfAddress = vnml["remote", "uri", "http://localhost:11316/ampRemote/"].ToString();
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
            if (!FormMain.QuietHours)
            {
                return false;
            }

            KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(FormMain.QuietHoursFrom, FormMain.QuietHoursTo);
            bool result = (DateTime.Now >= span.Key && DateTime.Now < span.Value);
            return result;
        }

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Saves the settings to a .vnml file.
        /// </summary>
        private void SaveSettings()
        {
            // ReSharper disable once IdentifierTypo
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            // ReSharper disable once StringLiteralTypo
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");
            vnml["quietHour", "enabled"] = cbQuietHours.Checked;
            vnml["quietHour", "start"] = dtpFrom.Value.ToString("HH':'mm");
            vnml["quietHour", "end"] = dtpTo.Value.ToString("HH':'mm");
            vnml["quietHour", "percentage"] = (int)nudQuietHourPercentage.Value;
            vnml["quietHour", "pause"] = rbPauseQuiet.Checked;
            vnml["remote", "uri"] = tbRemoteControlURI.Text;
            vnml["latency", "value"] = (int)nudLatency.Value;
            vnml["remote", "enabled"] = cbRemoteControlEnabled.Checked;

            // the user decides if an internet request is allowed..
            vnml["autoUpdate", "enabled"] = cbCheckUpdatesStartup.Checked;

            // ReSharper disable once StringLiteralTypo
            vnml.Save(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            Settings.Culture = (CultureInfo)cmbSelectLanguageValue.SelectedItem;

            SetMainWindowSettings();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            // ReSharper disable once IdentifierTypo
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            // ReSharper disable once StringLiteralTypo
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            cbQuietHours.Checked = Convert.ToBoolean(vnml["quietHour", "enabled", false]);

            dtpFrom.Value = DateTime.ParseExact(Convert.ToString(vnml["quietHour", "start", "23:00"]), "HH':'mm", CultureInfo.InvariantCulture);
            dtpTo.Value = DateTime.ParseExact(Convert.ToString(vnml["quietHour", "end", "08:00"]), "HH':'mm", CultureInfo.InvariantCulture);
            nudQuietHourPercentage.Value = Convert.ToInt32(vnml["quietHour", "percentage", 70]);
            rbPauseQuiet.Checked = true;
            rbPauseQuiet.Checked = Convert.ToBoolean(vnml["quietHour", "pause", true]);
            rbDecreaseVolumeQuietHours.Checked = !rbPauseQuiet.Checked;
            tbRemoteControlURI.Text = vnml["remote", "uri", "http://localhost:11316/ampRemote/"].ToString();
            nudLatency.Value = Convert.ToInt32(vnml["latency", "value", 300]);
            cbRemoteControlEnabled.Checked = Convert.ToBoolean(vnml["remote", "enabled", false]);

            // the user decides if an internet request is allowed..
            cbCheckUpdatesStartup.Checked = Convert.ToBoolean(vnml["autoUpdate", "enabled", false]);


            List<CultureInfo> cultures = DBLangEngine.GetLocalizedCultures();

            if (cultures != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                cmbSelectLanguageValue.Items.AddRange(cultures.ToArray());
            }
            cmbSelectLanguageValue.SelectedItem = Settings.Culture;

            bool? netShResult = VU.NetSH.IsNetShUrlReserved(lbRemoteControlURIVValue.Text);
            if (netShResult != null)
            {
                btAssignRemoteControlURI.Enabled = netShResult == false;
            }
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
                // ReSharper disable once RedundantAssignment
                netshRet = VU.NetSH.ReserveNetShUrl(
                    lbRemoteControlURIVValue.Text, 
                    VU.BuiltInWindowsAccountsLocalize.GetUserNameBySID(VU.BuiltInWindowsAccountsLocalize.Everyone));
            }
        }

        private void btAlbumNaming_Click(object sender, EventArgs e)
        {
            new FormAlbumNaming().ShowDialog();
        }

        private void btnTestQuietHour_Click(object sender, EventArgs e)
        {
            DateTime dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dt2 = dt1.AddDays(2);

            tbTestQuietHour.Clear();

            while (dt1 < dt2)
            {
                KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(FormMain.QuietHoursFrom, FormMain.QuietHoursTo, dtCompare);
                dt1 = dt1.AddMinutes(1);
                bool isQuietHour =(dt1 >= span.Key && dt1 < span.Value);
                tbTestQuietHour.Text += isQuietHour + @": " + dt1.ToString("HH':'mm dd'.'MM'.'yyyy") + Environment.NewLine;
            }
        }

        private void btnModifiedRandomization_Click(object sender, EventArgs e)
        {
            new FormRandomizePriority().ShowDialog();
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
    }
}
