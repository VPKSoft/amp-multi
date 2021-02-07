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
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;
using amp.Remote.DataClasses;
using VPKSoft.LangLib;

// for remote control..
// for remote control..
// for the database access..

namespace amp.Remote.WCFRemote
{
    /// <summary>
    /// An implementation for the remote control interface for the amp#.
    /// </summary>
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
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
        ServiceHost ampRemoteHost;

        /// <summary>
        /// Gets or sets the remote control provider.
        /// </summary>
        /// <value>The remote control provider.</value>
        public static RemoteProvider RemoteProvider { get; set; }

        /// <summary>
        /// Initializes a ServiceHost class instance for the self-hosted remote control basic HTTP binding WCF API.
        /// </summary>
        /// <returns>True if the initialization was a success, otherwise false.</returns>
        public bool InitAmpRemote(RemoteProvider remoteProvider)
        {
            RemoteProvider = remoteProvider;

            // If not defined int the settings, we don't even try..
            if (!Program.Settings.RemoteControlApiWcf)
            {
                return false;
            }

            // Create a ServiceHost class instance
            ampRemoteHost = new ServiceHost(GetType(), ampRemoteAddress);

            try // try to create a self-hosted WCF HTTP binding..
            {
                ampRemoteAddress = new Uri(Program.Settings.RemoteControlApiWcfAddress); // From the settings we get this..

                // A data transfer of information of possibly thousands of songs require large buffers/capacity..
                ampRemoteHost.AddServiceEndpoint(typeof(IampRemote), new BasicHttpBinding { MaxReceivedMessageSize = 2147483647, MaxBufferPoolSize = 2147483647 }, string.Empty);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true // just enable HTTP
                };

                ampRemoteHost.Description.Behaviors.Add(smb); // Add the behavior..

                ampRemoteHost.Open(); // open the self-hosted WCF HTTP binding..
                return true; // if success..
            }
            catch (Exception ex)
            {
                // If the self-hosted WCF HTTP binding was defined in the settings, but failed, do inform the user
                MessageBox.Show(DBLangEngine.GetStatMessage("msgRemoteWCFFailed", "Remote control HTTP binding failed to initialize ({0}) with an exception {1}.|As in the WCF self-hosting web service failed to initialize.", Program.Settings.RemoteControlApiWcfAddress, ex.Message),
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
            return RemoteProvider.CurrentAlbum;
        }

        /// <summary>
        /// Plays the next song. The next song to be played depends on the queue, random and shuffle states of the program.
        /// </summary>
        public void NextSong()
        {
            RemoteProvider.GetNextSong();
        }

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        public void PreviousSong()
        {
            RemoteProvider.GetPrevSong();
        }

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <param name="queued">If true only the queued songs are returned.</param>
        /// <returns>A list of songs in the current album.</returns>
        public List<AlbumSongRemote> GetAlbumSongs(bool queued = false)
        {
            return RemoteProvider.GetAlbumSongs(queued);
        }

        /// <summary>
        /// Gets the queued songs.
        /// </summary>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongRemote> GetQueuedSongs()
        {
            return RemoteProvider.GetAlbumSongs(true);
        }

        /// <summary>
        /// Gets the ID, length and position of the currently playing song.
        /// </summary>
        /// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>
        public Tuple<int, double, double> GetPlayingSong()
        {
            return RemoteProvider.MFile == null ? new Tuple<int, double, double>(-1, 0, 0) : new Tuple<int, double, double>(RemoteProvider.MFile.ID, RemoteProvider.Seconds, RemoteProvider.SecondsTotal);
        }

        /// <summary>
        /// Gets the current state of the amp# music player.
        /// </summary>
        /// <returns></returns>
        public PlayerStateRemote GetPlayerState()
        {
            return RemoteProvider.GetPlayerState();
        }

        /// <summary>
        /// Gets a value if the queue was changed from the previous query.
        /// </summary>
        /// <returns>True if the queue was changed from the previous query, otherwise false.</returns>
        public bool QueueChanged()
        {
            return RemoteProvider.QueueChanged();
        }

        /// <summary>
        /// Gets a value if the play list of the current album was changed from the previous query.
        /// </summary>
        /// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>
        public bool AlbumPlayListChanged()
        {
            return RemoteProvider.AlbumPlayListChanged();
        }

        /// <summary>
        /// Gets a value if current music album was changed.
        /// </summary>
        /// <returns>True if current music album was changed, otherwise false.</returns>
        public bool AlbumChanged()
        {
            return RemoteProvider.AlbumChanged;
        }

        /// <summary>
        /// Seeks the current song to a given <paramref name="seconds"/>. This will not cause an error if the given position is out of range.
        /// </summary>
        /// <param name="seconds">The position to jump the playback to.</param>
        public void SetPositionSeconds(double seconds)
        {
            RemoteProvider.SetPositionSeconds(seconds);
        }

        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongRemote> Queue(bool insert, List<AlbumSongRemote> queueList)
        {
            RemoteProvider.Queue(insert, queueList);
            if (RemoteProvider.Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                RemoteProvider.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="songIDs">A list of songs ID's to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongRemote> QueueIDs(bool insert, List<int> songIDs)
        {
            RemoteProvider.Queue(insert, songIDs);
            if (RemoteProvider.Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                RemoteProvider.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Scrambles the queue to have new random indices.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueue()
        {
            return RemoteProvider.ScrambleQueue();
        }

        /// <summary>
        /// Scrambles the queue between the specified selected songs.
        /// </summary>
        /// <param name="scrambleIdList">A list of selected music file identifiers to scramble.</param>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueueSelected(List<int> scrambleIdList)
        {
            return RemoteProvider.ScrambleQueueSelected(scrambleIdList);
        }

        /// <summary>
        /// Gets a list of songs which properties were changed (name, volume, rating).
        /// </summary>
        /// <returns>A list of songs which properties have been changed in the current album.</returns>
        public List<AlbumSongRemote> GetChangedSongs()
        {
            return RemoteProvider.GetChangedSongs();
        }

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="songIDs">A list of song IDs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        public List<AlbumSongRemote> QueueID(bool insert, List<int> songIDs)
        {
            RemoteProvider.Queue(insert, songIDs);

            if (RemoteProvider.Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                RemoteProvider.ShowQueue();
            }

            return GetQueuedSongs();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a paused state.
        /// </summary>
        /// <returns>Returns true if the playback is in a paused state, otherwise false.</returns>
        public bool Paused()
        {
            return RemoteProvider.Paused();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a stopped state.
        /// </summary>
        /// <returns>Returns true if the playback is in a stopped state, otherwise false.</returns>
        public bool Stopped()
        {
            return RemoteProvider.Stopped();
        }

        /// <summary>
        /// Gets a value indicating if the playback is in a playing state.
        /// </summary>
        /// <returns>Returns true if the playback is in a playing state, otherwise false.</returns>
        public bool Playing()
        {
            return RemoteProvider.Playing();
        }

        /// <summary>
        /// Starts a playback. If no song is playing the next song depends on whether the randomization and/or shuffle is on or off.
        /// </summary>
        public void Play()
        {
            RemoteProvider.Play();
        }

        /// <summary>
        /// Pauses the playback if not already paused.
        /// </summary>
        public void Pause()
        {
            RemoteProvider.Pause();
        }


        /// <summary>
        /// Plays a song with a given ID.
        /// </summary>
        /// <param name="id">An ID for the song which to play.</param>
        public void PlayID(int id)
        {
            RemoteProvider.Play(id);
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is randomizing songs, otherwise false.</returns>
        public bool Randomizing()
        {
            return RemoteProvider.Randomizing;
        }

        /// <summary>
        /// Gets a value indicating whether the stack queue playback mode is enabled or disabled.
        /// </summary>
        /// <returns><c>true</c> if the stack queue playback mode is enabled, <c>false</c> otherwise.</returns>
        public bool StackQueue()
        {
            return RemoteProvider.StackQueue;
        }

        /// <summary>
        /// Sets a value indicating whether the stack queue playback mode is enabled.
        /// </summary>
        /// <param name="value">A value indicating if the stack queue playback mode should be on or off.</param>
        public void SetStackQueue(bool value)
        {
            RemoteProvider.StackQueue = value;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <param name="value">A value indicating if the randomization should be on or off.</param>
        public void SetRandomizing(bool value)
        {
            RemoteProvider.Randomizing = value;
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        public bool Shuffle()
        {
            return RemoteProvider.Shuffle;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        public void SetShuffle(bool value)
        {
            RemoteProvider.Shuffle = value;
        }

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the Shuffle() method the keep backwards compatibility.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        // ReSharper disable once IdentifierTypo
        public bool Suffle()
        {
            return RemoteProvider.Shuffle;
        }

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the SetShuffle() method the keep backwards compatibility.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        // ReSharper disable once IdentifierTypo
        public void SetSuffle(bool value)
        {
            RemoteProvider.Shuffle = value;
        }

        /// <summary>
        /// Removes a song from the current album. No changes to the file system are applied.
        /// </summary>
        /// <param name="song">A song to remove from the current album.</param>
        /// <returns>True if the remove operation was successful, i.e. the song was found and removed, otherwise false.</returns>
        public bool RemoveSongFromAlbum(AlbumSongRemote song)
        {
            try
            {
                RemoteProvider.RemoveSongFromAlbum(song);
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
        public bool SetSongVolume(float volume) // 0.0-2.0
        {
            return RemoteProvider.SetSongVolume(volume);
        }

        /// <summary>
        /// Sets the main volume of amp# software.
        /// </summary>
        /// <param name="volume">The volume to set for the amp# software.</param>
        /// <returns>True if the volume was set within acceptable range, otherwise false.</returns>
        public bool SetAmpVolume(float volume)
        {
            try
            {
                RemoteProvider.ProgramVolume = volume;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the volume for multiple songs with a given list of ID numbers.
        /// </summary>
        /// <param name="songIdList">The song identifier list.</param>
        /// <param name="volume">The volume to set for the given songs. This value must be between 0 and 2 where 1 means original volume.</param>
        /// <returns>True if the given list was valid and the and the given volume was within acceptable range, otherwise false.</returns>
        public bool SetVolumeMultiple(List<int> songIdList, float volume) // 0.0-2.0
        {
            return RemoteProvider.SetSongVolume(songIdList, volume);
        }


        /// <summary>
        /// Sets the rating (stars) of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="rating">A rating for the song. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a song was playing which rating to set and the given rating was within acceptable range, otherwise false.</returns>
        public bool SetRating(int rating) // 0-1000
        {
            return RemoteProvider.SetRating(rating);
        }

        /// <summary>
        /// Sets the rating (stars) for the songs matching the given song ID list. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="songIdList">The song identifier list.</param>
        /// <param name="rating">A rating for the songs. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a songs which rating to set was a valid list and the given rating was within acceptable range, otherwise false.</returns>
        public bool SetRatingMultiple(List<int> songIdList, int rating)
        {
            return RemoteProvider.SetRating(songIdList, rating);
        }

        /// <summary>
        /// Gets a list of saved queues for a given album ID.
        /// </summary>
        /// <param name="albumName">A name for of an album which queue list to get. A String.Empty returns saved queues for all albums.</param>
        /// <returns>A list of QueueEntryRemote class instances for the requested album.</returns>
        public List<SavedQueueRemote> GetQueueList(string albumName)
        {
            return RemoteProvider.GetQueueList(albumName);
        }

        /// <summary>
        /// Gets a list if albums stored in the amp# database.
        /// </summary>
        /// <returns>A list of AlbumRemote class instances indicating the albums in the amp# database.</returns>
        public List<AlbumRemote> GetAlbums()
        {
            return RemoteProvider.GetAlbums();
        }

        /// <summary>
        /// Changes a playing album to a given album name.
        /// </summary>
        /// <param name="albumName">A name of an album to play.</param>
        /// <returns>True if the album exists and was successfully changed, otherwise false.</returns>
        public bool SelectAlbum(string albumName)
        {
            return RemoteProvider.SelectAlbum(albumName);
        }

        /// <summary>
        /// Gets a list of saved queues for the current album.
        /// </summary>
        /// <returns>A list of QueueEntryRemote class instances for the current album.</returns>
        public List<SavedQueueRemote> GetQueueListCurrentAlbum()
        {
            return GetQueueList(RemoteProvider.CurrentAlbum);
        }

        /// <summary>
        /// Loads a queue to the play list with a given unique ID.
        /// </summary>
        /// <param name="queueId">An index a queue to load for the playback for the current album.</param>
        public void LoadQueue(int queueId)
        {
            RemoteProvider.RefreshLoadQueueStats(queueId, false);
        }

        /// <summary>
        /// Appends a queue to the play list with a given unique ID preserving the current queue.
        /// </summary>
        /// <param name="queueId">An index a queue to load for the playback for the current album.</param>
        public void AppendQueue(int queueId)
        {
            RemoteProvider.RefreshLoadQueueStats(queueId, true);
        }

        /// <summary>
        /// Gets a value indicating if a previous song can be selected from the amp# play list.
        /// </summary>
        /// <returns>A value indicating if a previous song can be selected from the amp# play list.</returns>
        public bool CanGoPrevious()
        {
            return RemoteProvider.CanGoPrevious;
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
        [Obsolete("Too hard to maintain.")]
        public string GetMemberDescriptions()
        {
            return string.Empty;
        }
    }
}