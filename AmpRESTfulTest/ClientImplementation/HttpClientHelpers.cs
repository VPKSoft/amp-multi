using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using amp.Remote.DataClasses;

namespace AmpRESTfulTest.ClientImplementation
{
    /// <summary>
    /// Some helper classes for the software <see cref="HttpClient"/> class instance(s).
    /// </summary>
    public static class HttpClientHelpers
    {
        /// <summary>
        /// Gets or sets the log messages class instance.
        /// </summary>
        /// <value>The log messages class instance.</value>
        public static LoggingBase LogMessages { get; set; } = new();

        /// <summary>
        /// Makes a HTTP get call and returns the deserialized JSON object value.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.Threading.Tasks.Task&lt;T&gt;.</returns>
        public static async Task<T> HttpGet<T>(this HttpClient httpClient, string baseUrl, string path)
        {
            try
            {
                var command = baseUrl + path;
                LogMessages.Log(path, "GET");
                return await httpClient.GetFromJsonAsync<T>(command);
            }
            catch (Exception exception)
            {
                LogMessages.Log(exception, "GET");
                return default;
            }
        }

        /// <summary>
        /// Makes a HTTP post call and returns the <see cref="HttpResponseMessage"/> value.
        /// </summary>
        /// <typeparam name="T">The type of the object to post.</typeparam>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Net.Http.HttpResponseMessage&gt;.</returns>
        public static async Task<HttpResponseMessage> HttpPost<T>(this HttpClient httpClient, string baseUrl,
            string path, T value)
        {
            try
            {
                var command = baseUrl + path;
                LogMessages.Log(path, "POST", value);
                var result = await httpClient.PostAsJsonAsync(new Uri(command), value, CancellationToken.None);
                return result;
            }
            catch (Exception exception)
            {
                LogMessages.Log(exception, "POST");
                return default;
            }
        }
    }
}
