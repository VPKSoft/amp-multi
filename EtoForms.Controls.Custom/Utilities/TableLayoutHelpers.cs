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

using Eto.Drawing;
using Eto.Forms;

namespace EtoForms.Controls.Custom.Utilities;

/// <summary>
/// Some helper utilities for the <see cref="TableLayout"/> class.
/// </summary>
public static class TableLayoutHelpers
{
    /// <summary>
    /// Creates a <see cref="TableLayout"/> instance from the specified controls
    /// </summary>
    /// <param name="lastScaleWidth">if set to <c>true</c> the last control is scaled to maximum width.</param>
    /// <param name="spacing">The spacing between controls.</param>
    /// <param name="controls">The controls for the table layout.</param>
    /// <returns>An instance to a <see cref="TableLayout"/> class to be used as a row in another <see cref="TableLayout"/>.</returns>
    public static TableLayout CreateRowTable(bool lastScaleWidth, Size spacing, params object[] controls)
    {
        var tableRow = new TableRow();
        var result = new TableLayout
        {
            Rows =
            {
                tableRow,
            },
            Spacing = spacing,
        };

        foreach (var control in controls)
        {
            if (control is Control c)
            {
                tableRow.Cells.Add(new TableCell(c));

            }

            if (control is TableCell tc)
            {
                tableRow.Cells.Add(tc);
            }
        }

        if (lastScaleWidth)
        {
            tableRow.Cells.Add(new TableCell { ScaleWidth = true, });
        }

        return result;
    }
}