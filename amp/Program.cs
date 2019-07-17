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
using System.IO;
using System.Windows.Forms;
using amp.FormsUtility;
using amp.FormsUtility.Help;
using amp.FormsUtility.QueueHandling;
using amp.FormsUtility.Random;
using amp.FormsUtility.Songs;
using amp.UtilityClasses.Settings;
using VPKSoft.ErrorLogger;
using VPKSoft.LangLib;
using VPKSoft.PosLib;
using VPKSoft.Utils;
using Utils = VPKSoft.LangLib.Utils;
// (C): http://www.vpksoft.net/, GNU Lesser General Public License Version 3

namespace amp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static List<string> Arguments = new List<string>();
        public static DateTime FirstInstanceCall = DateTime.Now;

        static public string AppDataDir
        {
            get
            {
                MakeDataDir();
                return Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\" + Application.ProductName + @"\";
            }
        }

        public static string AppInstallDir
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static void MakeDataDir()
        {
            string dir = Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\" + Application.ProductName + @"\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }


        [STAThread]
        static void Main(string[] args)
        {
            Process localizeProcess = Utils.CreateDBLocalizeProcess(Paths.AppInstallDir);
            // localizeProcess..

            if (localizeProcess != null)
            {
                localizeProcess.Start();
                return;
            }


            ExceptionLogger.Bind(); // bind before any visual objects are created


            if (Settings.DbUpdateRequiredLevel < 1)
            {
                if (AppRunning.CheckIfRunning("VPKSoft.amp.DBUpdate.sharp#"))
                {
                    ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                    return;
                }
                DBLangEngine.UseCulture = Settings.Culture; // set the localization value..
                Application.Run(new FormDatabaseUpdatingProgress());
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                Settings.DbUpdateRequiredLevel = 1;
                Process.Start(new ProcessStartInfo(Path.Combine(Paths.AppInstallDir, "amp.exe"))); // the database is updated..
                return;
            }

            foreach (string arg in args)
            {
                Arguments.Add(arg);
            }
            if (AppRunning.CheckIfRunning("VPKSoft.amp.sharp#"))
            {
                if (Arguments.Count > 0)
                {
                    string msg = string.Empty;
                    foreach (string a in Arguments)
                    {
                        if (File.Exists(a))
                        {
                            if (Path.GetExtension(a).ToUpper() == ".mp3".ToUpper() ||
                                Path.GetExtension(a).ToUpper() == ".ogg".ToUpper() ||
                                Path.GetExtension(a).ToUpper() == ".flac".ToUpper() ||
                                Path.GetExtension(a).ToUpper() == ".wma".ToUpper() ||
                                Path.GetExtension(a).ToUpper() == ".wav".ToUpper())
                            {
                                msg += a + ";";
                            }
                        }
                    }
                    if (msg.Length > 0)
                    {
                        msg = msg.TrimEnd(';');
                        RemotingMessageClient amp_client = new RemotingMessageClient(50670, "apm_sharp_remoting");
                        amp_client.SendMessage(msg, amp_client.TCPPort, amp_client.UriExtension);
                    }
                }
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully
                return;
            }
            PositionCore.Bind(); // attach the PosLib to the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Save languages
            if (Utils.ShouldLocalize() != null)
            {
                new MainWindow();
                new FormPsycho();
                new FormAddAlbum();
                new FormRename();
                new FormQueueSnapshotName(); // 16.10.17
                new FormSavedQueues(); // 18.10.17
                new FormModifySavedQueue(); // 19.10.17
                new FormSettings(); // 21.10.17
                new FormTagInfo(); // 11.02.18
                new FormAlbumNaming(); // 27.10.18
                new FormDatabaseUpdatingProgress(); // 27.10.18
                new FormHelp(); // 28.10.18
                new FormRandomizePriority(); // 30.10.18
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully
                return;
            }
            // End save languages

            DBLangEngine.UseCulture = Settings.Culture; // set the localization value..
            Application.Run(new MainWindow());
            FormHelp.DisposeSingleton(); // release the help form if any..
            PositionCore.UnBind(); // release the event handlers used by the PosLib and save the default data
            ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
        }
    }
}
