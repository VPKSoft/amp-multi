using System;

namespace AmpRESTfulTest.ClientImplementation;

/// <summary>
/// An interface to log messages.
/// </summary>
public interface ILogInterface
{
    /// <summary>
    /// Logs the specified exception.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
    void Log(Exception exception, string method);

    /// <summary>
    /// Logs the specified message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
    void Log(string message, string method);

    /// <summary>
    /// Logs the specified message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
    /// <param name="value">The object which value to send.</param>
    void Log(string message, string method, object value);
}