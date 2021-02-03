using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using amp.Remote;

namespace AmpRESTfulTest
{
    /// <summary>
    /// A simple client class for the amp# remote api.
    /// </summary>
    public class AmpRestHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmpRestHttpClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="port">The port.</param>
        public AmpRestHttpClient(string baseUrl, int port)
        {
            BaseUrl = baseUrl;
            Port = port;
        }

        /// <summary>
        /// Plays the next song. The next song to be played depends on the queue, random and shuffle states of the program.
        /// </summary>
        public async Task NextSong()
        {
            await PostApiCommand("api/control/next");
        }

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        public async Task PreviousSong()
        {
            await PostApiCommand("api/control/previous");
        }

        /// <summary>
        /// Starts a playback. If no song is playing the next song depends on whether the randomization and/or shuffle is on or off.
        /// </summary>
        public async Task Play()
        {
            await PostApiCommand("api/control/play");
        }

        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public async Task Pause()
        {
            await PostApiCommand("api/control/pause");
        }

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <returns>A list of songs in the current album.</returns>
        public async Task<IEnumerable<AlbumSongRemote>> GetAlbumSongs()
        {
            try
            {
                var command = ServiceUrl + "api/songs";
                Log(command, "GET");
                return await HttpClient.GetFromJsonAsync<IEnumerable<AlbumSongRemote>>(command);
            }
            catch (Exception exception)
            {
                Log(exception, "GET");
                return new List<AlbumSongRemote>();
            }
        }

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <param name="queued">If true only the queued songs are returned.</param>
        /// <returns>A list of songs in the current album.</returns>
        public async Task<IEnumerable<AlbumSongRemote>> GetAlbumSongs(bool queued)
        {
            try
            {
                var command = ServiceUrl + $@"api/songs/{queued}";
                Log(command, "GET");
                return await HttpClient.GetFromJsonAsync<IEnumerable<AlbumSongRemote>>(command);
            }
            catch (Exception exception)
            {
                Log(exception, "GET");
                return new List<AlbumSongRemote>();
            }
        }

        /// <summary>
        /// Gets the ID, length and position of the currently playing song.
        /// </summary>
        /// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>
        public async Task<Tuple<int, double, double>> GetPlayingSong()
        {
            try
            {
                var command = ServiceUrl + "api/currentSong";
                Log(command, "GET");
                return await HttpClient.GetFromJsonAsync<Tuple<int, double, double>>(command);
            }
            catch (Exception exception)
            {
                Log(exception, "GET");
                return new Tuple<int, double, double>(-1, 0, 0);
            }
        }

        /// <summary>
        /// Gets the HTTP client to use with the RESTful service.
        /// </summary>
        /// <value>The HTTP client to use with the RESTful service.</value>
        private HttpClient HttpClient { get; } = new();

        /// <summary>
        /// Gets or sets the port to use with the RESTful service.
        /// </summary>
        /// <value>The port to use with the RESTful service.</value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the base URL to use with the RESTful service.
        /// </summary>
        /// <value>The base URL to use with the RESTful service.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Sets the RESTful service URL.
        /// </summary>
        /// <value>The RESTful service URL.</value>
        private string ServiceUrl
        {
            get
            {
                var builder = new UriBuilder(BaseUrl) {Port = Port};
                return builder.Uri.AbsoluteUri;
            }
        }

        /// <summary>
        /// The log message event.
        /// </summary>
        public EventHandler<AmpRestHttpClientLogEventArgs> LogMessage;

        /// <summary>
        /// Posts an API command with no result or additional data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="Task"/> instance.</returns>
        private async Task PostApiCommand(string command)
        {
            try
            {
                command = ServiceUrl + command;
                await HttpClient.PostAsync(command, null);
                Log(command, "POST");
            }
            catch (Exception exception)
            {
                Log(exception.Message, "POST");
            }
        }

        private void Log(Exception exception, string method)
        {
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"Error ({method}): '{exception.Message}'."});
        }

        private void Log(string message, string method)
        {
            LogMessage?.Invoke(this, new AmpRestHttpClientLogEventArgs {LogMessage = $"Command ({method}): '{message}'."});
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="AmpRestHttpClient.LogMessage"/> event.
    /// </summary>
    public class AmpRestHttpClientLogEventArgs : EventArgs
    {
        /// <summary>
        /// The message sent by the class.
        /// </summary>
        public string LogMessage;
    }
}