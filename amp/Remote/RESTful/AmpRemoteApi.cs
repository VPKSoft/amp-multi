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
using System.Collections.Generic;
using System.Web.Http;
using amp.Remote.DataClasses;
using amp.Remote.WCFRemote;
using amp.UtilityClasses.Enumerations;

namespace amp.Remote.RESTful
{
    /// <summary>
    /// Initializes the RESTful API controllers.
    /// </summary>
    public static class RestInitializer
    {
        /// <summary>
        /// Initializes the RESTful API with a specified base URL, port and an instance to a <paramref name="remoteProvider"/>.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="port">The port. If set to zero the port is assumed to be in the <paramref name="baseUrl"/></param>
        /// <param name="remoteProvider">The remote data provider.</param>
        public static void InitializeRest(string baseUrl, int port, RemoteProvider remoteProvider)
        {
            if (port > 0)
            {
                var builder = new UriBuilder(new Uri(baseUrl)) {Port = port};
                baseUrl = builder.Uri.AbsoluteUri;
            }

            RemoteProvider = remoteProvider;

            AmpRemoteController.CreateInstance(baseUrl);
        }

        /// <summary>
        /// Gets or sets the remote control provider.
        /// </summary>
        /// <value>The remote control provider.</value>
        public static RemoteProvider RemoteProvider { get; set; }
    }

    /// <summary>
    /// A remote REST API for the amp# software.
    /// </summary>
    public class AlbumController: ApiController
    {
        #region Miscellaneous        
        /// <summary>
        /// A method to test the connection API connection.
        /// </summary>
        /// <returns>A string containing "OK".</returns>
        [Route("api/ok")]
        [HttpGet]
        public string TestConnection()
        {
            return "OK";
        }
        #endregion 

        #region AlbumSongs
        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <param name="queued">If true only the queued songs are returned.</param>
        /// <returns>A list of songs in the current album.</returns>
        [Route("api/songs/{queued}")]
        [HttpGet]
        public IEnumerable<AlbumSongRemote> GetAlbumSongs(bool queued)
        {
            return RestInitializer.RemoteProvider.GetAlbumSongs(queued);
        }

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <returns>A list of songs in the current album.</returns>
        [Route("api/songs")]
        [HttpGet]
        public IEnumerable<AlbumSongRemote> GetAlbumSongs()
        {
            return RestInitializer.RemoteProvider.GetAlbumSongs(false);
        }

        /// <summary>
        /// Gets the ID, length and position of the currently playing song.
        /// </summary>
        /// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>
        [Route("api/currentSong")]
        [HttpGet]
        public Tuple<int, double, double> GetPlayingSong()
        {
            return RestInitializer.RemoteProvider.MFile == null
                ? new Tuple<int, double, double>(-1, 0, 0)
                : new Tuple<int, double, double>(RestInitializer.RemoteProvider.MFile.ID,
                    RestInitializer.RemoteProvider.Seconds, RestInitializer.RemoteProvider.SecondsTotal);
        }
        #endregion

        #region State
        /// <summary>
        /// Gets the current state of the amp# music player.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/state")]
        public PlayerStateRemote GetPlayerState()
        {
            return RestInitializer.RemoteProvider.GetPlayerState();
        }

        /// <summary>
        /// Gets a value if the queue was changed from the previous query.
        /// </summary>
        /// <returns>True if the queue was changed from the previous query, otherwise false.</returns>
        [HttpGet]
        [Route("api/queueChanged")]
        public bool QueueChanged()
        {
            return RestInitializer.RemoteProvider.QueueChanged();
        }

        /// <summary>
        /// Gets a value if the play list of the current album was changed from the previous query.
        /// </summary>
        /// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>
        [HttpGet]
        [Route("api/albumPlayListChanged")]
        public bool AlbumPlayListChanged()
        {
            return RestInitializer.RemoteProvider.AlbumPlayListChanged();
        }

        /// <summary>
        /// Gets a value if current music album was changed.
        /// </summary>
        /// <returns>True if current music album was changed, otherwise false.</returns>
        [HttpGet]
        [Route("api/albumChanged")]
        public bool AlbumChanged()
        {
            return RestInitializer.RemoteProvider.AlbumChanged;
        }
        #endregion

        #region Controls
        /// <summary>
        /// Executes simple commands such as play, pause, next, previous and so on.
        /// </summary>
        [HttpPost]
        [Route("api/control/{command}")]
        public void RunCommand(string command)
        {
            if (command == "next")
            {
                RestInitializer.RemoteProvider.GetNextSong();
            }

            if (command == "previous")
            {
                RestInitializer.RemoteProvider.GetPrevSong();
            }

            if (command == "pause")
            {
                RestInitializer.RemoteProvider.Pause();
            }

            if (command == "play")
            {
                RestInitializer.RemoteProvider.Play();
            }
        }

        /// <summary>
        /// Sets the playback position in seconds.
        /// </summary>
        /// <param name="seconds">The playback position in seconds.</param>
        [HttpPost]
        [Route("api/control/setPositionSeconds")]
        public void SetPositionSeconds([FromBody]double seconds)
        {
            RestInitializer.RemoteProvider.SetPositionSeconds(seconds);
        }
        #endregion

        #region Queue
        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        [HttpPost]
        [Route("api/queue/{insert}")]
        public List<AlbumSongRemote> Queue(bool insert, [FromBody] List<AlbumSongRemote> queueList)
        {
            // TODO::!!
            RestInitializer.RemoteProvider.Queue(insert, queueList);
            if (RestInitializer.RemoteProvider.Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                RestInitializer.RemoteProvider.ShowQueue();
            }

            return RestInitializer.RemoteProvider.GetQueuedSongs();
        }
        #endregion

        public string Get()
        {
            return RestInitializer.RemoteProvider.CurrentAlbum;
        }

        public string Get(int id)
        {
            return Get();
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
            Console.WriteLine("Post method called with value = " + value);
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
            Console.WriteLine("Put method called with value = " + value);
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
            Console.WriteLine("Delete method called with id = " + id);
        }
    }
}
