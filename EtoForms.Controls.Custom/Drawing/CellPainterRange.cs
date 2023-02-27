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

using System;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom.Drawing;

/// <summary>
/// A class to paint a specific <see cref="GridView"/> cell with a value range using a colored SVG image.
/// Implements the <see cref="EtoForms.Controls.Custom.Drawing.CellPainter{T, TValue}" />
/// </summary>
/// <typeparam name="T">The grid row data type.</typeparam>
/// <seealso cref="EtoForms.Controls.Custom.Drawing.CellPainter{T, TValue}" />
public class CellPainterRange<T> : CellPainter<T, (int, bool)?>
{
    private readonly int maxValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="CellPainterRange{T}"/> class.
    /// </summary>
    /// <param name="gridView">The grid view which the cell belongs to.</param>
    /// <param name="column">The column which cell is to be custom painted.</param>
    /// <param name="maxValue">The maximum value of the cell range.</param>
    /// <param name="getValueFunc">An access func to get the value of the property required for painting the grid cell.</param>
    public CellPainterRange(GridView gridView, GridColumn column, int maxValue, Func<T, (int, bool)?> getValueFunc) : base(gridView, column, getValueFunc)
    {
        CellPaintHandler += CellPaintEventHandler;
        this.maxValue = maxValue;
    }

    private void CellPaintEventHandler(object? sender, CellPaintEventArgs e)
    {
        if (e.Item is not T)
        {
            return;
        }

        e.Graphics.FillRectangle(BackgroundColor ?? GridView.BackgroundColor, e.ClipRectangle);

        if (SvgImageBytes == null || SvgImageBytesUndefined == null)
        {
            return;
        }

        var wh = (int)Math.Min(e.ClipRectangle.Width, e.ClipRectangle.Height);
        var drawRect = new Size(wh, wh);

        var value = GetDrawableCellValue(e) ?? (0, false);

        using var drawImage = value.Item2
            ? EtoHelpers.ImageFromSvg(ForegroundColor, SvgImageBytes, drawRect)
            : EtoHelpers.ImageFromSvg(ForegroundColorUndefined, SvgImageBytesUndefined, drawRect);
                

        var left = (float)value.Item1 / maxValue * e.ClipRectangle.Width;

        var drawCount = (int)Math.Ceiling((double)e.ClipRectangle.Width / wh);

        e.Graphics.SetClip(new RectangleF(e.ClipRectangle.Left, e.ClipRectangle.Top, left + 1, e.ClipRectangle.Height));

        for (var i = 0; i < drawCount; i++)
        {
            e.Graphics.DrawImage(drawImage, new PointF(i * wh + e.ClipRectangle.Left, e.ClipRectangle.Top));
        }
    }

    
    /// <summary>
    /// Gets or sets the foreground color for the custom painting of undefined rating.
    /// </summary>
    /// <value>The foreground color for the custom painting of undefined rating.</value>
    public Color ForegroundColorUndefined { get; init; }

    /// <summary>
    /// Gets or sets the SVG image bytes of undefined rating.
    /// </summary>
    /// <value>The SVG image bytes of undefined rating.</value>
    public byte[]? SvgImageBytesUndefined { get; init; }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();
        CellPaintHandler -= CellPaintEventHandler;
    }
}