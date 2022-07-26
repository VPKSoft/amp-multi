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

namespace amp.EtoForms.Forms.AdditionalClasses;

/// <summary>
/// Data class for the <see cref="FormLoadProgress{T}"/> tasks.
/// </summary>
internal class TaskData
{
    /// <summary>
    /// Gets or sets the index of the task.
    /// </summary>
    /// <value>The index of the task.</value>
    internal int TaskIndex { get; init; }

    /// <summary>
    /// Gets or sets the size of the task in rows of data from the database.
    /// </summary>
    /// <value>The size of the task.</value>
    internal int TaskSize { get; init; }

    /// <summary>
    /// Gets or sets the progress action to report task completion.
    /// </summary>
    /// <value>The progress action.</value>
    internal Action<int> ProgressAction { get; init; } = delegate { };
}