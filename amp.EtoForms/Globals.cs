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

using Eto.Drawing;

namespace amp.EtoForms;

/// <summary>
/// Global data for the amp# application.
/// </summary>
internal static class Globals
{
    private static Settings.Settings? settings;
    private static string? dataFolder;

    /// <summary>
    /// Gets the application data folder for the amp# software.
    /// </summary>
    /// <value>The application data folder for the amp# software.</value>
    internal static string DataFolder
    {
        get
        {
            dataFolder ??= Settings.CreateApplicationSettingsFolder("VPKSoft", "amp");
            return dataFolder;
        }
    }

#pragma warning disable CS0649
    private static Serilog.Core.Logger? logger;
#pragma warning restore CS0649

    /// <summary>
    /// Gets the <see cref="Serilog.Core.Logger"/> with not null value unless in debug mode.
    /// </summary>
    internal static Serilog.Core.Logger? Logger
    {
        get
        {
            if (logger == null)
            {
#if !DEBUG
                logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(DataFolder, "amp_log.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 20_000_000, retainedFileCountLimit: 10)
                    .CreateLogger();
#endif
            }

            return logger;
        }
    }

    /// <summary>
    /// Gets the application settings.
    /// </summary>
    /// <value>The application settings.</value>
    internal static Settings.Settings Settings
    {
        get
        {
            var reload = false;
            if (settings == null)
            {
                settings = new Settings.Settings();
                reload = true;
            }

            dataFolder = settings.CreateApplicationSettingsFolder("VPKSoft", "amp");

            if (reload)
            {
                settings.Load(Path.Combine(dataFolder, "settings.json"));
            }

            return settings;
        }
    }

    /// <summary>
    /// Invokes the action with exception handling and logs the possible exception.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <returns><c>true</c> if the action was invoked without an exception, <c>false</c> otherwise.</returns>
    public static bool LoggerSafeInvoke(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            Logger?.Error(ex, "");
            return false;
        }
    }

    /// <summary>
    /// Gets or sets the width of the window border.
    /// </summary>
    /// <value>The width of the window border.</value>
    internal static int WindowBorderWidth { get; set; } = 10;

    /// <summary>
    /// Gets or sets the default padding.
    /// </summary>
    /// <value>The default padding.</value>
    internal static int DefaultPadding { get; set; } = 5;

    /// <summary>
    /// Gets or sets the default size of a button.
    /// </summary>
    /// <value>The default size of a button.</value>
    internal static Size ButtonDefaultSize { get; set; } = new(40, 40);

    /// <summary>
    /// Gets or sets the default size of the menu item images.
    /// </summary>
    /// <value>The default size of the menu item images.</value>
    internal static Size MenuImageDefaultSize { get; set; } = new(16, 16);


    /// <summary>
    /// Saves the application settings.
    /// </summary>
    internal static void SaveSettings()
    {
        Settings.Save(Path.Combine(DataFolder, "settings.json"));
    }
}