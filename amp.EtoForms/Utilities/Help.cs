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
            string helpPath = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
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
}