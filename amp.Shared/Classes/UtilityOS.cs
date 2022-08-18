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

using System.Runtime.InteropServices;

namespace amp.Shared.Classes;

/// <summary>
/// Some operating system utilities.
/// </summary>
// ReSharper disable once InconsistentNaming, OS is upper case
public static class UtilityOS
{
    /// <summary>
    /// Gets a nullable value depending on the current operating system.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="windowsValue">The value for Windows operation system.</param>
    /// <param name="linuxValue">The value for Linux operation system.</param>
    /// <param name="macValue">The value for macOS operation system.</param>
    /// <param name="fallback">The fallback value if the operating system was not Linux, Windows or macOS.</param>
    /// <returns>A value of type of <typeparamref name="T"/>?.</returns>
    // ReSharper disable once InconsistentNaming, OS is upper case
    public static T? GetValueForOS<T>(T? windowsValue, T? linuxValue, T? macValue, T? fallback)
    {
        if (IsWindowsOS)
        {
            return windowsValue;
        }

        if (IsLinuxOS)
        {
            return linuxValue;
        }

        if (IsMacOS)
        {
            return macValue;
        }

        return fallback;
    }

    /// <summary>
    /// Gets a nullable value depending on the current operating system.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="windowsValue">The value for Windows operation system.</param>
    /// <param name="linuxValue">The value for Linux operation system.</param>
    /// <param name="macValue">The value for macOS operation system.</param>
    /// <param name="fallback">The fallback value if the operating system was not Linux, Windows or macOS.</param>
    /// <returns>A value of type of <typeparamref name="T"/>.</returns>
    // ReSharper disable once InconsistentNaming, OS is upper case
    public static T GetValueForOSNotNull<T>(T windowsValue, T linuxValue, T macValue, T fallback)
    {
        return GetValueForOS(windowsValue, linuxValue, macValue, fallback)!;
    }

    /// <summary>
    /// Gets a value whether the current operating system is Windows.
    /// </summary>
    /// <value><c>true</c> if the current operating system is Windows; otherwise, <c>false</c>.</value>
    // ReSharper disable once InconsistentNaming, OS is upper case
    public static bool IsWindowsOS => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets a value whether the current operating system is macOS.
    /// </summary>
    /// <value><c>true</c> if the current operating system is macOS; otherwise, <c>false</c>.</value>
    // ReSharper disable once InconsistentNaming, OS is upper case
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    /// <summary>
    /// Gets a value whether the current operating system is Linux.
    /// </summary>
    /// <value><c>true</c> if the current operating system is Linux; otherwise, <c>false</c>.</value>
    // ReSharper disable once InconsistentNaming, OS is upper case
    public static bool IsLinuxOS => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// The macOS operating system name in lower case ("macos").
    /// </summary>
    public const string MacOSNameLowerCase = "macos";

    /// <summary>
    /// The Windows operating system name in lower case ("windows").
    /// </summary>
    public const string WindowsNameLowerCase = "windows";

    /// <summary>
    /// The Linux operating system name in lower case ("linux").
    /// </summary>
    public const string LinuxNameLowerCase = "linux";


    /// <summary>
    /// Gets the operating system name in lower case. E.g. windows, linux, macos.
    /// </summary>
    /// <value>The operating system name in lower case.</value>
    public static string OsNameLowerCase
    {
        get
        {
            if (IsMacOS)
            {
                return MacOSNameLowerCase;
            }

            if (IsLinuxOS)
            {
                return LinuxNameLowerCase;
            }

            return WindowsNameLowerCase;
        }
    }
}