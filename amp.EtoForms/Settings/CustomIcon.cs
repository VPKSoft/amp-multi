﻿#region License
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

namespace amp.EtoForms.Settings;

/// <summary>
/// A class for the <see cref="CustomIcons"/> to save icon SVG data.
/// </summary>
public class CustomIcon
{
    /// <summary>
    /// Gets or sets the icon data bytes.
    /// </summary>
    /// <value>The icon data.</value>
    public byte[] IconData { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Gets or sets the optional icon color.
    /// </summary>
    /// <value>The icon color.</value>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to preserve original SVG color information.
    /// </summary>
    /// <value><c>true</c> if to preserve original SVG color information; otherwise, <c>false</c>.</value>
    public bool PreserveOriginalColor { get; set; }

    /// <summary>
    /// Gets or sets the width of the icon.
    /// </summary>
    /// <value>The width of the icon.</value>
    public int IconWidth { get; set; }

    /// <summary>
    /// Gets or sets the icon height.
    /// </summary>
    /// <value>The icon height.</value>
    public int IconHeight { get; set; }
}