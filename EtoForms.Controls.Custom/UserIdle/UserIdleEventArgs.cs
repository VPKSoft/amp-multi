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

namespace EtoForms.Controls.Custom.UserIdle;

/// <summary>
/// A class used with an event to indicate user inactivity.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class UserIdleEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdleEventArgs"/> class.
    /// </summary>
    /// <param name="isUserIdle">if set to <c>true</c> the user is currently idle.</param>
    public UserIdleEventArgs(bool isUserIdle)
    {
        IsUserIdle = isUserIdle;
    }

    /// <summary>
    /// Gets a value indicating whether the user is currently idle.
    /// </summary>
    /// <value><c>true</c> if the user is currently idle; otherwise, <c>false</c>.</value>
    private bool IsUserIdle { get; }
}