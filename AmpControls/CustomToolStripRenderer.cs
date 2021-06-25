#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

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

using System.Drawing;
using System.Windows.Forms;

namespace AmpControls
{
    /// <summary>
    /// A class to help change the colors of the tool strip buttons.
    /// Implements the <see cref="System.Windows.Forms.ToolStripProfessionalRenderer" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ToolStripProfessionalRenderer" />
    public class CustomToolStripRenderer: ToolStripProfessionalRenderer
    {
        /// <summary>
        /// Gets or sets the background color of a selected <see cref="ToolStripButton"/>.
        /// </summary>
        /// <value>The background color of a selected <see cref="ToolStripButton"/>.</value>
        public Color ColorSelected { get; set; }

        /// <summary>
        /// Gets or sets the background color of a normal not selected <see cref="ToolStripButton"/>.
        /// </summary>
        /// <value>The background color of a normal not selected <see cref="ToolStripButton"/>.</value>
        public Color ColorNormal { get; set; }

        /// <summary>
        /// Gets or sets the color of a selected <see cref="ToolStripButton"/>'s border.
        /// </summary>
        /// <value>The color of a selected <see cref="ToolStripButton"/>'s border.</value>
        public Color ColorSelectedBorder { get; set; }

        /// <summary>
        /// Gets or sets the color of a checked <see cref="ToolStripButton"/>'s border.
        /// </summary>
        /// <value>The color of a checked <see cref="ToolStripButton"/>'s border.</value>
        public Color ColorCheckedBorder { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripBorder" />  event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using var pen = new Pen(ColorNormal == Color.Empty ? e.ToolStrip.BackColor : ColorNormal);
            e.Graphics.DrawRectangle(pen, e.AffectedBounds);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderButtonBackground" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width, e.Item.Size.Height);
            base.OnRenderButtonBackground(e);
            if (e.Item is ToolStripButton item)
            {
                if (!item.Selected)
                {
                    using var brush = new SolidBrush(ColorNormal == Color.Empty ? e.ToolStrip.BackColor : ColorNormal);
                    e.Graphics.FillRectangle(brush, rectangle);
                }
                else
                {
                    rectangle = new Rectangle(0, 0, item.Size.Width - 1, item.Size.Height - 1);
                    using var brush = new SolidBrush(ColorSelected);
                    using var pen = new Pen(ColorSelectedBorder);

                    e.Graphics.FillRectangle(brush, rectangle);
                    e.Graphics.DrawRectangle(pen, rectangle);
                }

                if (item.Checked)
                {
                    using var pen = new Pen(ColorCheckedBorder);
                    rectangle = new Rectangle(0, 0, item.Size.Width - 1, item.Size.Height - 1);
                    e.Graphics.DrawRectangle(pen, rectangle);
                }
            }
        }
    }
}
