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

using amp.EtoForms.Utilities.SvgColorization;
using Eto.Drawing;
using Eto.Forms;

namespace amp.EtoForms.Utilities;

/// <summary>
/// Some helper methods for Eto.Forms.
/// </summary>
public class EtoHelpers
{
    /// <summary>
    /// Set an icon from specified bytes to a specified <see cref="Form"/>.
    /// </summary>
    /// <param name="form">The form to set the icon for.</param>
    /// <param name="iconData">The icon data.</param>
    public static void SetIcon(Window form, byte[] iconData)
    {
        // Set the icon for the form.
        try
        {
            using var memoryStream = new MemoryStream(iconData);
            form.Icon = new Icon(memoryStream);
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(IconLoadErrorMessage, ex.Message), ErrorDialogTitle,
                MessageBoxButtons.OK, MessageBoxType.Error);
        }
    }

    /// <summary>
    /// Gets or sets a message to display if an icon loading fails.
    /// </summary>
    public static string IconLoadErrorMessage { get; set; } = "Failed to load icon with message: '{0}.'";

    /// <summary>
    /// A message for an error dialog title.
    /// </summary>
    public static string ErrorDialogTitle { get; set; } = "Error";

    /// <summary>
    /// Creates a new <see cref="TableLayout"/> containing the specified control and a label with specified text.
    /// </summary>
    /// <param name="text">The text for the label.</param>
    /// <param name="control">The control.</param>
    /// <param name="padding">The padding to use.</param>
    /// <returns>A new instance to the <see cref="TableLayout"/> control.</returns>
    public static TableLayout LabelWrap(string text, Control control, int padding = 5)
    {
        return new TableLayout(new TableRow(new TableCell(PaddingBottomWrap(new Label { Text = text, }), true)),
                new TableRow(new TableCell(control, true)))
        { Padding = new Padding(padding), };
    }

    /// <summary>
    /// Creates a new <see cref="Panel"/> control and contains the specified control within
    /// the panel using the specified padding value as the bottom padding.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="padding">The padding to use.</param>
    /// <returns>A new instance to the <see cref="Panel"/> control.</returns>
    public static Panel PaddingBottomWrap(Control control, int padding = 5)
    {
        return new Panel { Content = control, Padding = new Padding(0, 0, 0, padding), };
    }

    /// <summary>
    /// Creates a new <see cref="Panel"/> control and contains the specified control within
    /// the panel using the specified padding.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="padding">The padding to use.</param>
    /// <returns>A new instance to the <see cref="Panel"/> control.</returns>
    public static Panel PaddingWrap(Control control, int padding = 5)
    {
        return new Panel { Content = control, Padding = new Padding(padding), };
    }

    /// <summary>
    /// Create a <see cref="TableRow"/> with a <see cref="TableLayout"/> containing one row with the specified control, a label and a button.
    /// </summary>
    /// <param name="labelText">The label text.</param>
    /// <param name="buttonText">The button text.</param>
    /// <param name="control">The control to use inside the <see cref="TableLayout"/>.</param>
    /// <param name="clickHandler">The click handler for the <see cref="Button"/>.</param>
    /// <param name="space">A space between the specified control and the button.</param>
    /// <returns>A new instance to a <see cref="TableRow"/> control.</returns>
    public static TableRow LabelWrapperWithButton(string labelText, string buttonText, Control control,
        EventHandler<EventArgs> clickHandler, int space = 5)
    {
        return new TableRow(
            LabelWrap(labelText,
                new TableLayout
                {
                    Rows =
                    {
                        new TableRow
                        {
                            Cells =
                            {
                                new TableCell(control, true),
                                new TableCell(new Panel { Width = space, }),
                                new TableCell(new Button(clickHandler) { Text = buttonText, }),
                            },
                        },
                    },
                }, space)
        );
    }

    /// <summary>
    /// Create a <see cref="TableRow"/>  with a <see cref="TableLayout"/> containing one row with the specified control, a label and a button with scaled SVG image.
    /// </summary>
    /// <param name="labelText">The label text.</param>
    /// <param name="buttonText">The button text.</param>
    /// <param name="control">The control to use inside the <see cref="TableLayout"/>.</param>
    /// <param name="clickHandler">The click handler for the <see cref="Button"/>.</param>
    /// <param name="svgImageBytes">The SVG image data in a byte array.</param>
    /// <param name="buttonColor">The <see cref="Color"/> for the SVG image.</param>
    /// <param name="imagePadding">The amount of pixels the image should be smaller than the button height.</param>
    /// <param name="space">A space between the specified control and the button.</param>
    /// <returns>A new instance to a <see cref="TableRow"/> control.</returns>
    public static TableRow LabelWrapperWithButton(string labelText, string? buttonText, Control control,
        EventHandler<EventArgs> clickHandler, byte[] svgImageBytes, Color buttonColor, int imagePadding = 6, int space = 5)
    {
        return new TableRow(
            LabelWrap(labelText,
                new TableLayout
                {
                    Rows =
                    {
                        new TableRow
                        {
                            Cells =
                            {
                                new TableCell(control, true),
                                new TableCell(new Panel { Width = space, }),
                                new TableCell(CreateImageButton(SvgColorize.FromBytes(svgImageBytes), buttonColor, imagePadding, clickHandler)),
                            },
                        },
                    },
                }, space)
        );
    }

    /// <summary>
    /// Wraps multiple controls under a single label.
    /// </summary>
    /// <param name="labelText">The label text.</param>
    /// <param name="controls">The controls to wrap under a single <see cref="Label"/>.</param>
    /// <param name="padding">The padding to use.</param>
    /// <param name="spacing">A space to use with the controls. A <see cref="Panel"/> controls are used to define the spacing.</param>
    /// <returns>A new instance to a <see cref="TableRow"/> control.</returns>
    public static TableRow LabelWrapperWithControls(string labelText, int padding, int spacing, params Control[] controls)
    {
        var result = new TableRow();

        for (int i = 0; i < controls.Length; i++)
        {
            if (i + 1 < controls.Length)
            {
                result.Cells.Add(new TableCell(controls[i]));
                result.Cells.Add(new Panel { Width = spacing, });
            }
            else
            {
                result.Cells.Add(new TableCell(controls[i]));
            }
        }

        result.Cells.Add(new TableCell(new Panel()) { ScaleWidth = true, });

        return new TableRow(new TableLayout(
            new TableRow(new TableCell(PaddingBottomWrap(new Label { Text = labelText, }))),
            new TableRow(new TableLayout(result)))
        { Padding = new Padding(padding), });
    }

    /// <summary>
    /// Gets or sets the resize loop limit for the <see cref="CreateImageButton"/> method as some layouts
    /// may cause the button resize pixel by pixel causing heavy calculation loop.
    /// </summary>
    /// <value>The resize loop limit.</value>
    public static int ResizeLoopLimit { get; set; } = 15;

    /// <summary>
    /// Wraps the specified control in a <see cref="TableLayout"/> with two columns preventing the control's height to scale beyond is minimum height.
    /// </summary>
    /// <param name="control">The control to wrap.</param>
    /// <param name="upperCell">A value indicating whether to put the control in the upper or the lower cell of the internal <seealso cref="TableLayout"/>.</param>
    /// <returns>A new table cell containing the control.</returns>
    public static TableCell HeightLimitWrap(Control control, bool upperCell)
    {
        return upperCell
            ? new TableCell(new TableLayout { Rows = { control, new TableRow { ScaleHeight = true, }, }, })
            : new TableCell(new TableLayout { Rows = { new TableRow { ScaleHeight = true, }, control, }, });
    }

    /// <summary>
    /// Wraps the specified control in a <see cref="TableLayout"/> with two columns preventing the control's width to scale beyond is minimum width.
    /// </summary>
    /// <param name="control">The control to wrap.</param>
    /// <param name="leftCell">A value indicating whether to put the control in the left or the right cell of the internal <seealso cref="TableLayout"/>.</param>
    /// <returns>A new table cell containing the control.</returns>
    public static TableCell WidthLimitWrap(Control control, bool leftCell)
    {
        return leftCell
            ? new TableCell(new TableLayout { Rows = { new TableRow(control, new TableCell { ScaleWidth = true, }), }, })
            : new TableCell(new TableLayout { Rows = { new TableRow(new TableCell { ScaleWidth = true, }, control), }, });
    }

    /// <summary>
    /// Wraps the specified controls into a new <see cref="TableLayout"/> and returns the contents as a new <see cref="TableRow"/>.
    /// </summary>
    /// <param name="addScaleWidthCell">A value indicating whether to add a maximum width scaling cell to the last cell of the row.</param>
    /// <param name="controls">The controls to wrap.</param>
    /// <returns>A new instance to the <see cref="TableRow"/> class.</returns>
    public static TableRow TableWrap(bool addScaleWidthCell, params Control[] controls)
    {
        var row = new TableRow();

        foreach (var control in controls)
        {
            row.Cells.Add(control);
        }

        if (addScaleWidthCell)
        {
            row.Cells.Add(new TableCell { ScaleWidth = true, });
        }

        return new TableRow(new TableLayout(row, new TableRow { ScaleHeight = true, }));
    }

    /// <summary>
    /// Creates a vertical <see cref="TableLayout"/> from the specified controls.
    /// </summary>
    /// <param name="addScaleHeightCell">if set to <c>true</c> add height scaling cell.</param>
    /// <param name="controls">The controls for the table row.</param>
    /// <returns>Am instance to the <see cref="TableLayout"/> class.</returns>
    public static TableLayout TableWrapVertical(bool addScaleHeightCell, params Control[] controls)
    {
        var layOut = new TableLayout();

        foreach (var control in controls)
        {
            layOut.Rows.Add(control);
        }

        if (addScaleHeightCell)
        {
            layOut.Rows.Add(new TableRow { ScaleHeight = addScaleHeightCell, });
        }

        return layOut;
    }


    /// <summary>
    /// Generates an image from the specified SVG image data with specified color.
    /// </summary>
    /// <param name="svgColor">The color for the SVG.</param>
    /// <param name="imageData">The SVG image data.</param>
    /// <param name="size">The size for the image.</param>
    /// <returns>A <see cref="Image"/> class instance with the SVG colorized and sized to the specified size.</returns>
    public static Image ImageFromSvg(Color svgColor, byte[] imageData, Size size)
    {
        var color = new SvgColor(svgColor.Rb, svgColor.Gb, svgColor.Bb);
        var svgData = SvgColorize.FromBytes(imageData)
            .ColorizeElementsFill(SvgElement.All, color)
            .ColorizeElementsStroke(SvgElement.All, color);

        return SvgToImage.ImageFromSvg(svgData.ToBytes(), size);
    }

    /// <summary>
    /// Creates a new instance of a <see cref="Button"/> control with auto-scaling SVG image.
    /// </summary>
    /// <param name="svgColorize">An instance of the <see cref="SvgColorize"/> class containing the SVG data..</param>
    /// <param name="svgColor">The <see cref="Color"/> for the SVG image color.</param>
    /// <param name="imagePadding">The amount of pixels the image should be smaller than the button height.</param>
    /// <param name="clickHandler">The click handler for the <see cref="Button"/>.</param>
    /// <param name="buttonText">The text for the button.</param>
    /// <param name="customPosition">An optional custom image position.</param>
    /// <returns>A new instance to a <see cref="Button"/> control.</returns>
    public static Button CreateImageButton(SvgColorize svgColorize, Color svgColor, int imagePadding, EventHandler<EventArgs> clickHandler, string? buttonText = null, ButtonImagePosition? customPosition = null)
    {
        var button = new Button(clickHandler);
        button.ImagePosition = buttonText == null
            ? customPosition ?? ButtonImagePosition.Below
            : customPosition ?? ButtonImagePosition.Left;

        bool allowImageDraw = false;

        button.Shown += (_, _) =>
        {
            allowImageDraw = true;
        };

        button.Text = buttonText;

        var sizeWh = Math.Min(button.Width, button.Height) - imagePadding;

        var color = new SvgColor(svgColor.Rb, svgColor.Gb, svgColor.Bb);
        var svgData = svgColorize
            .ColorizeElementsFill(SvgElement.All, color)
            .ColorizeElementsStroke(SvgElement.All, color);
        button.Image = SvgToImage.ImageFromSvg(svgData.ToBytes(), new Size(16, 16));

        var resizeCount = 0;

        button.SizeChanged += delegate
        {
            var newSize = Math.Min(button.Width, button.Height) - imagePadding;
            if (!allowImageDraw || sizeWh == newSize || newSize < 1 || resizeCount >= ResizeLoopLimit)
            {
                return;
            }

            resizeCount++;

            sizeWh = newSize;

            color = new SvgColor(svgColor.Rb, svgColor.Gb, svgColor.Bb);
            svgData = svgColorize
                .ColorizeElementsFill(SvgElement.All, color)
                .ColorizeElementsStroke(SvgElement.All, color);
            button.Image = SvgToImage.ImageFromSvg(svgData.ToBytes(), new Size(sizeWh, sizeWh));
        };

        return button;
    }
}