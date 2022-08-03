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

using Eto.Drawing;
using Eto.Forms;
using VPKSoft.ApplicationSettingsJson;

namespace EtoForms.FormPositions;

/// <summary>
/// A class to save the form position and size to file.
/// Implements the <see cref="ApplicationJsonSettings" />
/// </summary>
/// <seealso cref="ApplicationJsonSettings" />
public class FormSaveLoadPosition : ApplicationJsonSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormSaveLoadPosition"/> class.
    /// </summary>
    public FormSaveLoadPosition()
    {
        positionFileName = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormSaveLoadPosition"/> class.
    /// </summary>
    /// <param name="window">The window which position to save or load.</param>
    /// <param name="fileName">Name of the file to save the window position.</param>
    public FormSaveLoadPosition(Window window, string fileName)
    {
        this.window = window;
        positionFileName = fileName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormSaveLoadPosition"/> class.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <exception cref="InvalidOperationException">The save path must be set if no file name is provided.</exception>
    public FormSaveLoadPosition(Window window)
    {
        _ = SavePath ?? throw new InvalidOperationException("The save path must be set if no file name is provided.");
        this.window = window;
        var formFileName = window.GetType().Name;
        foreach (var nameChar in Path.GetInvalidFileNameChars())
        {
            formFileName = formFileName.Replace(nameChar.ToString(), string.Empty);
        }

        positionFileName = Path.Combine(SavePath, formFileName + ".json");
    }

    /// <summary>
    /// Gets or sets the save path where to place the form position save files.
    /// </summary>
    /// <value>The save path.</value>
    public static string? SavePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to load window state also.
    /// </summary>
    /// <value><c>true</c> if to load window state also; otherwise, <c>false</c>.</value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool LoadWindowState { get; set; }

    /// <summary>
    /// Saves the window position, size and state to file.
    /// </summary>
    public void Save()
    {
        Save(positionFileName);
    }

    /// <summary>
    /// Loads the window position, size and state from file.
    /// </summary>
    public void Load()
    {
        Load(positionFileName);
    }

    /// <summary>
    /// Loads application settings from the specified file name.
    /// </summary>
    /// <param name="fileName">Name of the file to load the settings from.</param>
    public override void Load(string fileName)
    {
        base.Load(fileName);
        if (Height > 0 && Width > 0)
        {
            var valid = false;
            foreach (var screen in Screen.Screens)
            {
                if (screen.Bounds.Contains(new PointF(XCoordinate, YCoordinate)))
                {
                    var bottomRectTest = new RectangleF(
                        screen.Bounds.Right - RightBottomInvalidMargin.Width,
                        screen.Bounds.Bottom - RightBottomInvalidMargin.Height,
                        RightBottomInvalidMargin.Width, RightBottomInvalidMargin.Height);
                    if (!bottomRectTest.Contains(new PointF(XCoordinate, YCoordinate)))
                    {
                        valid = true;
                        break;
                    }
                }
            }

            if (valid)
            {
                window!.Location = new Point(XCoordinate, YCoordinate);
                window.Width = Width;
                window.Height = Height;
                if (LoadWindowState)
                {
                    window.WindowState = (WindowState)WindowState;
                }
            }
        }
    }

    /// <summary>
    /// Saves the settings to a specified file name.
    /// </summary>
    /// <param name="fileName">Name of the file to save the settings into.</param>
    public override void Save(string fileName)
    {
        WindowState = (int)window!.WindowState;
        if (WindowState != (int)Eto.Forms.WindowState.Normal)
        {
            XCoordinate = window.RestoreBounds.X;
            YCoordinate = window.RestoreBounds.Y;
            Width = window.RestoreBounds.Width;
            Height = window.RestoreBounds.Height;

        }
        else
        {
            Width = window.Width;
            Height = window.Height;
            XCoordinate = window.Location.X;
            YCoordinate = window.Location.Y;
        }
        base.Save(fileName);
    }

    /// <summary>
    /// Resets the window position to top-left corner of the primary screen with size at lest the <see cref="WindowMinimumSize"/>.
    /// </summary>
    public void ResetPosition()
    {
        var screen = Screen.Screens.FirstOrDefault(f => f.IsPrimary);
        if (screen != null)
        {
            window!.WindowState = Eto.Forms.WindowState.Normal;

            window.Location = new Point(screen.Bounds.TopLeft);
            if (window.Width < WindowMinimumSize.Width)
            {
                window.Width = WindowMinimumSize.Width;
            }

            if (window.Height < WindowMinimumSize.Height)
            {
                window.Height = WindowMinimumSize.Height;
            }
        }
    }

    /// <summary>
    /// Gets or sets the size of the area in right bottom corner of the screen when the where the window position is counted as invalid.
    /// </summary>
    /// <value>The right bottom invalid margin.</value>
    public static Size RightBottomInvalidMargin { get; set; } = new(200, 100);

    /// <summary>
    /// Gets or sets the minimum size of the window.
    /// </summary>
    /// <value>The minimum size of the window.</value>
    public static Size WindowMinimumSize { get; set; } = new(100, 200);

    /// <summary>
    /// Gets or sets the state of the window.
    /// </summary>
    /// <value>The state of the window.</value>
    [Settings]
    // ReSharper disable once MemberCanBePrivate.Global, must be public, because is a ApplicationJsonSettings setting.
    public int WindowState { get; set; }

    /// <summary>
    /// Gets or sets the X-coordinate of the window.
    /// </summary>
    /// <value>The window X-coordinate.</value>
    [Settings]
    // ReSharper disable once MemberCanBePrivate.Global, must be public, because is a ApplicationJsonSettings setting.
    public int XCoordinate { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the window.
    /// </summary>
    /// <value>The window Y-coordinate.</value>
    [Settings]
    // ReSharper disable once MemberCanBePrivate.Global, must be public, because is a ApplicationJsonSettings setting.
    public int YCoordinate { get; set; }

    /// <summary>
    /// Gets or sets the width of the window.
    /// </summary>
    /// <value>The window width.</value>
    [Settings]
    // ReSharper disable once MemberCanBePrivate.Global, must be public, because is a ApplicationJsonSettings setting.
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the window.
    /// </summary>
    /// <value>The window height.</value>
    [Settings]
    // ReSharper disable once MemberCanBePrivate.Global, must be public, because is a ApplicationJsonSettings setting.
    public int Height { get; set; }

    private readonly string positionFileName;
    private readonly Window? window;
}