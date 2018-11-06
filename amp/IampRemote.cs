#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace amp
{
    // netsh http add urlacl url=http://+:11316/ampRemote/ user="%USERNAME%"
    // netsh http delete urlacl url=http://+:11316/ampRemote/
    // netsh http show urlacl url=http://+:11316/ampRemote/    
    // VPKSoft --> ASCII = 653, V=86, P=80, K=75, S=83, o=111, f=102, t=116

    /// <summary>
    /// Remote control interface for the amp#
    /// </summary>
    [ServiceContract(Namespace = "http://ampRemote")]
    public interface IampRemote
    {
        /// <summary>
        /// Gets the name of the current album.
        /// </summary>
        /// <returns>The name of the current album</returns>
        [OperationContract]
        string GetAlbumName();

        /// <summary>
        /// Plays the next song. The next song to be played depends on the queue, random and shuffle states of the program.
        /// </summary>
        [OperationContract()]
        void NextSong();

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        [OperationContract]
        void PreviousSong();

        /// <summary>
        /// Gets a value indicating if a previous song can be selected from the amp# play list.
        /// </summary>
        /// <returns>A value indicating if a previous song can be selected from the amp# play list.</returns>
        [OperationContract]
        bool CanGoPrevious();

        /// <summary>
        /// Gets the songs in the current album. This can take a while if there are thousands of songs in the album.
        /// </summary>
        /// <param name="queued">If true only the queued songs are returned.</param>
        /// <returns>A list of songs in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> GetAlbumSongs(bool queued = false);

        /// <summary>
        /// Gets the queued songs.
        /// </summary>
        /// <returns>A list of queued songs in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> GetQueuedSongs();

        /// <summary>
        /// Gets the ID, length and position of the currently playing song.
        /// </summary>
        /// <returns>A Tuple containing the ID, length and position of the currently playing song. If no song is playing the ID will be -1.</returns>
        [OperationContract]
        Tuple<int, double, double> GetPlayingSong();

        /// <summary>
        /// Gets the current state of the amp# music player.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        PlayerState GetPlayerState();

        /// <summary>
        /// Seeks the current song to a given <paramref name="seconds"/>. This will not cause an error if the given position is out of range.
        /// </summary>
        /// <param name="seconds">The position to jump the playback to.</param>
        [OperationContract]
        void SetPositionSeconds(double seconds);

        /// <summary>
        /// Gets a value if the queue was changed from the previous query.
        /// </summary>
        /// <returns>True if the queue was changed from the previous query, otherwise false.</returns>
        [OperationContract]
        bool QueueChanged();

        /// <summary>
        /// Gets a value if the play list of the current album was changed from the previous query.
        /// </summary>
        /// <returns>True if the play list of the current album was changed from the previous query, otherwise false.</returns>
        [OperationContract]
        bool AlbumPlayListChanged();

        /// <summary>
        /// Gets a value if current music album was changed.
        /// </summary>
        /// <returns>True if current music album was changed, otherwise false.</returns>
        [OperationContract]
        bool AlbumChanged();

        /// <summary>
        /// Inserts or appends to the queue the given song list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> Queue(bool insert, List<AlbumSongWCF> queueList);

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="queueList">A list of songs ID's to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> QueueIDs(bool insert, List<int> songIDs);

        /// <summary>
        /// Gets a list of songs which properties were changed (name, volume, rating).
        /// </summary>
        /// <returns>A list of songs which properties have been changed in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> GetChangedSongs();

        /// <summary>
        /// Inserts or appends to the queue the given song ID list.
        /// </summary>
        /// <param name="insert">Whether to insert or append to the queue.</param>
        /// <param name="songID">A list of song IDs to be appended or inserted into the queue.</param>
        /// <returns>A list of queued songs in the current album.</returns>
        [OperationContract]
        List<AlbumSongWCF> QueueID(bool insert, List<int> songIDs);

        /// <summary>
        /// Gets a value indicating if the playback is in a paused state.
        /// </summary>
        /// <returns>Returns true if the playback is in a paused state, otherwise false.</returns>
        [OperationContract]
        bool Paused();

        /// <summary>
        /// Scrambles the queue to have new random indices.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        [OperationContract]
        bool ScrambleQueue();

        /// <summary>
        /// Pauses the playback if not already paused.
        /// </summary>
        [OperationContract]
        void Pause();

        /// <summary>
        /// Gets a value indicating if the playback is in a stopped state.
        /// </summary>
        /// <returns>Returns true if the playback is in a stopped state, otherwise false.</returns>
        [OperationContract]
        bool Stopped();

        /// <summary>
        /// Gets a value indicating if the playback is in a playing state.
        /// </summary>
        /// <returns>Returns true if the playback is in a playing state, otherwise false.</returns>
        [OperationContract]
        bool Playing();

        /// <summary>
        /// Starts a playback. If no song is playing the next song depends on whether the randomization and/or shuffle is on or off.
        /// </summary>
        [OperationContract]
        void Play();

        /// <summary>
        /// Plays a song with a given ID.
        /// </summary>
        /// <param name="ID">An ID for the song which to play.</param>
        [OperationContract]
        void PlayID(int ID);

        /// <summary>
        /// Gets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is randomizing songs, otherwise false.</returns>
        [OperationContract]
        bool Randomizing();

        /// <summary>
        /// Sets a value indicating if the playback "engine" is randomizing songs. Queued song do override randomization.
        /// </summary>
        /// <param name="value">A value indicating if the randomization should be on or off.</param>
        [OperationContract]
        void SetRandomizing(bool value);

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        [OperationContract]
        bool Shuffle();

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        [OperationContract]
        void SetShuffle(bool value);

        /// <summary>
        /// Gets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the Shuffle() method the keep backwards compatibility.
        /// </summary>
        /// <returns>Returns true if the playback "engine" is shuffling songs, otherwise false.</returns>
        [OperationContract]
        bool Suffle();

        /// <summary>
        /// Sets a value indicating if the playback "engine" is shuffling songs. Queued song do override shuffling.
        /// <para/>This is the miss-spelled version of the SetShuffle() method the keep backwards compatibility.
        /// </summary>
        /// <param name="value">A value indicating if the shuffling should be on or off.</param>
        [OperationContract]
        void SetSuffle(bool value);

        /// <summary>
        /// Removes a song from the current album. No changes to the file system are applied.
        /// </summary>
        /// <param name="song">A song to remove from the current album.</param>
        /// <returns>True if the remove operation was successful, i.e. the song was found and removed, otherwise false.</returns>
        [OperationContract]
        bool RemoveSongFromAlbum(AlbumSongWCF song);

        /// <summary>
        /// Sets the volume of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="volume">The volume to set for the currently playing song. This value must be between 0 and 2 where 1 means original volume.</param>
        /// <returns>True if a song was playing which volume to set and the given volume was within acceptable range, otherwise false.</returns>
        [OperationContract]
        bool SetVolume(float volume);

        /// <summary>
        /// Sets the volume for multiple songs with a given list of ID numbers.
        /// </summary>
        /// <param name="songIDList">The song identifier list.</param>
        /// <param name="volume">The volume to set for the given songs. This value must be between 0 and 2 where 1 means original volume.</param>
        /// <returns>True if the given list was valid and the and the given volume was within acceptable range, otherwise false.</returns>
        [OperationContract]
        bool SetVolumeMultiple(List<int> songIDList, float volume);

        /// <summary>
        /// Sets the rating (stars) of the currently playing song. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="rating">A rating for the song. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a song was playing which rating to set and the given rating was within acceptable range, otherwise false.</returns>
        [OperationContract]
        bool SetRating(int rating);

        /// <summary>
        /// Sets the rating (stars) for the songs matching the given song ID list. Do note that this does not reflect anyhow to the actual music file, only the amp# database is affected.
        /// </summary>
        /// <param name="songIDList">The song identifier list.</param>
        /// <param name="rating">A rating for the songs. This value must be between 0 and 1000 where 500 means a normal rating.</param>
        /// <returns>True if a songs which rating to set was a valid list and the given rating was within acceptable range, otherwise false.</returns>
        [OperationContract]
        bool SetRatingMultiple(List<int> songIDList, int rating);


        /// <summary>
        /// Gets a list of saved queues for a given album ID.
        /// </summary>
        /// <param name="albumName">A name for of an album which queue list to get. A String.Empty returns saved queues for all albums.</param>
        /// <returns>A list of QueueEntry class instances for the requested album.</returns>
        [OperationContract]
        List<QueueEntry> GetQueueList(string albumName);

        /// <summary>
        /// Gets a list of saved queues for the current album.
        /// </summary>
        /// <returns>A list of QueueEntry class instances for the current album.</returns>
        [OperationContract]
        List<QueueEntry> GetQueueListCurrentAlbum();

        /// <summary>
        /// Loads a queue to the play list with a given unique ID.
        /// </summary>
        /// <param name="QueueID">An index a queue to load for the playback for the current album.</param>
        [OperationContract]
        void LoadQueue(int QueueID);

        /// <summary>
        /// Appends a queue to the play list with a given unique ID preserving the current queue.
        /// </summary>
        /// <param name="QueueID">An index a queue to load for the playback for the current album.</param>
        [OperationContract]
        void AppendQueue(int QueueID);

        /// <summary>
        /// Gets a list if albums stored in the amp# database.
        /// </summary>
        /// <returns>A list of AlbumWCF class instances indicating the albums in the amp# database.</returns>
        [OperationContract]
        List<AlbumWCF> GetAlbums();

        /// <summary>
        /// Changes a playing album to a given album name.
        /// </summary>
        /// <param name="albumName">A name of an album to play.</param>
        /// <returns>True if the album exists and was successfully changed, otherwise false.</returns>
        [OperationContract]
        bool SelectAlbum(string albumName);

        /// <summary>
        /// Gets the descriptions of all the public members of a class implementing this interface.
        /// </summary>
        /// <returns>The descriptions of all the public members of a class implementing this interface.</returns>
        [OperationContract]
        string GetMemberDescriptions();

        /// <summary>
        /// Just a simple method to check if the connection is working.
        /// </summary>
        /// <returns>Always returns true.</returns>
        [OperationContract]
        bool ConnectionTest();
    }

    /// <summary>
    /// A class to present the player state all at once.
    /// </summary>
    [DataContract]
    public class PlayerState
    {
        /// <summary>
        /// Indicates if the player is in random mode.
        /// </summary>
        [DataMember]
        public bool Random { get; set; }

        /// <summary>
        /// Indicates if the player is in shuffle mode.
        /// </summary>
        [DataMember]
        public bool Shuffle { get; set; }

        /// <summary>
        /// Indicates if the player is paused.
        /// </summary>
        [DataMember]
        public bool Paused { get; set; }

        /// <summary>
        /// Indicates if the player neither paused or played, meaning it is stopped.
        /// </summary>
        [DataMember]
        public bool Stopped { get; set; }

        /// <summary>
        /// Indicates if the queue has been changed from the previous query.
        /// </summary>
        [DataMember]
        public bool QueueChangedFromPreviousQuery { get; set; }

        /// <summary>
        /// Gets a number of songs in the queue.
        /// </summary>
        [DataMember]
        public int QueueCount { get; set; }

        /// <summary>
        /// Gets a an ID of the current song.
        /// </summary>
        [DataMember]
        public int CurrentSongID { get; set; }

        /// <summary>
        /// Gets a name of the current song.
        /// </summary>
        [DataMember]
        public string CurrentSongName { get; set; }

        /// <summary>
        /// Gets a position of the current song.
        /// </summary>
        [DataMember]
        public double CurrentSongPosition { get; set; }

        /// <summary>
        /// Gets the length of the current song.
        /// </summary>
        [DataMember]
        public double CurrentSongLength { get; set; }

        /// <summary>
        /// Gets the currently selected album name of the amp# audio player.
        /// </summary>
        [DataMember]
        public string CurrentAlbumName { get; set; }

        /// <summary>
        /// Gets a value indicating if the album was changed from the previous query.
        /// </summary>
        [DataMember]
        public bool AlbumChanged { get; set; }

        /// <summary>
        /// Gets a value indicating if the album's contents where changed from the previous query.
        /// </summary>
        [DataMember]
        public bool AlbumContentsChanged { get; set; }

        /// <summary>
        /// Gets a value indicating if the album's song properties where changed.
        /// </summary>
        [DataMember]
        public bool SongsChanged { get; set; }

        /// <summary>
        /// Gets a value indicating if a previous song can be selected from the amp# play list.
        /// </summary>
        [DataMember]
        public bool CanGoPrevious { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an album is loading.
        /// </summary>
        [DataMember]
        public bool AlbumLoading { get; set; }
    }

    /// <summary>
    /// A class indicating a single saved queue in the amp# database.
    /// </summary>
    [DataContract]
    public class QueueEntry
    {
        /// <summary>
        /// Gets an unique database ID number for a saved queue.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets a name for a saved queue.
        /// </summary>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// Gets date and time when the queue was created/modified.
        /// </summary>
        [DataMember]
        public DateTime CreteDate { get; set; }
    }

    /// <summary>
    /// Gets a list of albums stored in the amp# database.
    /// </summary>
    [DataContract]
    public class AlbumWCF
    {
        /// <summary>
        /// An album name in the amp# database.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// A class indicating a single song in the amp# database.
    /// </summary>
    [DataContract]
    public class AlbumSongWCF
    {
        /// <summary>
        /// Gets an unique database ID number for the song.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets a queue index of a song. A zero value indicates that a song is not in the queue.
        /// </summary>
        [DataMember]
        public int QueueIndex { get; set; }

        /// <summary>
        /// Gets a value indicating the song's volume.
        /// </summary>
        [DataMember]
        public float Volume { get; set; }

        /// <summary>
        /// Gets a value indicating the song's rating.
        /// </summary>
        [DataMember]
        public int Rating { get; set; }

        /// <summary>
        /// Gets a value of the song's duration in seconds.
        /// </summary>
        [DataMember]
        public int Duration { get; set; }

        /// <summary>
        /// Gets the artist of the song.
        /// </summary>
        [DataMember]
        public string Artist { get; set; }

        /// <summary>
        /// Gets the album of the song.
        /// <note type="note">This is not an amp# album, this is an ID3vX Tag Album.</note>
        /// </summary>
        [DataMember]
        public string Album { get; set; }

        /// <summary>
        /// Gets the title of the song.
        /// <note type="note">This is not an amp# title, this is an ID3vX Tag Title.</note>
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets a song name combined with the song's ID3vX Tag values and it's queue index in square brackets if the queue index is larger than zero.
        /// <note type="note">If the song is renamed via the amp# interface the value comes from the amp# database (as always, amp# does not reflect any changes to the file/file system).</note>
        /// </summary>
        [DataMember]
        public string SongName { get; set; }

        /// <summary>
        /// Gets a song name combined with the song's ID3vX Tag values.
        /// <note type="note">If the song is renamed via the amp# interface the value comes from the amp# database (as always, amp# does not reflect any changes to the file/file system).</note>
        /// </summary>
        [DataMember]
        public string SongNameNoQueue { get; set; }

        /// <summary>
        /// Gets the publishing year of the song. This is an ID3vX Tag value.
        /// </summary>
        [DataMember]
        public string Year { get; set; }

        /// <summary>
        /// Gets the track of the song. This is an ID3vX Tag value.
        /// </summary>
        [DataMember]
        public string Track { get; set; }

        /// <summary>
        /// Gets the full file name of the underlying file of the song.
        /// </summary>
        [DataMember]
        public string FullFileName { get; set; }

        /// <summary>
        /// Gets the name of the song if it was overridden via the amp#.
        /// <note type="note">As always, amp# does not reflect any changes to the file/file system. All "modified" data is preserved in a database.</note>
        /// </summary>
        [DataMember]
        public string OverrideName { get; set; }

        /// <summary>
        /// Gets a string combination of everything the ID3vX tag of the file contains. This is for search purposes and the information is in no way in readable format.
        /// </summary>
        [DataMember]
        public string TagStr { get; set; }

        /// <summary>
        /// Gets a song name combined with the song's ID3vX Tag values and it's queue index in square brackets if the queue index is larger than zero.
        /// <note type="note">If the song is renamed via the amp# interface the value comes from the amp# database (as always, amp# does not reflect any changes to the file/file system).</note>
        /// </summary>
        /// <returns>A song name combined with the song's ID3vX Tag values and it's queue index in square brackets if the queue index is larger than zero.</returns>
        public override string ToString()
        {
            return SongName;
        }
    }
}
