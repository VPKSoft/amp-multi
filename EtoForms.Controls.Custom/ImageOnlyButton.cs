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
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A button with image only.
/// Implements the <see cref="Drawable" />
/// </summary>
/// <seealso cref="Drawable" />
public class ImageOnlyButton : Drawable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageOnlyButton"/> class.
    /// </summary>
    /// <param name="click">The click event handler.</param>
    /// <param name="svgImageData">The SVG image data.</param>
    public ImageOnlyButton(EventHandler<EventArgs> click, byte[] svgImageData) : this(svgImageData)
    {
        Click += click;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageOnlyButton"/> class.
    /// </summary>
    /// <param name="svgImageData">The SVG image data.</param>
    public ImageOnlyButton(byte[] svgImageData)
    {
        this.svgImageData = svgImageData;
        MouseDown += ImageOnlyButton_MouseDown;
        MouseUp += ImageOnlyButton_MouseUp;
        Paint += ImageOnlyButton_Paint;
    }

    private void ImageOnlyButton_Paint(object? sender, PaintEventArgs e)
    {
        DrawButton(e.Graphics, e.ClipRectangle);
    }

    private void DrawButton(Graphics graphics, RectangleF clipRectangle)
    {
        var drawRectangle = ShowBorder
            ? new RectangleF(new PointF(clipRectangle.X + 1, clipRectangle.Y + 1),
                new SizeF(clipRectangle.Width - 2, clipRectangle.Height - 2))
            : clipRectangle;

        var borderRectangle = new RectangleF(new PointF(clipRectangle.X, clipRectangle.Y),
            new SizeF(clipRectangle.Width - 1, clipRectangle.Height - 1));

        var wh = (int)Math.Min(drawRectangle.Width, drawRectangle.Height);

        if (!previousDrawArea.Equals(drawRectangle) || drawImage?.IsDisposed == true || drawImage == null)
        {
            previousDrawArea = drawRectangle;
            drawImage?.Dispose();
            drawImage = EtoHelpers.ImageFromSvg(base.Enabled ? imageColor : disabledColor, svgImageData, new Size(wh, wh));
        }

        graphics.FillRectangle(BackgroundColor, clipRectangle);

        var imageLocation = new PointF((drawRectangle.Width - wh) / 2 + drawRectangle.X, (drawRectangle.Height - wh) / 2 + drawRectangle.Y);

        graphics.DrawImage(drawImage!, imageLocation);
        graphics.DrawRectangle(BorderColor, borderRectangle);
    }

    /// <summary>
    /// Occurs when the button is clicked.
    /// </summary>
    public event EventHandler<EventArgs>? Click;

    private void ImageOnlyButton_MouseUp(object? sender, MouseEventArgs e)
    {
        if (mouseDown)
        {
            mouseDown = false;
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ImageOnlyButton_MouseDown(object? sender, MouseEventArgs e)
    {
        mouseDown = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button should display border.
    /// </summary>
    /// <value><c>true</c> if the button should display border; otherwise, <c>false</c>.</value>
    public bool ShowBorder
    {
        get => showBorder;

        set
        {
            if (value != showBorder)
            {
                showBorder = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>The color of the background.</value>
    /// <remarks>Note that on some platforms (e.g. Mac), setting the background color of a control can change the performance
    /// characteristics of the control and its children, since it must enable layers to do so.</remarks>
    public new Color BackgroundColor
    {
        get => base.BackgroundColor;

        set
        {
            if (base.BackgroundColor != value)
            {
                base.BackgroundColor = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the image when the button is disabled.
    /// </summary>
    /// <value>The color of the disabled button image.</value>
    public Color DisabledColor
    {
        get => disabledColor;

        set
        {
            if (disabledColor != value)
            {
                disabledColor = value;
                drawImage?.Dispose();
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the image.
    /// </summary>
    /// <value>The color of the image.</value>
    public Color ImageColor
    {
        get => imageColor;

        set
        {
            if (imageColor != value)
            {
                imageColor = value;
                drawImage?.Dispose();
                drawImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the border.
    /// </summary>
    /// <value>The color of the border.</value>
    public Color BorderColor
    {
        get => borderColor;

        set
        {
            if (value != borderColor)
            {
                borderColor = value;
                Invalidate();
            }
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
                drawImage?.Dispose();
                drawImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the SVG image.
    /// </summary>
    /// <value>The SVG image.</value>
    public byte[] SvgImage
    {
        get => svgImageData;

        set
        {
            if (!svgImageData.SequenceEqual(value))
            {
                svgImageData = value;
                drawImage?.Dispose();
                drawImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the default color of the disabled <see cref="ImageOnlyButton"/>'s image.
    /// </summary>
    /// <value>The disabled default color of the image.</value>
    /// <remarks>The default value is #B6BCB6.</remarks>
    public static Color DefaultDisableColor { get; set; } = Color.Parse("#B6BCB6");

    /// <summary>
    /// Gets or sets the default color of the <see cref="ImageOnlyButton"/>'s image.
    /// </summary>
    /// <value>The default color of the image.</value>
    /// <remarks>The default value is #008080.</remarks>
    public static Color DefaultImageColor { get; set; } = Colors.Teal;

    /// <summary>
    /// Gets or sets the default color of the border of the <see cref="ImageOnlyButton"/>.
    /// </summary>
    /// <value>The default color of the border.</value>
    /// <remarks>The default value is #A9A9A9.</remarks>
    public static Color DefaultBorderColor { get; set; } = Colors.DarkGray;

    private bool mouseDown;
    private Image? drawImage;
    private RectangleF previousDrawArea;
    private byte[] svgImageData;
    private Color disabledColor = DefaultDisableColor;
    private Color imageColor = DefaultImageColor;
    private bool showBorder = true;
    private Color borderColor = DefaultBorderColor;
}