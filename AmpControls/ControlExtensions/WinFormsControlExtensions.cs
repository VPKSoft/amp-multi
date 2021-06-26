#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

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
using System.Windows.Forms;

namespace AmpControls.ControlExtensions
{
    /// <summary>
    /// Extensions for thread-safe WinForms invocation.
    /// </summary>
    public static class WinFormsControlExtensions
    {
        /// <summary>
        /// Invokes the anonymous method.
        /// </summary>
        /// <param name="control">The control invoking the method.</param>
        /// <param name="invokeAction">The invoke action.</param>
        public static void InvokeAnonymous(this Control control, Action invokeAction)
        {
            if (!control.IsHandleCreated || !control.IsDisposed || !control.Disposing)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(invokeAction));
            }
            else
            {
                invokeAction();
            }
        }

        /// <summary>
        /// Invokes the anonymous method with the result value of type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the anonymous method.</typeparam>
        /// <param name="control">The control invoking the method.</param>
        /// <param name="invokeAction">The invoke action.</param>
        /// <returns>A value of <typeparamref name="T"/>.</returns>
        public static T InvokeAnonymous<T>(this Control control, Func<T> invokeAction)
        {
            if (!control.IsHandleCreated || !control.IsDisposed || !control.Disposing)
            {
                return default;
            }

            if (control.InvokeRequired)
            {
                T result = default;
                control.Invoke(new MethodInvoker(() => { result = invokeAction(); }));

                return result;
            }
            else
            {
                return invokeAction();
            }
        }
    }
}
