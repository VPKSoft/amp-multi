#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using VPKSoft.Utils;
using VPKSoft.LangLib;
using VPKSoft.PosLib;
using VPKSoft.ErrorLogger; // (C): http://www.vpksoft.net/, GNU Lesser General Public License Version 3
using System.Diagnostics;

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
            Process localizeProcess = VPKSoft.LangLib.Utils.CreateDBLocalizeProcess(VPKSoft.Utils.Paths.AppInstallDir);
            // localizeProcess..

            if (localizeProcess != null)
            {
                localizeProcess.Start();
                return;
            }


            ExceptionLogger.Bind(); // bind before any visual objects are created


            if (Settings.DBUpdateRequiredLevel < 1)
            {
                if (AppRunning.CheckIfRunning("VPKSoft.amp.DBUpdate.sharp#"))
                {
                    ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                    return;
                }
                DBLangEngine.UseCulture = Settings.Culture; // set the localization value..
                Application.Run(new FormDatabaseUpdatingProgress());
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                Settings.DBUpdateRequiredLevel = 1;
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
            if (VPKSoft.LangLib.Utils.ShouldLocalize() != null)
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
