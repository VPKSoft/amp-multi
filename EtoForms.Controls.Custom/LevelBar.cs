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

namespace EtoForms.Controls.Custom;

/// <summary>
/// A control to indicate a level value in a specific range.
/// Implements the <see cref="Drawable" />
/// </summary>
/// <seealso cref="Drawable" />
public class LevelBar : Drawable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LevelBar"/> class.
    /// </summary>
    public LevelBar()
    {
        Paint += LevelBar_Paint;
        Padding = Padding.Empty;
        BackgroundColor = Colors.Black;
    }

    #region Fields
    private double minimum;
    private double maximum = 100;
    private double currentValue;
    private Color colorStart = Colors.SteelBlue;
    private Color colorEnd = Colors.Navy;
    private bool drawWithGradient = true;
    private Orientation orientation = Orientation.Vertical;
    private bool inverseDraw;

    #endregion

    #region PublicProperties    
    /// <summary>
    /// Gets or sets the orientation of the level bar.
    /// </summary>
    /// <value>The orientation.</value>
    public Orientation Orientation
    {
        get => orientation;
        set
        {
            if (value != orientation)
            {
                orientation = value;
                Invalidate();
            }
        }
    }

    /// <inheritdoc cref="Control.BackgroundColor"/>
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
    /// Gets or sets the start color for the level bar drawing.
    /// </summary>
    /// <value>The start color.</value>
    public Color ColorStart
    {
        get => colorStart;

        set
        {
            if (colorStart != value)
            {
                colorStart = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the end color for the level bar drawing.
    /// </summary>
    /// <value>The end color.</value>
    public Color ColorEnd
    {
        get => colorEnd;

        set
        {
            if (colorEnd != value)
            {
                colorEnd = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to invert the bar drawing.
    /// </summary>
    /// <value><c>true</c> if to invert the bar drawing; otherwise, <c>false</c>.</value>
    public bool InverseDraw
    {
        get => inverseDraw;
        set
        {
            if (value != inverseDraw)
            {
                inverseDraw = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to draw the level bar using gradient from <see cref="ColorStart"/> to <see cref="ColorEnd"/>.
    /// </summary>
    /// <value><c>true</c> if to draw the control using gradient; otherwise, <c>false</c>.</value>
    public bool DrawWithGradient
    {
        get => drawWithGradient;

        set
        {
            if (drawWithGradient != value)
            {
                drawWithGradient = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the minimum value for the slider.
    /// </summary>
    /// <value>The minimum value for the slider.</value>
    public double Minimum
    {
        get => minimum;

        set
        {
            if (Math.Abs(value - minimum) > Globals.FloatingPointTolerance)
            {
                if (maximum < value)
                {
                    maximum = value + maximum - minimum;
                }

                minimum = value;
                if (minimum < currentValue)
                {
                    currentValue = minimum;
                }

                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum value for the slider.
    /// </summary>
    /// <value>The maximum value for the slider.</value>
    public double Maximum
    {
        get => maximum;

        set
        {
            if (Math.Abs(maximum - value) > Globals.FloatingPointTolerance)
            {
                if (value < minimum)
                {
                    minimum -= maximum - minimum;
                    if (currentValue > maximum)
                    {
                        currentValue = maximum;
                    }

                    currentValue = value;
                }

                maximum = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the current slider position value.
    /// </summary>
    /// <value>The current slider position value.</value>
    public double Value
    {
        get => currentValue;

        set => UpdateValue(value);
    }
    #endregion

    #region PrivateMethods
    private void UpdateValue(double value)
    {
        if (Math.Abs(currentValue - value) > Globals.FloatingPointTolerance)
        {
            if (value < minimum)
            {
                currentValue = minimum;
            }

            if (value > maximum)
            {
                currentValue = maximum;
            }

            currentValue = value;

            Invalidate();
        }
    }

    private void DrawLevelBar(Graphics graphics, RectangleF clipRectangle)
    {
        graphics.FillRectangle(BackgroundColor, clipRectangle);
        Brush brush;
        if (drawWithGradient)
        {
            if (orientation == Orientation.Horizontal)
            {
                brush = new LinearGradientBrush(colorStart, colorEnd, new PointF(0, clipRectangle.MiddleY),
                    new PointF(clipRectangle.Right, clipRectangle.MiddleY));
            }
            else
            {
                brush = new LinearGradientBrush(colorStart, colorEnd,
                    new PointF(clipRectangle.MiddleX, clipRectangle.Bottom),
                    new PointF(clipRectangle.MiddleX, clipRectangle.Top));
            }
        }
        else
        {
            brush = new SolidBrush(colorStart);
        }

        using (brush)
        {
            var size = (float)(currentValue / (maximum - minimum) *
                       (orientation == Orientation.Horizontal ? clipRectangle.Width : clipRectangle.Height));

            graphics.FillRectangle(BackgroundColor, clipRectangle);

            RectangleF fillArea;

            if (inverseDraw)
            {
                fillArea = orientation == Orientation.Horizontal
                    ? new RectangleF(clipRectangle.Right - size, clipRectangle.Top, clipRectangle.Right, clipRectangle.Height)
                    : new RectangleF(0, clipRectangle.Top, clipRectangle.Width, size);
            }
            else
            {
                fillArea = orientation == Orientation.Horizontal
                    ? new RectangleF(clipRectangle.Left, clipRectangle.Top, size, clipRectangle.Height)
                    : new RectangleF(0, clipRectangle.Bottom - size, clipRectangle.Width, size);
            }


            graphics.FillRectangle(brush, fillArea);
        }
    }
    #endregion

    #region InternalEvents
    private void LevelBar_Paint(object? sender, PaintEventArgs e)
    {
        DrawLevelBar(e.Graphics, e.ClipRectangle);
    }
    #endregion
}