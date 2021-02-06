using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using amp.Remote.DataClasses;
using Newtonsoft.Json;

namespace AmpRESTfulTest.ClientImplementation
{
    /// <summary>
    /// A simple client class for the amp# remote api.
    /// </summary>
    public class AmpRestHttpClient: LoggingBase
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

        #region PlayerCommands
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
        #endregion

        #region SongData
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
        #endregion

        #region Queue

        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="albumSong">A song to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public async Task<List<AlbumSongRemote>> Queue(bool insert, AlbumSongRemote albumSong)
        {
            return await Queue(insert, new List<AlbumSongRemote>(new[] {albumSong}));
        }


        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public async Task<List<AlbumSongRemote>> Queue(bool insert, List<AlbumSongRemote> queueList)
        {
            var result = await HttpClient.HttpPost(ServiceUrl, $@"api/queue/{insert}", queueList);
            try
            {
                result.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<List<AlbumSongRemote>>((await result.Content.ReadAsStringAsync()));
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region PlayerState
        /// <summary>
        /// Sets the playback position in seconds.
        /// </summary>
        /// <param name="seconds">The playback position in seconds.</param>
        public async Task SetPositionSeconds(double seconds)
        {
            await HttpClient.HttpPost(ServiceUrl, "api/control/setPositionSeconds", seconds);
        }

        /// <summary>
        /// Gets the current state of the amp# music player.
        /// </summary>
        /// <returns>An instance to a <see cref="PlayerStateRemote"/> class.</returns>
        public async Task<PlayerStateRemote> GetPlayerState()
        {
            return await HttpClient.HttpGet<PlayerStateRemote>(ServiceUrl, "api/state");
        }

        /// <summary>
        /// Gets a value if the queue was changed from the previous query.
        /// </summary>
        /// <returns>True if the queue was changed from the previous query, otherwise false.</returns>
        public async Task<bool> QueueChanged()
        {
            return await HttpClient.HttpGet<bool>(ServiceUrl, "api/queueChanged");
        }

        /// <summary>
        /// Gets a value if the play list of the current album was changed from the previous query.
        /// </summary>
        /// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>
        public async Task<bool> AlbumPlayListChanged()
        {
            return await HttpClient.HttpGet<bool>(ServiceUrl, "api/albumPlayListChanged");
        }

        /// <summary>
        /// Gets a value if current music album was changed.
        /// </summary>
        /// <returns>True if current music album was changed, otherwise false.</returns>
        public async Task<bool> AlbumChanged()
        {
            return await HttpClient.HttpGet<bool>(ServiceUrl, "api/albumChanged");
        }
        #endregion

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

    }
}