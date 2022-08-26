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

namespace VPKSoft.Utils.Common.ExtensionClasses;

/// <summary>
/// An extension utility class for the <see cref="Thread"/> class.
/// </summary>
public static class ThreadJoinAbortTimeout
{
    /// <summary>
    /// Tries to join the specified <see cref="Thread"/> in specified time interval and if the join fails within the interval the thread is aborted.
    /// </summary>
    /// <param name="thread">The thread to try to join.</param>
    /// <param name="waitAmount">The total amount of time in milliseconds to wait for the thread to join.</param>
    /// <param name="waitStep">The waiting interval stepping.</param>
    public static void TryJoin(this Thread thread, int waitAmount, int waitStep = 50)
    {
        var waitedAmount = 0;
        bool joined;
        while (!(joined = thread.Join(50)) && waitedAmount < waitAmount)
        {
            waitedAmount += waitStep;
        }

        if (!joined)
        {
            thread.Interrupt();
        }
    }
}