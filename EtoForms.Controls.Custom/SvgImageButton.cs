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
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A button with SVG image.
/// Implements the <see cref="Button" />
/// </summary>
/// <seealso cref="Button" />
public class SvgImageButton : Button
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgImageButton"/> class.
    /// </summary>
    /// <param name="click">Delegate to handle when the button is clicked.</param>
    /// <remarks>This is a convenience constructor to set up the click event.</remarks>
    public SvgImageButton(EventHandler<EventArgs> click) : base(click)
    {
    }

    private byte[]? image = Array.Empty<byte>();
    private Size imageSize;
    private Color? solidImageColor;
    private Color disabledImageColor = Color.Parse("#B6BCB6");
    private bool skipSizeSet;

    private void SetImage()
    {
        var color = Enabled ? solidImageColor : disabledImageColor;

        if (image != null)
        {
            base.Image = color == null
                ? SvgToImage.ImageFromSvg(image, imageSize)
                : EtoHelpers.ImageFromSvg(color.Value, image, imageSize);
        }
        else
        {
            base.Image = null;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:Eto.Forms.Control" /> (or its children) are enabled and accept user input.
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    /// <remarks>Typically when a control is disabled, the user cannot do anything with the control or any of its children.
    /// Including for example, selecting text in a text control.
    /// Certain controls can have a 'Read Only' mode, such as <see cref="P:Eto.Forms.TextBox.ReadOnly" /> which allow the user to
    /// select text, but not change its contents.</remarks>
    public override bool Enabled
    {
        get => base.Enabled;

        set
        {
            if (value != base.Enabled)
            {
                base.Enabled = value;
                SetImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the image when the <see cref="SvgImageButton"/> is not <see cref="Enabled"/>.
    /// </summary>
    /// <value>The color of the SVG image when the button is disabled.</value>
    /// <remarks>The default value is #B6BCB6.</remarks>
    public Color DisabledImageColor
    {
        get => disabledImageColor;

        set
        {
            if (value != disabledImageColor)
            {
                disabledImageColor = value;
                SetImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the solid color for the SVG image.
    /// </summary>
    /// <value>The solid color for the SVG image.</value>
    /// <remarks>A null value causes the SVG image to be rendered with its original colors.</remarks>
    public Color? SolidImageColor
    {
        get => solidImageColor;

        set
        {
            if (value != solidImageColor)
            {
                solidImageColor = value;
                SetImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the size for the SVG image.
    /// </summary>
    /// <value>The size for the image SVG image.</value>
    public Size ImageSize
    {
        get => imageSize;

        set
        {
            if (imageSize != value)
            {
                imageSize = value;
                if (skipSizeSet)
                {
                    return;
                }

                skipSizeSet = true;
                Size = value;
                SetImage();
                skipSizeSet = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the size of the control. Use -1 to specify auto sizing for either the width and/or height.
    /// </summary>
    /// <value>The size of the control.</value>
    public override Size Size
    {
        get => base.Size;

        set
        {
            if (value != base.Size)
            {
                base.Size = value;
                if (skipSizeSet)
                {
                    return;
                }

                skipSizeSet = true;
                ImageSize = value;
                SetImage();
                skipSizeSet = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the SVG image data in bytes.
    /// </summary>
    /// <value>The SVG image to display-</value>
    public new byte[]? Image
    {
        get => image;

        set
        {
            if (image != value)
            {
                image = value;
                SetImage();
            }
        }
    }
}