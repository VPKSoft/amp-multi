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

namespace EtoForms.Controls.Custom.Utilities;


/// <summary>
/// A dialog to query a value from the user.
/// Implements the <see cref="Dialog{T}" />
/// </summary>
/// <typeparam name="T">The type of the value to query.</typeparam>
/// <seealso cref="Dialog{T}" />
public class DialogQueryValue<T> : Dialog<T?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogQueryValue{T}"/> class.
    /// </summary>
    /// <param name="title">The dialog title.</param>
    /// <param name="valueQueryText">The value query text next to the input control.</param>
    /// <param name="allowEmpty">if set to <c>true</c> an empty value is allowed if the <typeparamref name="T"/> allows it.</param>
    /// <param name="defaultSpacing">The default spacing.</param>
    /// <param name="defaultPadding">The default padding.</param>
    /// <param name="value">The initial value.</param>
    /// <param name="minimumValue">The minimum value for e.g. numeric types.</param>
    /// <param name="maximumValue">The maximum value for e.g. numeric types.</param>
    /// <param name="decimals">The number of allowed decimals for floating point types.</param>
    public DialogQueryValue(string title, string valueQueryText, bool allowEmpty, Size defaultSpacing, Padding defaultPadding, T? value = default, T? minimumValue = default, T? maximumValue = default, int decimals = 0)
    {
        this.valueQueryText = valueQueryText;
        this.allowEmpty = allowEmpty;
        this.defaultSpacing = defaultSpacing;
        this.defaultPadding = defaultPadding;
        this.value = value;
        this.minimumValue = minimumValue;
        this.maximumValue = maximumValue;
        this.decimals = decimals;

        queryControl = new Panel();
        if (typeof(T) == typeof(string))
        {
            var control = new TextBox { Text = value?.ToString(), };
            control.TextChanged += Control_TextChanged;
            if (!allowEmpty)
            {
                control.TextChanged += (_, _) =>
                {
                    if (!allowEmpty)
                    {
                        btnOk.Enabled = !string.IsNullOrWhiteSpace(control.Text);
                    }
                };
            }

            queryControl = control;
        }

        Content = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = valueQueryText,},
                        new TableCell(queryControl, true),
                    },
                },
                new TableRow { ScaleHeight = true,},
            },
            Padding = defaultPadding,
            Spacing = defaultSpacing,
        };

        Title = title;
        MinimumSize = new Size(400, 100);

        btnOk.Click += BtnOk_Click;
        btnCancel.Click += BtnCancel_Click;

        PositiveButtons.Add(btnOk);
        NegativeButtons.Add(btnCancel);

        DefaultButton = btnOk;
        AbortButton = btnCancel;

        Shown += DialogQueryValue_Shown;
    }

    private void DialogQueryValue_Shown(object? sender, EventArgs e)
    {
        queryControl.Focus();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close(default);
    }


    // ReSharper disable NotAccessedField.Local
    private readonly string valueQueryText;
    private readonly bool allowEmpty;
    private readonly Size defaultSpacing;
    private readonly Padding defaultPadding;
    private object? value;
    private readonly T? minimumValue;
    private readonly T? maximumValue;
    private readonly int decimals;

    private Control queryControl;
    // ReSharper restore NotAccessedField.Local

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        Close((T?)value);
    }

    private void Control_TextChanged(object? sender, EventArgs e)
    {
        value = ((TextBox)sender!).Text;
    }

    private readonly Button btnOk = new() { Text = Globals.OkButtonText, Enabled = false, };
    private readonly Button btnCancel = new() { Text = Globals.CancelButtonText, };
}