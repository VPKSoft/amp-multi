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

namespace amp.Database.QueryHelpers;

/// <summary>
/// Event arguments for the <see cref="EventArgs"/> class to report progress changes.
/// Implements the <see cref="System" />
/// </summary>
/// <seealso cref="System" />
public class QueryProgressChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the total task count to query is divided into.
    /// </summary>
    /// <value>The total count.</value>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current count of completed tasks of the query.
    /// </summary>
    /// <value>The current count.</value>
    public int CurrentCount { get; init; }

    /// <summary>
    /// Gets or sets sequential index of the task which is the creation order of the divided tasks.
    /// </summary>
    /// <value>The sequential index of the task.</value>
    public int TaskIndex { get; init; }

    /// <summary>
    /// Gets the percentage of <see cref="TotalCount"/> compared to <see cref="CurrentCount"/>.
    /// </summary>
    /// <value>The current percentage.</value>
    public double CurrentPercentage
    {
        get
        {
            if (TotalCount == 0)
            {
                return 0;
            }

            return (double)CurrentCount / TotalCount * 100.0;
        }
    }
}