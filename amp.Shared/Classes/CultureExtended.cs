#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

namespace amp.Shared.Classes;

/// <summary>
/// Due to a bug in Eto.Forms (WPF) the ComboBox selected item text must be overridden (See: https://github.com/picoe/Eto/issues/414)
/// Implements the <see cref="System.Globalization.CultureInfo" />
/// </summary>
/// <seealso cref="System.Globalization.CultureInfo" />
public class CultureExtended : CultureInfo
{
    private readonly bool local;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureExtended"/> class.
    /// </summary>
    /// <param name="name">The name of the culture.</param>
    /// <param name="local">if set to <c>true</c> the <see cref="ToString"/> returns the localized culture name].</param>
    public CultureExtended(string name, bool local) : base(name)
    {
        this.local = local;
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return local ? base.DisplayName : base.EnglishName;
    }
}