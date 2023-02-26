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

using Eto.Forms;

namespace EtoForms.Controls.Custom.Utilities;

/// <summary>
/// A class to help create a <see cref="TableLayout"/> layout.
/// </summary>
public class FluentTableLayoutBuilder
{
    /// <summary>
    /// Prevents a default instance of the <see cref="FluentTableLayoutBuilder"/> class from being created.
    /// </summary>
    private FluentTableLayoutBuilder()
    {

    }

    /// <summary>
    /// Gets or sets the root <see cref="TableLayout"/> control.
    /// </summary>
    /// <value>The root table layout.</value>
    public TableLayout? RootTableLayout { get; set; }

    /// <summary>
    /// Gets or sets the control spacing.
    /// </summary>
    /// <value>The spacing.</value>
    public int? Spacing { get; set; }

    /// <summary>
    /// Creates a new instance of the <seealso cref="FluentTableLayoutBuilder"/> class and initializes the <see cref="RootTableLayout"/> property for it.
    /// </summary>
    /// <returns>FluentTableLayoutBuilder.</returns>
    public static FluentTableLayoutBuilder New()
    {
        var result = new FluentTableLayoutBuilder
        {
            RootTableLayout = new TableLayout(),
        };

        return result;
    }

    /// <summary>
    /// Sets the control <see cref="Spacing"/> value.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithSpacing(int spacing)
    {
        Spacing = spacing;

        return this;
    }

    /// <summary>
    /// Creates a new <see cref="TableRow"/> to the <see cref="RootTableLayout"/> control.
    /// </summary>
    /// <param name="controls">The controls for the row.</param>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithRootRow(params Control[] controls)
    {
        return WithRootRow(Spacing!.Value, controls);
    }

    /// <summary>
    /// Creates a new <see cref="TableRow"/> to the <see cref="RootTableLayout"/> control.
    /// </summary>
    /// <param name="spacing">The spacing between the controls within the row.</param>
    /// <param name="controls">The controls for the row.</param>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithRootRow(int spacing, params Control[] controls)
    {
        var tableRow = new TableRow();

        for (int i = 0; i < controls.Length; i++)
        {
            if (i + 1 < controls.Length)
            {
                tableRow.Cells.Add(new TableCell(controls[i]));
                tableRow.Cells.Add(new Panel { Width = spacing, });
            }
            else
            {
                tableRow.Cells.Add(new TableCell(controls[i]));
            }
        }

        RootTableLayout!.Rows.Add(tableRow);
        return this;
    }

    /// <summary>
    /// Creates a new <see cref="TableLayout"/> class instance with the specified controls and adds
    /// the created layout to the <see cref="RootTableLayout"/> as a new row.
    /// </summary>
    /// <param name="controls">The controls for the row.</param>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithRow(params Control[] controls)
    {
        return WithRow(Spacing!.Value, controls);
    }

    /// <summary>
    /// Creates a new <see cref="TableLayout"/> class instance with the specified controls and adds
    /// the created layout to the <see cref="RootTableLayout"/> as a new row.
    /// </summary>
    /// <param name="spacing">The spacing between the controls within the row.</param>
    /// <param name="controls">The controls for the row.</param>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithRow(int spacing, params Control[] controls)
    {
        var tableRow = new TableRow();

        for (int i = 0; i < controls.Length; i++)
        {
            if (i + 1 < controls.Length)
            {
                tableRow.Cells.Add(new TableCell(controls[i]));
                tableRow.Cells.Add(new Panel { Width = spacing, });
            }
            else
            {
                tableRow.Cells.Add(new TableCell(controls[i]));
            }
        }

        RootTableLayout!.Rows.Add(new TableRow(new TableCell(new TableLayout(tableRow))));

        return this;
    }

    /// <summary>
    /// Adds a new empty <see cref="TableRow"/> with no controls into the <see cref="RootTableLayout"/>
    /// with <see cref="TableRow.ScaleHeight"/> set to <c>true</c>.
    /// </summary>
    /// <returns>An instance of this <see cref="FluentTableLayoutBuilder"/> class.</returns>
    public FluentTableLayoutBuilder WithEmptyRow()
    {
        RootTableLayout!.Rows.Add(new TableRow { ScaleHeight = true, });
        return this;
    }

    /// <summary>
    /// Gets the <see cref="RootTableLayout"/> instance value.
    /// </summary>
    /// <returns>The <see cref="RootTableLayout"/> instance.</returns>
    public TableLayout GetTable()
    {
        return RootTableLayout!;
    }
}