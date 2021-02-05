using System;

namespace AmpRESTfulTest.ClientImplementation
{
    /// <summary>
    /// Event arguments for the <see cref="ILogInterface.LogMessage"/> event.
    /// </summary>
    public class AmpRestHttpClientLogEventArgs : EventArgs
    {
        /// <summary>
        /// The message sent by the class.
        /// </summary>
        public string LogMessage;
    }
}