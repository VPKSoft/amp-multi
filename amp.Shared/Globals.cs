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

using System.Globalization;
using amp.Shared.Localization;

namespace amp.Shared;

/// <summary>
/// Global definitions for the library.
/// </summary>
public class Globals
{
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

    /// <summary>
    /// Gets or sets the locale for the localization resources.
    /// </summary>
    /// <value>The locale for localization resources.</value>
    public static string Locale
    {
        get => Culture.Name.Split('-')[0];

        set => Culture = new CultureInfo(value);
    }

    /// <summary>
    /// Overrides the current thread's CurrentUICulture property for all resource lookups.
    /// </summary>
    /// <value>The culture for localization.</value>
    public static CultureInfo Culture
    {
        get => UI.Culture;

        set
        {
            Messages.Culture = value;
            Mac.Culture = value;
            UI.Culture = value;
            EtoForms.Culture = value;
        }
    }

    /// <summary>
    /// The localized languages in this library.
    /// </summary>
    public static readonly string[] Languages = { "en", "fi", };
}