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

namespace EtoForms.Controls.Custom.Drawing;

/// <summary>
/// Class CellPainter.
/// Implements the <see cref="IDisposable" />
/// </summary>
/// <typeparam name="T">The type of the grid row data</typeparam>
/// <typeparam name="TValue">The type of the grid row data member.</typeparam>
/// <seealso cref="IDisposable" />
public abstract class CellPainter<T, TValue> : IDisposable
{
    private readonly DrawableCell cell;
    internal readonly GridView GridView;
    private readonly Func<T, TValue?> getValueFunc;

    /// <summary>
    /// Initializes a new instance of the <see cref="CellPainter{T, TValue}"/> class.
    /// </summary>
    /// <param name="gridView">The grid view which the cell belongs to.</param>
    /// <param name="column">The column which cell is to be custom painted.</param>
    /// <param name="getValueFunc">An access func to get the value of the property required for painting the grid cell.</param>
    /// <exception cref="InvalidOperationException">The property name must exists within the type T.</exception>
    protected CellPainter(GridView gridView, GridColumn column, Func<T, TValue?> getValueFunc)
    {
        this.GridView = gridView;
        if (column.DataCell is DrawableCell drawableCell)
        {
            cell = drawableCell;
        }
        else
        {
            cell = new DrawableCell();
            column.DataCell = cell;
        }

        this.GridView = gridView;
        this.getValueFunc = getValueFunc;
        cell.Paint += DrawableCell_Paint;
    }

    /// <summary>
    /// The cell paint handler.
    /// </summary>
    public EventHandler<CellPaintEventArgs>? CellPaintHandler;

    /// <summary>
    /// Gets or sets the foreground color for the custom painting.
    /// </summary>
    /// <value>The foreground color for the custom painting.</value>
    public Color ForegroundColor { get; set; }

    /// <summary>
    /// Gets or sets the background color for the custom painting.
    /// </summary>
    /// <value>The background color for the custom painting.</value>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the SVG image bytes.
    /// </summary>
    /// <value>The SVG image bytes.</value>
    public byte[]? SvgImageBytes { get; set; }

    /// <summary>
    /// Handles the Paint event of the DrawableCell control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DrawableCellPaintEventArgs"/> instance containing the event data.</param>
    /// <exception cref="NotImplementedException"></exception>
    private void DrawableCell_Paint(object? sender, CellPaintEventArgs e)
    {
        CellPaintHandler?.Invoke(sender, e);
    }

    /// <summary>
    /// Gets the drawable cell value.
    /// </summary>
    /// <param name="eventArgs">The <see cref="CellPaintEventArgs"/> instance containing the event data.</param>
    /// <returns>System.Nullable&lt;TValue&gt;.</returns>
    internal TValue? GetDrawableCellValue(CellPaintEventArgs eventArgs)
    {
        return getValueFunc.Invoke((T)eventArgs.Item);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        cell.Paint -= DrawableCell_Paint;
    }
}