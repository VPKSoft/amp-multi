#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

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

using System.Diagnostics;
using amp.DataAccessLayer.DtoClasses;
using amp.EtoForms.Utilities;
using amp.Shared.Localization;
using Eto.Forms;
using VPKSoft.Utils.Common.EventArgs;
using UnhandledExceptionEventArgs = Eto.UnhandledExceptionEventArgs;

namespace amp.EtoForms;

/// <summary>
/// The program main entry point.
/// </summary>
public static class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    [STAThread]
    // ReSharper disable once UnusedParameter.Local, lets keep these arguments as this is the entry point of the application.
    static void Main(string[] args)
    {
        var processes = Array.Empty<Process>();

        ExceptionProvider.Instance.ExceptionOccurred += ExternalExceptionOccurred;

        try
        {
            Process.GetProcessesByName("amp.EtoForms");
        }
        catch (Exception ex)
        {
            Globals.Logger?.Error("Multi-instance process check failed.");
            Globals.Logger?.Error(ex, string.Empty);
        }

        if (processes.All(f => f.Id == Environment.ProcessId))
        {
            Thread.CurrentThread.CurrentUICulture =
                Thread.CurrentThread.CurrentCulture;

            AudioTrack.GenerateDisplayNameFunc = TrackDisplayNameGenerate.GetAudioTrackName;
            AlbumTrack.GenerateDisplayNameFunc = TrackDisplayNameGenerate.GetAudioTrackName;

            Shared.Globals.Locale = Globals.Settings.Locale;
            LocalizeExternals();
            FormSaveLoadPosition.SavePath = Globals.DataFolder;
#if OSX
Eto.Style.Add<Eto.Mac.Forms.ApplicationHandler>(null, handler => handler.AllowClosingMainForm = true);
#endif
            new Application().Run(new FormMain());
        }
        else
        {
            Globals.Logger?.Information("The application is already running.");
            Process.GetCurrentProcess().Kill();
        }
    }

    private static void ExternalExceptionOccurred(object? sender, ExceptionOccurredEventArgs e)
    {
        Globals.Logger?.Error(e.Exception, string.Empty);
    }

    internal static void Instance_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        Globals.Logger?.Error((Exception)e.ExceptionObject, "");
    }

    private static void LocalizeExternals()
    {
        global::EtoForms.Controls.Custom.Globals.OkButtonText = UI.OK;
        global::EtoForms.Controls.Custom.Globals.CancelButtonText = UI.Cancel;
    }
}