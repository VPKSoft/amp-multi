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

using amp.Database.DataModel;
using amp.Shared.Classes;
using AutoMapper;
using Eto.Drawing;
using Serilog.Core;

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
    private static Logger? logger;
#pragma warning restore CS0649

    /// <summary>
    /// Gets the <see cref="Serilog.Core.Logger"/> with not null value unless in debug mode.
    /// </summary>
    internal static Logger? Logger
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
    /// <param name="errorAction">The action to invoke in case of an error.</param>
    /// <returns><c>true</c> if the action was invoked without an exception, <c>false</c> otherwise.</returns>
    public static bool LoggerSafeInvoke(Action action, Action<Exception>? errorAction = null)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            errorAction?.Invoke(ex);
            Logger?.Error(ex, "");
            return false;
        }
    }

    /// <summary>
    /// Invokes the func asynchronously with exception handling and logs the possible exception.
    /// </summary>
    /// <param name="action">The asynchronous func to invoke.</param>
    /// <param name="errorAction">The asynchronous func to invoke in case of an error.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public static async Task<bool> LoggerSafeInvokeAsync(Func<Task> action, Func<Exception, Task>? errorAction = null)
    {
        try
        {
            await action();
            return true;
        }
        catch (Exception ex)
        {
            if (errorAction != null)
            {
                await errorAction.Invoke(ex);
            }
            Logger?.Error(ex, "");
            return false;
        }
    }

    /// <summary>
    /// Invokes the action with exception handling and logs the possible exception.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="errorAction">The action to invoke in case of an error.</param>
    /// <returns><c>true</c> if the action was invoked without an exception, <c>false</c> otherwise.</returns>
    public static bool LoggerSafeInvoke(Func<Task> action, Action<Exception>? errorAction = null)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            errorAction?.Invoke(ex);
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
    /// Gets the spacing size based on the <see cref="DefaultPadding"/> value.
    /// </summary>
    /// <value>The spacing size based on the <see cref="DefaultPadding"/> value.</value>
    internal static Size DefaultSpacing => new(DefaultPadding, DefaultPadding);

    private static Size? defaultButtonSize;

    /// <summary>
    /// Gets or sets the default size of a button.
    /// </summary>
    /// <value>The default size of a button.</value>
    internal static Size ButtonDefaultSize
    {
        get
        {
            defaultButtonSize ??= UtilityOS.GetValueForOSNotNull(
                new Size(40, 40),
                new Size(20, 20),
                new Size(50, 50),
                new Size(40, 40));

            return defaultButtonSize.Value;
        }

        set => defaultButtonSize = value;
    }

    private static Size? menuImageDefaultSize;

    /// <summary>
    /// Gets or sets the default size of the menu item images.
    /// </summary>
    /// <value>The default size of the menu item images.</value>
    internal static Size MenuImageDefaultSize
    {
        get
        {
            menuImageDefaultSize ??= UtilityOS.GetValueForOSNotNull(
                new Size(16, 16),
                new Size(16, 16),
                new Size(16, 16),
                new Size(16, 16));

            return menuImageDefaultSize.Value;
        }

        set => menuImageDefaultSize = value;
    }

    /// <summary>
    /// Saves the application settings.
    /// </summary>
    internal static void SaveSettings()
    {
        Settings.Save(Path.Combine(DataFolder, "settings.json"));
    }

    /// <summary>
    /// Gets or sets the floating point comparison tolerance.
    /// </summary>
    /// <value>The floating point comparison tolerance.</value>
    public static double FloatingPointTolerance { get; set; } = 0.000000001;

    /// <summary>
    /// Gets or sets the floating point comparison tolerance for the single-precision floating point values.
    /// </summary>
    /// <value>The floating point comparison tolerance.</value>
    public static float FloatingPointSingleTolerance { get; set; } = 0.00001f;

    private static MapperConfiguration? mapperConfiguration;
    private static IMapper? mapper;

    /// <summary>
    /// Gets the automatic mapper.
    /// </summary>
    /// <value>The automatic mapper.</value>
    internal static IMapper AutoMapper
    {
        get
        {
            mapperConfiguration ??= new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlbumSong, Models.AlbumSong>();
                cfg.CreateMap<Song, Models.Song>();
                cfg.CreateMap<Album, Models.Album>();
                cfg.CreateMap<Models.AlbumSong, AlbumSong>();
                cfg.CreateMap<Models.Song, Song>();
                cfg.CreateMap<Models.Album, Album>();
            });

            mapper ??= mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}