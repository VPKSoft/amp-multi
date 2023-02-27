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

using amp.Shared.Extensions;
using amp.Shared.Interfaces;
using ATL;
using Eto.Drawing;
using Eto.Forms;

namespace amp.EtoForms.Forms;

/// <summary>
/// A form to display the music track image.
/// Implements the <see cref="Form" />
/// </summary>
/// <seealso cref="Form" />
public class FormAlbumImage : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormAlbumImage"/> class.
    /// </summary>
    public FormAlbumImage()
    {
        WindowStyle = WindowStyle.None;
        base.Size = new Size(330, 330);
        ShowInTaskbar = false;
        Resizable = false;
        Content = new Panel
        {
            Content = imageView,
            Padding = 20,
            Size = new Size(300, 300),
        };
        Closing += FormAlbumImage_Closing;
    }

    internal void DisposeClose()
    {
        allowClose = true;
        Close();
    }

    private void FormAlbumImage_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (allowClose)
        {
            return;
        }

        e.Cancel = true;
        Visible = false;
    }

    /// <summary>
    /// Repositions the form related to the main form.
    /// </summary>
    /// <param name="main">The main form instance.</param>
    public void Reposition(FormMain main)
    {
        Location = new Point(main.Location.X + main.Width, main.Location.Y + 200);

        if (internalVisible && !Visible)
        {
            if (main.WindowState != WindowState.Minimized && main.Visible)
            {
                Show();
            }
        }

        if (Visible)
        {
            BringToFront();
        }
    }

    private readonly ImageView imageView = new();

    /// <summary>
    /// Shows the form next to the specified main form.
    /// </summary>
    /// <param name="main">The main form.</param>
    /// <param name="albumTrack">The album track to get the image for.</param>
    public void Show<TAudioTrack, TAlbum>(FormMain main, IAlbumTrack<TAudioTrack, TAlbum> albumTrack) where TAudioTrack : IAudioTrack where TAlbum : IAlbum
    {
        internalVisible = false;

        if (albumTrack.AudioTrack != null)
        {
            Show(main, albumTrack.AudioTrack);
        }
        else
        {
            if (Visible && !AlwaysVisible)
            {
                Close();
            }
            else if (AlwaysVisible)
            {
                imageView.Image = null;
            }
            internalVisible = Visible;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the form should be always visible. E.g. even when there is no track image.
    /// </summary>
    /// <value><c>true</c> if the form should be always visible; otherwise, <c>false</c>.</value>
    public bool AlwaysVisible { get; set; } = true;

    /// <summary>
    /// Shows the form next to the specified main form.
    /// </summary>
    /// <param name="main">The main form.</param>
    /// <param name="audioTrack">The track to get the image for.</param>
    public void Show(FormMain main, IAudioTrack audioTrack)
    {
        internalVisible = false;
        Location = new Point(main.Location.X + main.Width, main.Location.Y + 200);
        Globals.LoggerSafeInvoke(() =>
        {
            var track = new Track(audioTrack.FileNameFull());
            if (audioTrack.TrackImageData?.Length > 0)
            {
                using var stream = new MemoryStream(audioTrack.TrackImageData);

                var bitmap = new Bitmap(stream);
                imageView.Image = bitmap;

                if (main.WindowState != WindowState.Minimized && main.Visible)
                {
                    Show();
                }

                internalVisible = true;
            }
            else if (track.EmbeddedPictures.Any())
            {
                var picture = track.EmbeddedPictures.First();
                using var stream = new MemoryStream(picture.PictureData);

                var bitmap = new Bitmap(stream);
                imageView.Image = bitmap;

                if (main.WindowState != WindowState.Minimized && main.Visible)
                {
                    Show();
                }

                internalVisible = true;
            }
            else
            {
                if (Visible && !AlwaysVisible)
                {
                    Close();
                }
                else if (AlwaysVisible)
                {
                    imageView.Image = null;
                }
                internalVisible = Visible;
            }
        });
    }

    private bool internalVisible;
    private bool allowClose;
}