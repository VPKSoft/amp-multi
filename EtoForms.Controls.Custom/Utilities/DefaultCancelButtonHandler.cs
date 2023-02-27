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
using Eto.Forms;

namespace EtoForms.Controls.Custom.Utilities;

/// <summary>
/// A class to perform default actions with cancel and default <see cref="Button"/>s when Escape or Enter key is pressed.
/// Implements the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public class DefaultCancelButtonHandler : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCancelButtonHandler"/> class.
    /// </summary>
    /// <param name="window">The window to attach the events into.</param>
    public DefaultCancelButtonHandler(Window window)
    {
        this.window = window;
        window.KeyDown += Window_KeyDown;

        foreach (var control in window.Children)
        {
            control.KeyDown += Window_KeyDown;
        }
    }

    private void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Keys.Escape when e.Modifiers == Keys.None && cancelButton?.Enabled == true:
                cancelButton.PerformClick();
                e.Handled = true;
                return;
            case Keys.Enter when e.Modifiers == Keys.None && defaultButton?.Enabled == true:
                defaultButton.PerformClick();
                e.Handled = true;
                break;
        }
    }

    /// <summary>
    /// Returns a new instance of the <see cref="DefaultCancelButtonHandler"/> class.
    /// </summary>
    /// <param name="window">The window to attach the events into.</param>
    /// <returns>A new instance to the <see cref="DefaultCancelButtonHandler"/>.</returns>
    public static DefaultCancelButtonHandler WithWindow(Window window)
    {
        return new DefaultCancelButtonHandler(window);
    }

    /// <summary>
    /// Returns this instance with the cancel button defined.
    /// </summary>
    /// <param name="button">The cancel button.</param>
    /// <returns>An instance to <see cref="DefaultCancelButtonHandler"/>.</returns>
    public DefaultCancelButtonHandler WithCancelButton(Button button)
    {
        cancelButton = button;
        return this;
    }

    /// <summary>
    /// Returns this instance with the cancel and default buttons defined as the same button.
    /// </summary>
    /// <param name="button">The button for cancel and default actions.</param>
    /// <returns>An instance to <see cref="DefaultCancelButtonHandler"/>.</returns>
    public DefaultCancelButtonHandler WithBoth(Button button)
    {
        return WithCancelButton(button).WithDefaultButton(button);
    }

    /// <summary>
    /// Returns this instance with the default button defined.
    /// </summary>
    /// <param name="button">The default button.</param>
    /// <returns>An instance to <see cref="DefaultCancelButtonHandler"/>.</returns>
    public DefaultCancelButtonHandler WithDefaultButton(Button button)
    {
        defaultButton = button;
        return this;
    }

    private readonly Window window;
    private Button? cancelButton;
    private Button? defaultButton;

    /// <inheritdoc />
    public void Dispose()
    {
        try
        {
            window.KeyDown -= Window_KeyDown;

            foreach (var control in window.Children)
            {
                control.KeyDown -= Window_KeyDown;
            }
        }
        catch
        {
            // Dispose failed, some of the controls might already been disposed of.
        }
    }
}