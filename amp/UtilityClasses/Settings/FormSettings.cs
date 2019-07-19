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
    public partial class FormSettings : DBLangEngineWinforms
    {
        public FormSettings()
        {
            InitializeComponent();

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

        public static void SetMainWindowSettings()
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            MainWindow.QuietHours = Convert.ToBoolean(vnml["quietHour", "enabled", false]); // this is gotten from the settings

            MainWindow.QuietHoursFrom = vnml["quietHour", "start", "23:00"].ToString();
            MainWindow.QuietHoursTo = vnml["quietHour", "end", "08:00"].ToString();
            MainWindow.QuietHoursPause = Convert.ToBoolean(vnml["quietHour", "pause", true]);
            MainWindow.QuietHoursVolPercentage = (100.0 - Convert.ToDouble(vnml["quietHour", "percentage", 70])) / 100.0;
            MainWindow.LatencyMs = Convert.ToInt32(vnml["latency", "value", 300]);
            MainWindow.RemoteControlApiWcf = Convert.ToBoolean(vnml["remote", "enabled", false]);
            MainWindow.RemoteControlApiWcfAddress = vnml["remote", "uri", "http://localhost:11316/ampRemote/"].ToString();
        }

        public static KeyValuePair<DateTime, DateTime> CalculateQuietHour(string hourFrom, string hourTo)
        {
            return CalculateQuietHour(hourFrom, hourTo, DateTime.Now);
        }

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

        public static bool IsQuietHour()
        {
            if (!MainWindow.QuietHours)
            {
                return false;
            }

            KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(MainWindow.QuietHoursFrom, MainWindow.QuietHoursTo);
            bool retval = (DateTime.Now >= span.Key && DateTime.Now < span.Value);
            return retval;
        }

        private void SaveSettings()
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");
            vnml["quietHour", "enabled"] = cbQuietHours.Checked;
            vnml["quietHour", "start"] = dtpFrom.Value.ToString("HH':'mm");
            vnml["quietHour", "end"] = dtpTo.Value.ToString("HH':'mm");
            vnml["quietHour", "percentage"] = (int)nudQuietHourPercentage.Value;
            vnml["quietHour", "pause"] = rbPauseQuiet.Checked;
            vnml["remote", "uri"] = tbRemoteControlURI.Text;
            vnml["latency", "value"] = (int)nudLatency.Value;
            vnml["remote", "enabled"] = cbRemoteControlEnabled.Checked;

            vnml.Save(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            Settings.Culture = (CultureInfo)cmbSelectLanguageValue.SelectedItem;

            SetMainWindowSettings();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
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

            List<CultureInfo> cultures = DBLangEngine.GetLocalizedCultures();

            if (cultures != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                cmbSelectLanguageValue.Items.AddRange(cultures.ToArray());
            }
            cmbSelectLanguageValue.SelectedItem = Settings.Culture;

            bool? netshRet = VU.NetSH.IsNetShUrlReserved(lbRemoteControlURIVValue.Text);
            if (netshRet != null)
            {
                btAssignRemoteControlURI.Enabled = netshRet == false;
            }
        }

        private void tbRemoteControllURI_TextChanged(object sender, EventArgs e)
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
                KeyValuePair<DateTime, DateTime> span = CalculateQuietHour(MainWindow.QuietHoursFrom, MainWindow.QuietHoursTo, dtCompare);
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
