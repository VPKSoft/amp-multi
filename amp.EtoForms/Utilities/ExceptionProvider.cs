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

using VPKSoft.Utils.Common.EventArgs;
using VPKSoft.Utils.Common.Interfaces;

namespace amp.EtoForms.Utilities;
internal class ExceptionProvider : IExceptionReporter
{
    /// <summary>
    /// Prevents a default instance of the <see cref="ExceptionProvider"/> class from being created.
    /// </summary>
    private ExceptionProvider()
    {
    }

    private static ExceptionProvider? instance;

    /// <summary>
    /// Gets the singleton instance of the <see cref="ExceptionProvider"/> class.
    /// </summary>
    /// <value>The singleton instance of the <see cref="ExceptionProvider"/> class.</value>
    public static ExceptionProvider Instance
    {
        get
        {
            instance ??= new ExceptionProvider();
            return instance;
        }
    }

    /// <inheritdoc cref="IExceptionReporter.ExceptionOccurred"/>
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    /// <inheritdoc cref="IExceptionReporter.RaiseExceptionOccurred"/>
    public void RaiseExceptionOccurred(Exception exception, string @class, string method)
    {
        ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(exception, @class, method));
    }
}