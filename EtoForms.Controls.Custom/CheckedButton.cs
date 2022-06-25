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
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.SvgColorization;
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A <see cref="Button"/> control descendant to act as a check button.
/// Implements the <see cref="Eto.Forms.Button" />
/// </summary>
/// <seealso cref="Eto.Forms.Button" />
public class CheckedButton : Button
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedButton"/> class.
    /// </summary>
    public CheckedButton()
    {
        Shown += CheckedButton_Shown;
        SizeChanged += CheckedButton_SizeChanged;
    }

    private void CheckedButton_SizeChanged(object? sender, EventArgs e)
    {
        if (shown)
        {
            PaintBothImages();
        }
    }

    private void CheckedButton_Shown(object? sender, EventArgs e)
    {
        shown = true;
        PaintBothImages();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedButton"/> class.
    /// </summary>
    /// <param name="checkedChange">Delegate to handle when the button <see cref="Checked"/> status is changed.</param>
    /// <remarks>This is a convenience constructor to set up the click event.</remarks>
    public CheckedButton(EventHandler<CheckedChangeEventArguments> checkedChange) : base(OnClick)
    {
        Shown += CheckedButton_Shown;
        SizeChanged += CheckedButton_SizeChanged;
        CheckedChange += delegate { checkedChange.Invoke(this, new CheckedChangeEventArguments { Checked = @checked, }); };
    }

    /// <summary>
    /// Occurs when the value of the  [checked change].
    /// </summary>
    public event EventHandler<CheckedChangeEventArguments>? CheckedChange;


    #region PrivateMethodsAndProperties
    private static void OnClick(object? sender, EventArgs e)
    {
        if (sender == null)
        {
            return;
        }

        var button = (CheckedButton)sender;

        button.Checked = !button.Checked;
    }

    private void PaintCheckedImage()
    {
        var color = new SvgColor(CheckedImageColor.Rb, CheckedImageColor.Gb, CheckedImageColor.Bb);
        if (checkedSvgImage != null)
        {
            checkedImage?.Dispose();
            var svgData = checkedSvgImage
                .ColorizeElementsFill(SvgElement.All, color)
                .ColorizeElementsStroke(SvgElement.All, color);

            checkedImage = SvgToImage.ImageFromSvg(svgData.ToBytes(), new Size(SizeWidthHeight, SizeWidthHeight));
        }

        Image = @checked ? checkedImage : uncheckedImage;
    }

    private void PaintUncheckedImage()
    {
        var color = new SvgColor(UncheckedImageColor.Rb, UncheckedImageColor.Gb, UncheckedImageColor.Bb);
        if (uncheckedSvgImage != null)
        {
            uncheckedImage?.Dispose();
            var svgData = uncheckedSvgImage
                .ColorizeElementsFill(SvgElement.All, color)
                .ColorizeElementsStroke(SvgElement.All, color);

            uncheckedImage = SvgToImage.ImageFromSvg(svgData.ToBytes(), new Size(SizeWidthHeight, SizeWidthHeight));
        }

        Image = @checked ? checkedImage : uncheckedImage;
    }

    private void PaintBothImages()
    {
        PaintUncheckedImage();
        PaintUncheckedImage();
    }

    // Weird button size increasing occurs on GTK if the button image size resizes along with the button size, Thus: Platform.IsGtk --> 16px.
    private int SizeWidthHeight => Math.Min(Width, Height) - ImagePadding < 16 || Platform.IsGtk ? 16 : Math.Min(Width, Height) - ImagePadding;
    #endregion

    private SvgColorize? uncheckedSvgImage;
    private SvgColorize? checkedSvgImage;
    private Image? checkedImage;
    private Image? uncheckedImage;
    private Color checkedImageColor = Colors.SteelBlue;
    private Color uncheckedImageColor = Colors.SteelBlue;
    private int imagePadding = 6;
    private bool @checked;
    private bool shown;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CheckedButton"/> is checked.
    /// </summary>
    /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
    public bool Checked
    {
        get => @checked;

        set
        {
            if (@checked != value)
            {
                @checked = value;
                Image = @checked ? checkedImage : uncheckedImage;
                CheckedChange?.Invoke(this, new CheckedChangeEventArguments { Checked = @checked, });
            }
        }
    }

    /// <summary>
    /// Gets or sets the image padding in pixels compared to the button size.
    /// </summary>
    /// <value>The image padding.</value>
    public int ImagePadding
    {
        get => imagePadding;

        set
        {
            if (value != imagePadding)
            {
                imagePadding = value;
                PaintBothImages();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the checked image.
    /// </summary>
    /// <value>The color of the checked image.</value>
    public Color CheckedImageColor
    {
        get => checkedImageColor;

        set
        {
            if (value != checkedImageColor)
            {
                checkedImageColor = value;
                PaintCheckedImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the unchecked image.
    /// </summary>
    /// <value>The color of the unchecked image.</value>
    public Color UncheckedImageColor
    {
        get => uncheckedImageColor;

        set
        {
            if (value != uncheckedImageColor)
            {
                uncheckedImageColor = value;
                PaintUncheckedImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the checked SVG image.
    /// </summary>
    /// <value>The checked SVG image.</value>
    public SvgColorize? CheckedSvgImage
    {
        get => checkedSvgImage;

        set
        {
            if (value != checkedSvgImage)
            {
                checkedSvgImage = value;
                PaintCheckedImage();
            }
        }
    }

    /// <summary>
    /// Gets or sets the unchecked SVG image.
    /// </summary>
    /// <value>The unchecked SVG image.</value>
    public SvgColorize? UncheckedSvgImage
    {
        get => uncheckedSvgImage;

        set
        {
            if (value != uncheckedSvgImage)
            {
                uncheckedSvgImage = value;
                PaintUncheckedImage();
            }
        }
    }
}