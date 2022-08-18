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

using System.Reflection;
using amp.Shared.Classes;
using Eto.Forms;

namespace amp.EtoForms.Utilities;

/// <summary>
/// A class to wrap the web browser help.
/// </summary>
internal static class Help
{
    /// <summary>
    /// Launches a web browser to display the software help.
    /// </summary>
    internal static void LaunchHelp()
    {
        Globals.LoggerSafeInvoke(() =>
        {
            var helpPath = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
            if (string.IsNullOrWhiteSpace(helpPath))
            {
                var args = Environment.GetCommandLineArgs();
                if (args.Length > 0)
                {
                    helpPath = args[0];
                }
            }

            if (!string.IsNullOrWhiteSpace(helpPath))
            {
                var directoryName = Path.GetDirectoryName(helpPath);
                if (directoryName != null)
                {
                    helpPath = directoryName;
                }
            }

            var helpPathNative = Path.Join(helpPath, $"amp-help-{Globals.Settings.Locale}");
            var helpPathFallback = Path.Join(helpPath, "amp-help-en");

            var fileName = string.Empty;

            if (Directory.Exists(helpPathNative))
            {
                fileName = Path.Join(helpPathNative, "index.html");
            }
            else if (Directory.Exists(helpPathFallback))
            {
                fileName = Path.Join(helpPathFallback, "index.html");
            }

            if (File.Exists(fileName))
            {
                var uri = new Uri(fileName).AbsoluteUri;
                Application.Instance.Open(uri);
            }
        });
    }

    /// <summary>
    /// Launches the help from the folder specified in the settings.
    /// </summary>
    /// <param name="parent">The parent for a dialog in case the help file is not found.</param>
    internal static void LaunchHelpFromSettings(Control parent)
    {

        var helpFileFallback = Path.Combine(Globals.Settings.HelpFolder, $"amp-en-{UtilityOS.OsNameLowerCase}", "index.html");
        var helpFileCurrentLocale = Path.Combine(Globals.Settings.HelpFolder, $"amp-{Globals.Settings.Locale}-{UtilityOS.OsNameLowerCase}", "index.html");
        var indexHtml = Path.Combine(Globals.Settings.HelpFolder, "index.html");

        var helpFile = FileUtils.FirstExistingFile(helpFileCurrentLocale, helpFileFallback, indexHtml);

        if (File.Exists(helpFile))
        {
            var uri = new Uri(helpFile).AbsoluteUri;
            Application.Instance.Open(uri);
            return;
        }

        MessageBox.Show(parent, Shared.Localization.Messages.PleaseSetTheHelpPathFromTheSettings, Shared.Localization.Messages.Information);
    }
}