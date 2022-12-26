﻿#region License
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

namespace EtoForms.Controls.Custom.EventArguments;

/// <summary>
/// Event arguments for an event where a <see cref="double"/> value has been changed.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class ValueChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the common modifier key is down.
    /// </summary>
    /// <value><c>true</c> if the common modifier key is down; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// On Windows/Linux, this will typically means <see cref="F:Eto.Forms.Keys.Control" />, and on OS X this will be <see cref="F:Eto.Forms.Keys.Application" /> (the command key).
    /// </remarks>
    public bool CommonModifier { get; set; }
}