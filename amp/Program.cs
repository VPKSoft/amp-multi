﻿#region License
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
using System.Threading;
using System.Windows.Forms;
using amp.FormsUtility;
using amp.FormsUtility.Help;
using amp.FormsUtility.Progress;
using amp.FormsUtility.QueueHandling;
using amp.FormsUtility.Random;
using amp.FormsUtility.Songs;
using amp.UtilityClasses.Settings;
using VPKSoft.ErrorLogger;
using VPKSoft.IPC;
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
        [STAThread]
        static void Main()
        {
            Process localizeProcess = Utils.CreateDBLocalizeProcess(Paths.AppInstallDir);
            // localizeProcess..

            if (localizeProcess != null)
            {
                localizeProcess.Start();
                return;
            }

            ExceptionLogger.Bind(); // bind before any visual objects are created
            ExceptionLogger.ApplicationCrashData += ExceptionLogger_ApplicationCrashData;


            // Save languages
            if (Utils.ShouldLocalize() != null)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new FormMain();
                // ReSharper disable once ObjectCreationAsStatement
                new FormPsycho();
                // ReSharper disable once ObjectCreationAsStatement
                new FormProgressBackground();
                // ReSharper disable once ObjectCreationAsStatement
                new FormAddAlbum();
                // ReSharper disable once ObjectCreationAsStatement
                new FormRename();
                // ReSharper disable once ObjectCreationAsStatement
                new FormQueueSnapshotName(); // 16.10.17
                // ReSharper disable once ObjectCreationAsStatement
                new FormSavedQueues(); // 18.10.17
                // ReSharper disable once ObjectCreationAsStatement
                new FormModifySavedQueue(); // 19.10.17
                // ReSharper disable once ObjectCreationAsStatement
                new FormSettings(); // 21.10.17
                // ReSharper disable once ObjectCreationAsStatement
                new FormTagInfo(); // 11.02.18
                // ReSharper disable once ObjectCreationAsStatement
                new FormAlbumNaming(); // 27.10.18
                // ReSharper disable once ObjectCreationAsStatement
                new FormDatabaseUpdatingProgress(); // 27.10.18
                // ReSharper disable once ObjectCreationAsStatement
                new FormHelp(); // 28.10.18
                // ReSharper disable once ObjectCreationAsStatement
                new FormRandomizePriority(); // 30.10.18
                ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully
                return;
            }
            // End save languages


            if (Settings.DbUpdateRequiredLevel < 1)
            {
                if (AppRunning.CheckIfRunning("VPKSoft.amp.DBUpdate.sharp#"))
                {
                    ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
                    ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                    return;
                }
                DBLangEngine.UseCulture = Settings.Culture; // set the localization value..
                Application.Run(new FormDatabaseUpdatingProgress());
                ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
                Settings.DbUpdateRequiredLevel = 1;
                Process.Start(new ProcessStartInfo(Path.Combine(Paths.AppInstallDir, "amp.exe"))); // the database is updated..
                return;
            }

            if (AppRunning.CheckIfRunning("VPKSoft.amp.sharp#"))
            {
                ExceptionLogger.LogMessage($"Application is running. Checking for open file requests. The current directory is: '{Environment.CurrentDirectory}'.");
                try
                {
                    IpcClientServer ipcClient = new IpcClientServer();
                    ipcClient.CreateClient("localhost", 50671);
                    string[] args = Environment.GetCommandLineArgs();

                    // only send the existing files to the running instance, don't send the executable
                    // file name thus the start from 1..
                    for (int i = 1; i < args.Length; i++)
                    {
                        string file = args[i];
                        
                        ExceptionLogger.LogMessage($"Request file open: '{file}'.");
                        if (File.Exists(file))
                        {
                            ExceptionLogger.LogMessage($"File exists: '{file}'. Send open request.");
                            ipcClient.SendMessage(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogError(ex);
                    // just in case something fails with the IPC communication..
                }
                ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
                ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully..
                return;
            }
            else
            {
                // create an IPC server at localhost, the port was randomized in the development phase..
                IpcServer.CreateServer("localhost", 50671);

                // subscribe to the IPC event if the application receives a message from another instance of this application..
                IpcClientServer.RemoteMessage.MessageReceived += RemoteMessage_MessageReceived;
            }

            PositionCore.Bind(); // attach the PosLib to the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DBLangEngine.UseCulture = Settings.Culture; // set the localization value..
            Application.Run(new FormMain());
            FormHelp.DisposeSingleton(); // release the help form if any..
            PositionCore.UnBind(); // release the event handlers used by the PosLib and save the default data

            // unsubscribe the IpcClientServer MessageReceived event handler..
            IpcClientServer.RemoteMessage.MessageReceived -= RemoteMessage_MessageReceived;

            ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
            ExceptionLogger.UnBind(); // unbind so the truncate thread is stopped successfully        
        }

        // the application is crashing without exception handling..
        private static void ExceptionLogger_ApplicationCrashData(ApplicationCrashEventArgs e)
        {
            // unsubscribe this event handler..
            ExceptionLogger.ApplicationCrashData -= ExceptionLogger_ApplicationCrashData;
            IpcClientServer.RemoteMessage.MessageReceived -= RemoteMessage_MessageReceived;
            ExceptionLogger.UnBind(); // unbind the exception logger..

            // kill self as the native inter-op libraries may have some issues of keeping the process alive..
            Process.GetCurrentProcess().Kill();

            // This is the end..
        }

        /// <summary>
        /// The IPC channel pushes the existing files to be added to the the list to be added to the temporary album.
        /// </summary>
        private static readonly List<string> TemporaryRemoteFiles = new List<string>();

        // an event handler for the IPC channel to add files to the temporary album via user shell interaction..
        private static void RemoteMessage_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (FormMain.RemoteFileBeingProcessed)
            {
                TemporaryRemoteFiles.Add(e.Message);
            }
            else
            {
                FormMain.StopIpcTimer = true;
                FormMain.RemoteFiles.Add(e.Message);
                FormMain.RemoteFiles.AddRange(TemporaryRemoteFiles);
                TemporaryRemoteFiles.Clear();
                FormMain.StopIpcTimer = false;
            }

            bool threadWait = false;

            while (TemporaryRemoteFiles.Count > 0 && FormMain.RemoteFileBeingProcessed)
            {
                threadWait = true;
                Thread.Sleep(100);
            }

            if (threadWait)
            {
                FormMain.StopIpcTimer = true;
                FormMain.RemoteFiles.AddRange(TemporaryRemoteFiles);
                TemporaryRemoteFiles.Clear();
                FormMain.StopIpcTimer = false;
            }
        }

        /// <summary>
        /// An IPC client / server to transmit Windows shell file open requests to the current process.
        /// (C): VPKSoft: https://gist.github.com/VPKSoft/5d78f1c06ec51ebad34817b491fe6ac6
        /// </summary>
        private static readonly IpcClientServer IpcServer = new IpcClientServer();
    }
}
