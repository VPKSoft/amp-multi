#region License
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

#define UseRunProgramDialog
// define this to use the custom association dialog..
#define UseAssociationDialog
// define this to use the local application data folder..
#define InstallLocalAppData

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using InstallerBaseWixSharp.Files.Dialogs;
using InstallerBaseWixSharp.Files.Localization.TabDeliLocalization;
using InstallerBaseWixSharp.Registry;
using WixSharp;
using WixSharp.Forms;
using File = WixSharp.File;

namespace InstallerBaseWixSharp
{
    class Program
    {
        const string AppName = "amp#";
        internal static readonly string Executable = $"{AppName.TrimEnd('#')}.exe";
        const string  Company = "VPKSoft";
        private static readonly string InstallDirectory = $@"%ProgramFiles%\{Company}\{AppName}";
        const string  ApplicationIcon = @".\Files\FileResources\icon.ico";

        static void Main()
        {
            string appVersion = "1.0.0.0";
            string OutputFile()
            {
                try
                {
                    var info = FileVersionInfo.GetVersionInfo(@"..\amp\bin\Release\net47\win10-x64\amp.exe");
                    appVersion = string.Concat(info.FileMajorPart, ".", info.FileMinorPart, ".", info.FileBuildPart);
                    return string.Concat(AppName, "_", info.FileMajorPart, ".", info.FileMinorPart, ".", info.FileBuildPart);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return AppName;
                }
            }

            var project = new ManagedProject("amp#",
                new Dir(InstallDirectory,
                    new WixSharp.Files(@"..\amp\bin\Release\net47\win10-x64\*.*"),
                    new File("Program.cs")),
                new Dir($@"%ProgramMenu%\{Company}\{AppName}",
                    // ReSharper disable three times StringLiteralTypo
                    new ExeFileShortcut(AppName, $"[INSTALLDIR]{Executable}", "")
                    {
                        WorkingDirectory = "[INSTALLDIR]", IconFile = ApplicationIcon
                    }),
#if InstallLocalAppData
                new Dir($@"%LocalAppDataFolder%\{AppName}", new File(@"..\amp\Localization\lang.sqlite")),
#endif

                new CloseApplication($"[INSTALLDIR]{Executable}", true), 
                new Property("Executable", Executable),
                new Property("AppName", AppName),
                new Property("Company", Company))
            {
                GUID = new Guid("3E320290-4AB2-4DA5-9F90-C3C775EDA03C"),
                ManagedUI = new ManagedUI(),
                ControlPanelInfo = 
                {
                    Manufacturer = Company, 
                    UrlInfoAbout  = "https://www.vpksoft.net", 
                    Name = $"Installer for the {AppName} application", 
                    HelpLink = "https://www.vpksoft.net", 
                },
                Platform = Platform.x64,
                OutFileName = OutputFile(),
            };

            #region Upgrade
            // the application update process..
            project.Version = Version.Parse(appVersion);
            project.MajorUpgrade = MajorUpgrade.Default;
            #endregion

            project.Package.Name = $"Installer for the {AppName} application";

            //project.ManagedUI = ManagedUI.Empty;    //no standard UI dialogs
            //project.ManagedUI = ManagedUI.Default;  //all standard UI dialogs

            //custom set of standard UI dialogs

            project.ManagedUI.InstallDialogs.Add(Dialogs.Welcome)
                                            .Add(Dialogs.Licence)
#if UseRunProgramDialog
                                            .Add<RunProgramDialog>()
#endif
#if UseAssociationDialog
                                            .Add<AssociationsDialog>()
#endif                                            
                                            .Add<ProgressDialog>()
                                            .Add(Dialogs.Exit);

            project.ManagedUI.ModifyDialogs.Add(Dialogs.MaintenanceType)
                                           .Add(Dialogs.Features)
                                           .Add(Dialogs.Progress)
                                           .Add(Dialogs.Exit);

            project.ControlPanelInfo.ProductIcon = ApplicationIcon;

            AutoElements.UACWarning = "";


            project.BannerImage = @"Files\install_top.png";
            project.BackgroundImage = @"Files\install_side.png";
            project.LicenceFile = @"Files\MIT.License.rtf";

            RegistryFileAssociation.ReportExceptionAction = delegate(Exception ex)
            {
                MessageBox.Show(ex.Message);
            };

            project.AfterInstall += delegate(SetupEventArgs args)
            {
                if (args.IsUninstalling)
                {
                    string locale = "en-US";
                    try
                    {
                        locale = CommonCalls.GetKeyValue(Company, AppName, "LOCALE");
                        CommonCalls.DeleteValue(Company, AppName, "LOCALE");
                    }
                    catch
                    {
                        // ignored..
                    }

                    RegistryFileAssociation.UnAssociateFiles(Company, AppName);
                    CommonCalls.DeleteCompanyKeyIfEmpty(Company);

                    #if InstallLocalAppData
                    var sideLocalization = new TabDeliLocalization();
                    sideLocalization.GetLocalizedTexts(Properties.Resources.tabdeli_messages);

                    var messageCaption = sideLocalization.GetMessage("txtDeleteLocalApplicationData",
                        "Delete application data", locale);

                    var messageText = sideLocalization.GetMessage("txtDeleteApplicationDataQuery",
                        "Delete application settings and other data?", locale);

                    if (MessageBox.Show(messageText,
                        messageCaption, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete(
                                Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? string.Empty,
                                    AppName), true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endif
                }

                if (args.IsInstalling)
                {
                    RegistryFileAssociation.AssociateFiles(Company, AppName, args.Session.Property("RUNEXE"),
                        args.Session.Property("ASSOCIATIONS"), true);

                    try
                    {
                        CommonCalls.SetKeyValue(Company, AppName, "LOCALE", args.Session.Property("LANGNAME"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            project.Load += Msi_Load;
            project.BeforeInstall += Msi_BeforeInstall;
            project.AfterInstall += Msi_AfterInstall;


            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            ValidateAssemblyCompatibility();

            project.DefaultDeferredProperties += ",RUNEXE,PIDPARAM,ASSOCIATIONS,LANGNAME";

            project.Localize();

            project.BuildMsi();
        }

        static void Msi_Load(SetupEventArgs e)
        {
            //if (!e.IsUninstalling) MessageBox.Show(e.ToString(), "Load");
        }

        static void Msi_BeforeInstall(SetupEventArgs e)
        {
            //if (!e.IsUninstalling) MessageBox.Show(e.ToString(), "BeforeInstall");
        }

        static void Msi_AfterInstall(SetupEventArgs e)
        {
            // run the executable after the install with delay (wait PID to )..
            if (e.IsInstalling)
            {
                try 
                {
                    if (System.IO.File.Exists(e.Session.Property("RUNEXE")))
                    {
                        var startInfo = new ProcessStartInfo(e.Session.Property("RUNEXE"),
                            $"--waitPid {e.Session.Property("PIDPARAM")}");

                        // start the process as the current user..
                        startInfo.UseShellExecute = false;
                        startInfo.LoadUserProfile = true;
                        Process.Start(startInfo);
                    }
                    else
                    {
                        Console.WriteLine(e.Session.Property("RUNEXE"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Post run failed: '{ex.Message}'...");
                }
            }
        }

        static void ValidateAssemblyCompatibility()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (!assembly.ImageRuntimeVersion.StartsWith("v2."))
            {
                Console.WriteLine(
                    $@"Warning: assembly '{assembly.GetName().Name}' is compiled for {assembly.ImageRuntimeVersion}" +
                    @" runtime, which may not be compatible with the CLR version hosted by MSI. " +
                    @"The incompatibility is particularly possible for the EmbeddedUI scenarios. " +
                    @"The safest way to solve the problem is to compile the assembly for v3.5 Target Framework.");
            }
        }
    }
}




