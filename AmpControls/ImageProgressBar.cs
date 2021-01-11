#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AmpControls
{
    /// <summary>
    /// A progress bar control using an image to indicate progress.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public sealed partial class ImageProgressBar : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageProgressBar"/> class.
        /// </summary>
        public ImageProgressBar()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PropertyChanged += ImageProgressBar_PropertyChanged;
            Disposed += ImageProgressBar_Disposed;
        }

        private void ImageProgressBar_Disposed(object sender, EventArgs e)
        {
            PropertyChanged -= ImageProgressBar_PropertyChanged;
            Disposed -= ImageProgressBar_Disposed;
        }

        private void ImageProgressBar_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        [Category("Behaviour")]
        [Description("Gets or sets the current value of the progress bar.")]
        public int Value { get; set; } = 0;

        /// <summary>
        /// Gets or sets the minimum value of the progress bar.
        /// </summary>
        [Category("Behaviour")]
        [Description("Gets or sets the minimum value of the progress bar.")]
        public int Minimum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value of the progress bar.
        /// </summary>
        [Category("Behaviour")]
        [Description("Gets or sets the maximum value of the progress bar.")]
        public int Maximum { get; set; } = 100;

        /// <summary>
        /// Gets or sets the progress bar image.
        /// </summary>
        [Category("Appearance")]
        public Image ProgressBarImage { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
#pragma warning disable CS0067
        // ReSharper disable once CommentTypo
        public event PropertyChangedEventHandler PropertyChanged; // warning disabled as the Fody injects the properties to notify of them self..
#pragma warning restore CS0067

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Value >= Minimum && Value <= Maximum && ProgressBarImage != null)
            {
                int adjustValue = Minimum + Value;
                int divider = Maximum - Minimum;
                int paintWidth = adjustValue == 0 || divider == 0 ? 0 : Width * adjustValue / divider;
                for (int i = 0; i < paintWidth; i++)
                {
                    e.Graphics.DrawImage(ProgressBarImage, new Rectangle(i, 0, 1, Height), new Rectangle(0, 0, 1, Height), GraphicsUnit.Pixel);
                }
            }
            else if (Value > Maximum && ProgressBarImage != null)
            {
                for (int i = 0; i < Width; i++)
                {
                    e.Graphics.DrawImage(ProgressBarImage, new Rectangle(i, 0, 1, Height), new Rectangle(0, 0, 1, Height), GraphicsUnit.Pixel);
                }
            }
        }
    }
}
