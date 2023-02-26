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
using System.Threading;
using Eto.Forms;
using VPKSoft.Utils.Common.EventArgs;
using VPKSoft.Utils.Common.ExtensionClasses;
using VPKSoft.Utils.Common.Interfaces;

namespace EtoForms.Controls.Custom.UserIdle;

/// <summary>
/// A class to detect if the user is idle. E.g. not interacting with the application.
/// Implements the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public class UserIdleChecker : IDisposable, IExceptionReporter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdleChecker"/> class.
    /// </summary>
    /// <param name="container">The container control which keyboard and mouse events to listen.</param>
    public UserIdleChecker(Container container)
    {
        this.container = container;
        AttachEventListeners(container);

        foreach (var control in container.Children)
        {
            AttachEventListeners(control);
        }

        observerThread = new Thread(ThreadFunc);
        observerThread.Start();
    }

    /// <summary>
    /// Gets or sets the interval the user must be inactive in seconds before the <see cref="UserIdle"/> event gets raised.
    /// </summary>
    /// <value>The interval for user inactivity in seconds.</value>
    public double UserInactiveInterval
    {
        get
        {
            lock (lockObject)
            {
                return userInactiveInterval;
            }
        }

        set
        {
            lock (lockObject)
            {
                userInactiveInterval = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating the user is currently idle.
    /// </summary>
    /// <value><c>true</c> if the user is currently idle; otherwise, <c>false</c>.</value>
    public bool IsUserIdle
    {
        get => idleEventInvoked;

        set
        {
            if (value != idleEventInvoked)
            {
                idleEventInvoked = value;
                if (!idleEventInvoked)
                {
                    previousActiveTime = DateTime.Now;
                    UserActivated?.Invoke(this, new UserIdleEventArgs(false));
                }
                else
                {
                    UserIdle?.Invoke(this, new UserIdleEventArgs(true));
                }
            }
        }
    }

    /// <summary>
    /// Occurs when the user has been inactive for the specified interval.
    /// </summary>
    public event EventHandler<UserIdleEventArgs>? UserIdle;

    /// <summary>
    /// Occurs when user inactivity ThreadStopped.
    /// </summary>
    public event EventHandler<UserIdleEventArgs>? UserActivated;

    private void AttachEventListeners(Control control)
    {
        control.KeyDown += Control_KeyEvent;
        control.KeyUp += Control_KeyEvent;
        control.MouseMove += Control_MouseEvent;
        control.MouseDown += Control_MouseEvent;
        control.MouseUp += Control_MouseEvent;
        control.MouseEnter += Control_MouseEvent;
        control.MouseLeave += Control_MouseEvent;
        control.MouseWheel += Control_MouseEvent;
        control.MouseDoubleClick += Control_MouseEvent;
    }

    private void DetachEventListeners(Control control)
    {
        control.KeyDown -= Control_KeyEvent;
        control.KeyUp -= Control_KeyEvent;
        control.MouseMove -= Control_MouseEvent;
        control.MouseDown -= Control_MouseEvent;
        control.MouseUp -= Control_MouseEvent;
        control.MouseEnter -= Control_MouseEvent;
        control.MouseLeave -= Control_MouseEvent;
        control.MouseWheel -= Control_MouseEvent;
        control.MouseDoubleClick -= Control_MouseEvent;
    }


    private readonly object lockObject = new();
    private double userInactiveInterval = 30;
    private DateTime previousActiveTime;
    private readonly Container container;

    private bool ThreadStopped
    {
        get
        {
            lock (lockObject)
            {
                return threadStopped;
            }
        }

        set
        {
            lock (lockObject)
            {
                threadStopped = value;
            }
        }
    }

    private bool threadStopped;
    private readonly Thread observerThread;
    private volatile bool idleEventInvoked;

    /// <summary>
    /// Gets or sets the date and time of previous user activity.
    /// </summary>
    /// <value>The date and time of previous user activity.</value>
    private DateTime PreviousActiveTime
    {
        get
        {
            lock (lockObject)
            {
                return previousActiveTime;
            }
        }

        set
        {
            lock (lockObject)
            {
                previousActiveTime = value;
            }

            if (idleEventInvoked)
            {
                idleEventInvoked = false;
                UserActivated?.Invoke(this, new UserIdleEventArgs(false));
            }
        }
    }

    private void ThreadFunc()
    {
        try
        {
            while (!ThreadStopped)
            {
                if (!idleEventInvoked)
                {
                    var spanSeconds = (DateTime.Now - PreviousActiveTime).TotalSeconds;
                    if (spanSeconds > UserInactiveInterval)
                    {
                        idleEventInvoked = true;
                        UserIdle?.Invoke(this, new UserIdleEventArgs(true));
                    }
                    else
                    {
                        idleEventInvoked = false;
                    }
                }

                Thread.Sleep(100);
            }
        }
        catch (ThreadInterruptedException ex)
        {
            RaiseExceptionOccurred(ex, nameof(UserIdleChecker), nameof(ThreadFunc));
        }
    }

    private void Control_KeyEvent(object? sender, KeyEventArgs e)
    {
        PreviousActiveTime = DateTime.Now;
    }

    private void Control_MouseEvent(object? sender, MouseEventArgs e)
    {
        PreviousActiveTime = DateTime.Now;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        ThreadStopped = true;
        observerThread.TryJoin(2000);

        DetachEventListeners(container);

        foreach (var control in container.Children)
        {
            DetachEventListeners(control);
        }
    }

    /// <inheritdoc />
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    /// <inheritdoc />
    public void RaiseExceptionOccurred(Exception exception, string @class, string method)
    {
        ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(exception, @class, method));
    }
}