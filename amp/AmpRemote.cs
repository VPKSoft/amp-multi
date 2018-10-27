#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System.Windows.Forms;
using System.Collections.Generic;
using VPKSoft.LangLib;
using System;
using System.ServiceModel; // for remote control..
using System.ServiceModel.Description; // for remote control..
using System.Data.SQLite; // for the database access..
using System.Globalization;
using System.Linq;

namespace amp
{
    /// <summary>
    /// An implementation for the remote control interface for the amp#.
    /// </summary>
    public class AmpRemote : IampRemote
    {
        #region WCF

        /// <summary>
        /// The remote control HTTP URL.
        /// </summary>
        Uri ampRemoteAddress = new Uri("http://localhost:11316/ampRemote");

        /// <summary>
        /// A ServiceHost class instance for the self-hosted remote control basic HTTP binding WCF API.
        /// </summary>
        ServiceHost ampRemoteHost = null;

        /// <summary>
        /// Reference to the MainWindow class instance as the playback logic is (sadly) in there.
        /// </summary>
        public static MainWindow MainWindow = null;

        /// <summary>
        /// Initializes a ServiceHost class instance for the self-hosted remote control basic HTTP binding WCF API.
        /// </summary>
        /// <returns>True if the initialization was a success, otherwise false.</returns>
        public bool InitAmpRemote()
        {
            // If not defined int the settings, we don't even try..
            if (!MainWindow.RemoteControlApiWCF)
            {
                return false;
            }

            // Create a ServiceHost class instance
            ampRemoteHost = new ServiceHost(this.GetType(), ampRemoteAddress);

            try // try to create a self-hosted WCF HTTP binding..
            {
                ampRemoteAddress = new Uri(MainWindow.RemoteControlApiWCFAddress); // From the settings we get this..

                // A data transfer of information of possibly thousands of songs require large buffers/capacity..
                ampRemoteHost.AddServiceEndpoint(typeof(IampRemote), new BasicHttpBinding() { MaxReceivedMessageSize = 2147483647, MaxBufferPoolSize = 2147483647 }, string.Empty);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

                smb.HttpGetEnabled = true; // just enable HTTP

                ampRemoteHost.Description.Behaviors.Add(smb); // Add the behavior..

                ampRemoteHost.Open(); // open the self-hosted WCF HTTP binding..
                return true; // if success..
            }
            catch (Exception ex)
            {
                // If the self-hosted WCF HTTP binding was defined in the settings, but failed, do inform the user
                MessageBox.Show(DBLangEngine.GetStatMessage("msgRemoteWCFFailed", "Remote control HTTP binding failed to initialize ({0}) with an exception {1}.|As in the WCF self-hosting web service failed to initialize.", MainWindow.RemoteControlApiWCFAddress, ex.Message),
                                DBLangEngine.GetStatMessage("msgError", "Error|A common error that should be defined in another message"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Do abort the host
                ampRemoteHost.Abort();

                // Not a disposable, so just null it
                ampRemoteHost = null;

                // FAIL!
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Gets the name of the current album.
        /// </summary>
        /// <returns>The name of the current album</returns>
        public string GetAlbumName()
        {
            return MainWindow.CurrentAlbum;
        }

        /// <summary>
        /// Plays the next song. The next song to be played depends on the queue, random and shuffle states of the program.
        /// </summary>
        public void NextSong()
        {
            MainWindow.GetNextSong();
        }

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        public void PreviousSong()
        {
            MainWindow.GetPrevSong();
        }

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <param name="queued">If true only the queued songs are returned.</param>
        /// <returns>A list of songs in the current album.</returns>
        public List<AlbumSongWCF> GetAlbumSongs(bool queued = false)
        {
            List<AlbumSongWCF> retList = new List<AlbumSongWCF>();
            foreach (MusicFile mf in MainWindow.PlayList)
            {
                if (mf.QueueIndex == 0 && queued)
                {
                    continue; // if only queued songs..
                }

                retList.Add(new AlbumSongWCF
                {
                    ID = mf.ID,
                    Duration = mf.Duration,
                    Volume = mf.Volume,
                    QueueIndex = mf.QueueIndex,
                    Rating = mf.Rating,
                    SongName = mf.SongName,
                    Album = mf.Album,
                    Artist = mf.Artist,
                    SongNameNoQueue = mf.SongNameNoQueue,
                    OverrideName = mf.OverrideName,
                    TagStr = mf.TagString,
                    Title = mf.Title,
                    Track = mf.Track,
                    Year = mf.Year,
                    FullFileName = mf.FullFileName
                }); // wow what a construct!
            }
            return retList;
        }

        /// <summary>
        /// Gets the queued songs.
        /// </summary>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongWCF> GetQueuedSongs()
        {
            return GetAlbumSongs(true);
        }

        /// <summary>
        /// Gets the ID, length and position of the currently playing song.
        /// </summary>
        /// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>
        public Tuple<int, double, double> GetPlayingSong()
        {
            return MainWindow.mFile == null ? new Tuple<int, double, double>(-1, 0, 0) : new Tuple<int, double, double>(MainWindow.mFile.ID, MainWindow.seconds, MainWindow.seconds_total);
        }

        /// <summary>
        /// Gets the current state of the amp# music player.
        /// </summary>
        /// <returns></returns>
        public PlayerState GetPlayerState()
        {
            MusicFile musicFile = MainWindow.mFile;

            int curentSongID = -1;
            double currentSongPosition = 0, currentSongLength = 0;
            string currentSongName = string.Empty;

            bool albumChanged = MainWindow.AlbumChanged;

            if (musicFile != null)
            {
                curentSongID = musicFile.ID;
                currentSongPosition = MainWindow.seconds;
                currentSongLength = MainWindow.seconds_total;
                currentSongName = musicFile.SongNameNoQueue;
            }

            return new PlayerState
            {
                Paused = Paused(),
                QueueCount = GetQueuedSongs().Count,
                Random = Randomizing(),
                Shuffle = Shuffle(),
                QueueChangedFromPreviousQuery = MusicFile.QueueChanged,
                CurrentSongID = curentSongID,
                CurrentSongLength = currentSongLength,
                CurrentSongName = currentSongName,
                CurrentSongPosition = currentSongPosition,
                CurrentAlbumName = GetAlbumName(),
                Stopped = Stopped(),
                AlbumContentsChanged = albumChanged,
                AlbumChanged = Database.AlbumChanged || albumChanged,
                SongsChanged = MainWindow.SongsChanged,
                CanGoPrevious = MainWindow.CanGoPrevious,
                AlbumLoading = MainWindow.AlbumLoading
            };
        }

        /// <summary>
        /// Gets a value if the queue was changed from the previous query.
        /// </summary>
        /// <returns>True if the queue was changed from the previous query, otherwise false.</returns>
        public bool QueueChanged()
        {
            return MusicFile.QueueChanged;
        }

        /// <summary>
        /// Gets a value if the play list of the current album was changed from the previous query.
        /// </summary>
        /// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>
        public bool AlbumPlayListChanged()
        {
            return Database.AlbumChanged;
        }

        /// <summary>
        /// Gets a value if current music album was changed.
        /// </summary>
        /// <returns>True if current music album was changed, otherwise false.</returns>
        public bool AlbumChanged()
        {
            return MainWindow.AlbumChanged;
        }

        /// <summary>
        /// Seeks the current song to a given <paramref name="seconds"/>. This will not cause an error if the given position is out of range.
        /// </summary>
        /// <param name="seconds">The position to jump the playback to.</param>
        public void SetPositionSeconds(double seconds)
        {
            MainWindow.SetPositionSeconds(seconds);
        }

        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongWCF> Queue(bool insert, List<AlbumSongWCF> queueList)
        {
            MainWindow.Queue(insert, queueList);
            if (MainWindow.QueueShowing) // refresh the queue list if it's showing..
            {
                MainWindow.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs ID's to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongWCF> QueueIDs(bool insert, List<int> songIDs)
        {
            MainWindow.Queue(insert, songIDs);
            if (MainWindow.QueueShowing) // refresh the queue list if it's showing..
            {
                MainWindow.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Scrambles the queue to have new random indices.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueue()
        {
            return MainWindow.ScrambleQueue();
        }

        /// <summary>
        /// Gets a list of songs which properties were changed (name, volume, rating).
        /// </summary>
        /// <returns>A list of songs which properties have been changed in the current album.</returns>
        public List<AlbumSongWCF> GetChangedSongs()
        {
            List<AlbumSongWCF> retList = new List<AlbumSongWCF>();
            foreach (MusicFile mf in MainWindow.PlayList)
            {
                if (!mf.SongChanged)
                {
                    continue; // if only queued songs..
                }

                mf.SongChanged = false;

                retList.Add(new AlbumSongWCF
                {
                    ID = mf.ID,
                    Duration = mf.Duration,
                    Volume = mf.Volume,
                    QueueIndex = mf.QueueIndex,
                    Rating = mf.Rating,
                    SongName = mf.SongName,
                    Album = mf.Album,
                    Artist = mf.Artist,
                    SongNameNoQueue = mf.SongNameNoQueue,
                    OverrideName = mf.OverrideName,
                    TagStr = mf.TagString,
                    Title = mf.Title,
                    Track = mf.Track,
                    Year = mf.Year,
                    FullFileName = mf.FullFileName
                }); // wow what a construct!
            }
            return retList;
        }

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="songID">A list of song IDs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongWCF> QueueID(bool insert, List<int> songIDs)
        {
            MainWindow.Queue(insert, songIDs);

            if (MainWindow.QueueShowing) // refresh the queue list if it's showing..
            {
                MainWindow.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a paused state.
        /// </summary>
        /// <returns>Returns true if the playback is in a paused state, otherwise false.</returns>
        public bool Paused()
        {
            return MainWindow.Paused();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a stopped state.
        /// </summary>
        /// <returns>Returns true if the playback is in a stopped state, otherwise false.</returns>
        public bool Stopped()
        {
            return MainWindow.Stopped();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a playing state.
        /// </summary>
        /// <returns>Returns true if the playback is in a playing state, otherwise false.</returns>
        public bool Playing()
        {
            return MainWindow.Playing();
        }

        /// <summary>
        /// Starts a playback. If no song is playing the next song depends on whether the randomization and/or shuffle is on or off.
        /// </summary>
        public void Play()
        {
            MainWindow.Play();
        }

        /// <summary>
        /// Pauses the playback if not already paused.
        /// </summary>
        public void Pause()
        {
            MainWindow.Pause();
        }


        /// <summary>
        /// Plays a song with a given ID.
        /// </summary>
        /// <param name="ID">An ID for the song which to play.</param>
        public void PlayID(int ID)
        {
            MainWindow.Play(ID);
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is randomizing songs, otherwise false.</returns>
        public bool Randomizing()
        {
            return MainWindow.Randomizing;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <param name="value">A value indicating if the randomization should be on or off.</param>
        public void SetRandomizing(bool value)
        {
            MainWindow.Randomizing = value;
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        public bool Shuffle()
        {
            return MainWindow.Suffle;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        public void SetShuffle(bool value)
        {
            MainWindow.Suffle = value;
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the Shuffle() method the keep backwards compatibility.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        public bool Suffle()
        {
            return MainWindow.Suffle;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the SetShuffle() method the keep backwards compatibility.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        public void SetSuffle(bool value)
        {
            MainWindow.Suffle = value;
        }

        /// <summary>
        /// Removes a song from the current album. No changes to the file system are applied.
        /// </summary>
        /// <param name="song">A song to remove from the current album.</param>
        /// <returns>True if the remove operation was successful, i.e. the song was found and removed, otherwise false.</returns>
        public bool RemoveSongFromAlbum(AlbumSongWCF song)
        {
            try
            {
                MainWindow.RemoveSongFromAlbum(song);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the volume of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="volume">The volume to set for the currently playing song. This value must be between 0 and 2 where 1 means original volume.</param>
        /// <returns>True if a song was playing which volume to set and the given volume was within acceptable range, otherwise false.</returns>
        public bool SetVolume(float volume) // 0.0-2.0
        {
            return MainWindow.SetVolume(volume);
        }

        /// <summary>
        /// Sets the volume for multiple songs with a given list of ID numbers.
        /// </summary>
        /// <param name="songIDList">The song identifier list.</param>
        /// <param name="volume">The volume to set for the given songs. This value must be between 0 and 2 where 1 means original volume.</param>
        /// <returns>True if the given list was valid and the and the given volume was within acceptable range, otherwise false.</returns>
        public bool SetVolumeMultiple(List<int> songIDList, float volume) // 0.0-2.0
        {
            return MainWindow.SetVolume(songIDList, volume);
        }


        /// <summary>
        /// Sets the rating (stars) of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="rating">A rating for the song. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a song was playing which rating to set and the given rating was within acceptable range, otherwise false.</returns>
        public bool SetRating(int rating) // 0-1000
        {
            return MainWindow.SetRating(rating);
        }

        /// <summary>
        /// Sets the rating (stars) for the songs matching the given song ID list. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="songIDList">The song identifier list.</param>
        /// <param name="rating">A rating for the songs. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a songs which rating to set was a valid list and the given rating was within acceptable range, otherwise false.</returns>
        public bool SetRatingMultiple(List<int> songIDList, int rating)
        {
            return MainWindow.SetRating(songIDList, rating);
        }

        /// <summary>
        /// Gets a list of saved queues for a given album ID.
        /// </summary>
        /// <param name="albumName">A name for of an album which queue list to get. A String.Empty returns saved queues for all albums.</param>
        /// <returns>A list of QueueEntry class instances for the requested album.</returns>
        public List<QueueEntry> GetQueueList(string albumName)
        {
            SQLiteConnection conn = MainWindow.conn; // there is sill a dependency for the MainWindow..
            List<QueueEntry> queueList = new List<QueueEntry>();
            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                if (albumName == string.Empty)
                {
                    command.CommandText =
                        string.Format("SELECT COUNT(DISTINCT ID) FROM " + Environment.NewLine +
                                      "QUEUE_SNAPSHOT WHERE ALBUM_ID = (SELECT ID FROM ALBUM WHERE ALBUMNAME = '{0}') ", albumName.Replace("'", "''"));
                }
                else
                {
                    command.CommandText =
                        "SELECT COUNT(DISTINCT ID) FROM " + Environment.NewLine +
                         "QUEUE_SNAPSHOT ";
                }

                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    return queueList;
                }

                if (albumName == string.Empty)
                {
                    command.CommandText =
                        "SELECT ID, SNAPSHOTNAME, MAX(SNAPSHOT_DATE) AS SNAPSHOT_DATE " + Environment.NewLine +
                                      "FROM QUEUE_SNAPSHOT " + Environment.NewLine +
                                      "GROUP BY ID, SNAPSHOTNAME " + Environment.NewLine +
                                      "ORDER BY MAX(SNAPSHOT_DATE) ";

                }
                else
                {
                    command.CommandText =
                        string.Format("SELECT ID, SNAPSHOTNAME, MAX(SNAPSHOT_DATE) AS SNAPSHOT_DATE " + Environment.NewLine +
                                      "FROM QUEUE_SNAPSHOT WHERE ALBUM_ID = (SELECT ALBUM_ID FROM ALBUM WHERE ALBUMNAME = '{0}') " + Environment.NewLine +
                                      "GROUP BY ID, SNAPSHOTNAME " + Environment.NewLine +
                                      "ORDER BY MAX(SNAPSHOT_DATE) ", albumName.Replace("'", "''"));
                }


                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        queueList.Add(new QueueEntry()
                        {
                            CreteDate = DateTime.ParseExact(dr.GetString(2), "yyyy-MM-dd HH':'mm':'ss", CultureInfo.InvariantCulture),
                            ID = dr.GetInt32(0),
                            QueueName = dr.GetString(1)
                        });
                    }
                }
            }

            return queueList;
        }

        /// <summary>
        /// Gets a list if albums stored in the amp# database.
        /// </summary>
        /// <returns>A list of AlbumWCF class instances indicating the albums in the amp# database.</returns>
        public List<AlbumWCF> GetAlbums()
        {
            return MainWindow.GetAlbums();
        }

        /// <summary>
        /// Changes a playing album to a given album name.
        /// </summary>
        /// <param name="albumName">A name of an album to play.</param>
        /// <returns>True if the album exists and was successfully changed, otherwise false.</returns>
        public bool SelectAlbum(string albumName)
        {
            return MainWindow.SelectAlbum(albumName);
        }

        /// <summary>
        /// Gets a list of saved queues for the current album.
        /// </summary>
        /// <returns>A list of QueueEntry class instances for the current album.</returns>
        public List<QueueEntry> GetQueueListCurrentAlbum()
        {
            return GetQueueList(MainWindow.CurrentAlbum);
        }

        /// <summary>
        /// Loads a queue to the play list with a given unique ID.
        /// </summary>
        /// <param name="QueueID">An index a queue to load for the playback for the current album.</param>
        public void LoadQueue(int QueueID)
        {
            MainWindow.RefreshLoadQueueStats(QueueID, false);
        }

        /// <summary>
        /// Appends a queue to the play list with a given unique ID preserving the current queue.
        /// </summary>
        /// <param name="QueueID">An index a queue to load for the playback for the current album.</param>
        public void AppendQueue(int QueueID)
        {
            MainWindow.RefreshLoadQueueStats(QueueID, true);
        }

        /// <summary>
        /// Gets a value indicating if a previous song can be selected from the amp# play list.
        /// </summary>
        /// <returns>A value indicating if a previous song can be selected from the amp# play list.</returns>
        public bool CanGoPrevious()
        {
            return MainWindow.CanGoPrevious;
        }

        /// <summary>
        /// Just a simple method to check if the connection is working.
        /// </summary>
        /// <returns>Always returns true.</returns>
        public bool ConnectionTest()
        {
            return true;
        }


        /// <summary>
        /// Gets the descriptions of all the public members of a class implementing this interface.
        /// </summary>
        /// <returns>The descriptions of all the public members of a class implementing this interface.</returns>
        public string GetMemberDescriptions()
        {
            return string.Join(
                Environment.NewLine,
                "/// <summary>",
                "/// Gets a value indicating if a previous song can be selected from the amp# play list.",
                "/// </summary>",
                "/// <returns>A value indicating if a previous song can be selected from the amp# play list.</returns>",
                "[OperationContract]",
                "bool CanGoBack();", 
                "",
                "/// <summary>",
                "/// Gets the name of the current album.",
                "/// </summary>",
                "/// <returns>The name of the current album</returns>",
                "[OperationContract]",
                "string GetAlbumName();",
                "",
                "/// <summary>",
                "/// Plays the next song. The next song to be played depends on the queue, random and shuffle states of the program.",
                "/// </summary>",
                "[OperationContract()]",
                "void NextSong();",
                "",
                "/// <summary>",
                "/// Plays the previous song.",
                "/// </summary>",
                "[OperationContract]",
                "void PreviousSong();",
                "",
                "/// <summary>",
                "/// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.",
                "/// </summary>",
                "/// <param name=\"queued\">If true only the queued songs are returned.</param>",
                "/// <returns>A list of songs in the current album.</returns>",
                "[OperationContract]",
                "List<AlbumSongWCF> GetAlbumSongs(bool queued = false);",
                "",
                "/// <summary>",
                "/// Gets the queued songs.",
                "/// </summary>",
                "/// <returns>A list of queued songs in the current album.</returns>",
                "[OperationContract]",
                "List<AlbumSongWCF> GetQueuedSongs();",
                "",
                "/// <summary>",
                "/// Gets the ID, length and position of the currently playing song.",
                "/// </summary>",
                "/// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>",
                "[OperationContract]",
                "Tuple<int, double, double> GetPlayingSong();",
                "",
                "/// <summary>",
                "/// Gets the current state of the amp# music player.",
                "/// </summary>",
                "/// <returns></returns>",
                "[OperationContract]",
                "PlayerState GetPlayerState();",
                "",
                "/// <summary>",
                "/// Seeks the current song to a given <paramref name=\"seconds\" />.This will not cause an error if the given position is out of range.",
                "/// </summary>",
                "/// <param name=\"seconds\">The position to jump the playback to.</param>",
                "[OperationContract]",
                "void SetPositionSeconds(double seconds);",
                "",
                "/// <summary>",
                "/// Gets a value if the queue was changed from the previous query.",
                "/// </summary>",
                "/// <returns>True if the queue was changed from the previous query, otherwise false.</returns>",
                "[OperationContract]",
                "bool QueueChanged();",
                "",
                "/// <summary>",
                "/// Gets a value if the play list of the current album was changed from the previous query.",
                "/// </summary>",
                "/// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>",
                "[OperationContract]",
                "bool AlbumPlayListChanged();",
                "",
                "/// <summary>",
                "/// Gets a value if current music album was changed.",
                "/// </summary>",
                "/// <returns>True if current music album was changed, otherwise false.</returns>",
                "[OperationContract]",
                "bool AlbumChanged();",
                "",
                "/// <summary>",
                "/// Inserts or appends to the queue the given song list.",
                "/// </summary>",
                "/// <param name=\"insert\">Whether to insert or append to the queue.</param>",
                "/// <param name=\"queueList\">A list of songs to be appended or inserted into the queue.</param>",
                "/// <returns>A list of queued songs in the current album.</returns>",
                "[OperationContract]",
                "List<AlbumSongWCF> Queue(bool insert, List<AlbumSongWCF> queueList);",
                "",
                "/// <summary>",
                "/// Gets a list of songs which properties were changed (name, volume, rating).",
                "/// </summary>",
                "/// <returns>A list of songs which properties have been changed in the current album.</returns>",
                "[OperationContract]",
                "List<AlbumSongWCF> GetChangedSongs();",
                "",
                "/// <summary>",
                "/// Inserts or appends to the queue the given song ID list.",
                "/// </summary>",
                "/// <param name=\"insert\">Whether to insert or append to the queue.</param>",
                "/// <param name=\"songID\">A list of song IDs to be appended or inserted into the queue.</param>",
                "/// <returns>A list of queued songs in the current album.</returns>",
                "[OperationContract]",
                "List<AlbumSongWCF> QueueID(bool insert, List<int> songIDs);",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback is in a paused state.",
                "/// </summary>",
                "/// <returns>Returns true if the playback is in a paused state, otherwise false.</returns>",
                "[OperationContract]",
                "bool Paused();",
                "",
                "/// <summary>",
                "/// Pauses the playback if not already paused.",
                "/// </summary>",
                "[OperationContract]",
                "void Pause();",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback is in a stopped state.",
                "/// </summary>",
                "/// <returns>Returns true if the playback is in a stopped state, otherwise false.</returns>",
                "[OperationContract]",
                "bool Stopped();",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback is in a playing state.",
                "/// </summary>",
                "/// <returns>Returns true if the playback is in a playing state, otherwise false.</returns>",
                "[OperationContract]",
                "bool Playing();",
                "",
                "/// <summary>",
                "/// Starts a playback. If no song is playing the next song depends on whether the randomization and/or shuffle is on or off.",
                "/// </summary>",
                "[OperationContract]",
                "void Play();",
                "",
                "/// <summary>",
                "/// Plays a song with a given ID.",
                "/// </summary>",
                "/// <param name=\"ID\">An ID for the song which to play.</param>",
                "[OperationContract]",
                "void PlayID(int ID);",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback \"engine\" is randomizing songs. Queued song do override randomization.",
                "/// </summary>",
                "/// <returns>Returns true if the playback \"engine\" is randomizing songs, otherwise false.</returns>",
                "[OperationContract]",
                "bool Randomizing();",
                "",
                "/// <summary>",
                "/// Sets a value indicating if the playback \"engine\" is randomizing songs. Queued song do override randomization.",
                "/// </summary>",
                "/// <param name=\"value\">A value indicating if the randomization should be on or off.</param>",
                "[OperationContract]",
                "void SetRandomizing(bool value);",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback \"engine\" is shuffling songs. Queued song do override shuffling.",
                "/// </summary>",
                "/// <returns>Returns true if the playback \"engine\" is shuffling songs, otherwise false.</returns>",
                "[OperationContract]",
                "bool Shuffle();",
                "",
                "/// <summary>",
                "/// Sets a value indicating if the playback \"engine\" is shuffling songs. Queued song do override shuffling.",
                "/// </summary>",
                "/// <param name=\"value\">A value indicating if the shuffling should be on or off.</param>",
                "[OperationContract]",
                "void SetShuffle(bool value);",
                "",
                "/// <summary>",
                "/// Gets a value indicating if the playback \"engine\" is shuffling songs. Queued song do override shuffling.",
                "/// <para/>This is the miss-spelled version of the Shuffle() method the keep backwards compatibility.",
                "/// </summary>",
                "/// <returns>Returns true if the playback \"engine\" is shuffling songs, otherwise false.</returns>",
                "[OperationContract]",
                "bool Suffle();",
                "",
                "/// <summary>",
                "/// Sets a value indicating if the playback \"engine\" is shuffling songs. Queued song do override shuffling.",
                "/// <para/>This is the miss-spelled version of the SetShuffle() method the keep backwards compatibility.",
                "/// </summary>",
                "/// <param name=\"value\">A value indicating if the shuffling should be on or off.</param>",
                "[OperationContract]",
                "void SetSuffle(bool value);",
                "",
                "/// <summary>",
                "/// Removes a song from the current album. No changes to the file system are applied.",
                "/// </summary>",
                "/// <param name=\"song\">A song to remove from the current album.</param>",
                "/// <returns>True if the remove operation was successful, i.e. the song was found and removed, otherwise false.</returns>",
                "[OperationContract]",
                "bool RemoveSongFromAlbum(AlbumSongWCF song);",
                "",
                "/// <summary>",
                "/// Sets the volume of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.",
                "/// </summary>",
                "/// <param name=\"volume\">The volume to set for the currently playing song. This value must be between 0 and 2 where 1 means original volume.</param>",
                "/// <returns>True if a song was playing which volume to set and the given volume was within acceptable range, otherwise false.</returns>",
                "[OperationContract]",
                "bool SetVolume(float volume);",
                "",
                "/// <summary>",
                "/// Sets the rating (stars) of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.",
                "/// </summary>",
                "/// <param name=\"rating\">A rating for the song. This value must be between 0 and 1000 where 500 means a normal rating.</param>",
                "/// <returns>True if a song was playing which rating to set and the given rating was within acceptable range, otherwise false.</returns>",
                "[OperationContract]",
                "bool SetRating(int rating);",
                "",
                "/// <summary>",
                "/// Gets a list of saved queues for a given album ID.",
                "/// </summary>",
                "/// <param name=\"albumName\">A name for of an album which queue list to get. A String.Empty returns saved queues for all albums.</param>",
                "/// <returns>A list of QueueEntry class instances for the requested album.</returns>",
                "[OperationContract]",
                "List<QueueEntry> GetQueueList(string albumName);",
                "",
                "/// <summary>",
                "/// Gets a list of saved queues for the current album.",
                "/// </summary>",
                "/// <returns>A list of QueueEntry class instances for the current album.</returns>",
                "[OperationContract]",
                "List<QueueEntry> GetQueueListCurrentAlbum();",
                "",
                "/// <summary>",
                "/// Loads a queue to the play list with a given unique ID.",
                "/// </summary>",
                "/// <param name=\"QueueID\">An index a queue to load for the playback for the current album.</param>",
                "[OperationContract]",
                "void LoadQueue(int QueueID);",
                "",
                "/// <summary>",
                "/// Appends a queue to the play list with a given unique ID preserving the current queue.",
                "/// </summary>",
                "/// <param name=\"QueueID\">An index a queue to load for the playback for the current album.</param>",
                "[OperationContract]",
                "void AppendQueue(int QueueID);",
                "",
                "/// <summary>",
                "/// Gets a list if albums stored in the amp# database.",
                "/// </summary>",
                "/// <returns>A list of AlbumWCF class instances indicating the albums in the amp# database.</returns>",
                "[OperationContract]",
                "List<AlbumWCF> GetAlbums();",
                "",
                "/// <summary>",
                "/// Changes a playing album to a given album name.",
                "/// </summary>",
                "/// <param name=\"albumName\">A name of an album to play.</param>",
                "/// <returns>True if the album exists and was successfully changed, otherwise false.</returns>",
                "[OperationContract]",
                "bool SelectAlbum(string albumName);",
                "",
                "/// <summary>",
                "/// Gets the descriptions of all the public members of a class implementing this interface.",
                "/// </summary>",
                "/// <returns>The descriptions of all the public members of a class implementing this interface.</returns>",
                "[OperationContract]",
                "string GetMemberDescriptions();",
                "",
                "/// <summary>",
                "/// Just a simple method to check if the connection is working.",
                "/// </summary>",
                "/// <returns>Always returns true.</returns>",
                "[OperationContract]",
                "bool ConnectionTest();"
          );
        }
    }
}