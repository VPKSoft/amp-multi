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

namespace amp.UtilityClasses.Threads
{
    /// <summary>
    /// A class to deal with WinForms thread safety.
    /// </summary>
    public static class ThreadSafeInvoker
    {
        /// <summary>
        /// Invokes the specified action thread safely.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="action">The action to invoke thread safely.</param>
        public static void Invoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(() =>
                {
                    action();
                }));
            }
            else
            {
                action(); 
            }
        }

        /// <summary>
        /// Invokes the specified action thread safely.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="action">The action to invoke thread safely.</param>
        /// <param name="value">The value for the action parameter</param>
        public static void Invoke<T>(this Control control, Action<T> action, T value)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(() =>
                {
                    action(value);
                }));
            }
            else
            {
                action(value); 
            }
        }

        /// <summary>
        /// Invokes the specified function thread safely.
        /// </summary>
        /// <typeparam name="TResult">The return value of the method that the <see cref="Func{TResult}"/> delegate encapsulates.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="func">The action to invoke thread safely.</param>
        /// <returns>TResult.</returns>
        public static TResult Invoke<TResult>(this Control control, Func<TResult> func)
        {
            var result = default(TResult);
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(() =>
                {
                    result = func();
                }));
            }
            else
            {
                result = func();
            }

            return result;
        }

        /// <summary>
        /// Invokes the specified function thread safely.
        /// </summary>
        /// <typeparam name="TResult">The return value of the method that the <see cref="Func{T, TResult}"/> delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the method that the <see cref="Func{T, TResult}"/> delegate encapsulates.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="func">The action to invoke thread safely.</param>
        /// <param name="value">The value for the function parameter</param>
        /// <returns>TResult.</returns>
        public static TResult Invoke<T, TResult>(this Control control, Func<T, TResult> func, T value)
        {
            var result = default(TResult);
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(() =>
                {
                    result = func(value);
                }));
            }
            else
            {
                result = func(value);
            }

            return result;
        }
    }
}
