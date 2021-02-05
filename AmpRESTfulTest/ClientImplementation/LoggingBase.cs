using System;
using Newtonsoft.Json;

namespace AmpRESTfulTest.ClientImplementation
{
    /// <summary>
    /// A class to provide message logging via <see cref="LogMessage"/> event.
    /// Implements the <see cref="AmpRESTfulTest.ClientImplementation.ILogInterface" />
    /// </summary>
    /// <seealso cref="AmpRESTfulTest.ClientImplementation.ILogInterface" />
    public class LoggingBase: ILogInterface
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
        public void Log(Exception exception, string method)
        {
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"Error ({method}): '{exception.Message}'."});
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
        public void Log(string message, string method)
        {
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"Command ({method}): '{message}'."});
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="method">The HTTP method. I.e. POST, GET, PUT.</param>
        /// <param name="value">The object which value to send.</param>
        public void Log(string message, string method, object value)
        {
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"Command ({method}): '{message}'."});
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"With value): '{JsonConvert.SerializeObject(value)}'."});
        }

        /// <summary>
        /// The log message event.
        /// </summary>
        public EventHandler<AmpRestHttpClientLogEventArgs> LogMessage;
    }
}
