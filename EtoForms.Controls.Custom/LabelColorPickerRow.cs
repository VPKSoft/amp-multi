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

using System;
using Eto.Drawing;
using Eto.Forms;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A labeled color picker control with clear button.
/// Implements the <see cref="Panel" />.
/// </summary>
/// <seealso cref="Panel" />
/// <typeparam name="T">The type of the optional data attached to this row, <see cref="DataObject"/>.</typeparam>
/// <seealso cref="TableRow" />
public class LabelColorPickerRow<T> : TableRow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelColorPickerRow{T}"/> class.
    /// </summary>
    /// <param name="text">The label text describing the color.</param>
    /// <param name="colorIsNullable">if set to <c>true</c> the color value can be set to <c>null</c>.</param>
    /// <param name="color">The color.</param>
    /// <param name="svgImage">The SVG image.</param>
    /// <param name="buttonSize">Size of the button.</param>
    public LabelColorPickerRow(string text, bool colorIsNullable, Color? color, byte[] svgImage, Size buttonSize)
    {
        label.Text = text;

        colorPicker.Value = color ?? default;

        initialColor = color;

        colorPicker.ValueChanged += ColorPicker_ValueChanged;

        this.buttonSize = buttonSize;

        imageButton = new ImageOnlyButton(svgImage) { Size = this.buttonSize, };


        imageButton.Click += ImageButton_Click;

        Control buttonControl = colorIsNullable ? imageButton : new Panel();

        Cells.Add(label);
        Cells.Add(new TableCell(colorPicker) { ScaleWidth = true, });
        Cells.Add(buttonControl);
    }

    private void ColorPicker_ValueChanged(object? sender, EventArgs e)
    {
        SelectedColor = colorPicker.Value;
    }

    private Color? selectedColor;

    /// <summary>
    /// Gets or sets the color the user selected.
    /// </summary>
    /// <value>The color of the selected.</value>
    public Color? SelectedColor
    {
        get => selectedColor;

        set
        {
            if (selectedColor != value)
            {
                selectedColor = value;
                ColorChanged = initialColor != selectedColor;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the color value has been changed by the user.
    /// </summary>
    /// <value><c>true</c> if the color was changed by the user; otherwise, <c>false</c>.</value>
    public bool ColorChanged { get; private set; }

    private void ImageButton_Click(object? sender, EventArgs e)
    {
        colorPicker.Value = default;
        SelectedColor = null;
    }

    /// <summary>
    /// Gets or sets the data object for this row.
    /// </summary>
    /// <value>The data object for this row.</value>
    public T? DataObject { get; set; }

    private readonly Color? initialColor;
    private readonly Size buttonSize;
    private readonly ColorPicker colorPicker = new();
    private readonly Label label = new();
    private readonly ImageOnlyButton imageButton;
}