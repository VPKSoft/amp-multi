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
using Eto.Forms;

namespace EtoForms.Controls.Custom.Helpers;

/// <summary>
/// An extension class for <see cref="Window"/> centering.
/// </summary>
public static class CenterExtension
{
    /// <summary>
    /// Centers the form related to the parent <see cref="Window"/> location.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <param name="owner">The owner.</param>
    public static void CenterForm(this Window child, Window? owner)
    {
        if (owner == null || !child.Visible)
        {
            return;
        }

        var left = (owner.Width - child.Width) / 2;
        var top = (owner.Height - child.Height) / 2;

        child.Location = new Point(owner.Location.X + left, owner.Location.Y + top);
    }
}