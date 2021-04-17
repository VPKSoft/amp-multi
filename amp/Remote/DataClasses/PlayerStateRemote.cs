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

using System.Runtime.Serialization;

namespace amp.Remote.DataClasses
{

    /// <summary>
    /// A class to present the player state all at once.
    /// </summary>
    [DataContract]
    public class PlayerStateRemote
    {
        /// <summary>
        /// Indicates if the player is in random mode.
        /// </summary>
        [DataMember]
        public bool Random { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the stack queue playback mode is enabled.
        /// </summary>
        [DataMember]
        public bool StackQueue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the amp# is playing a song.
        /// </summary>
        /// <value><c>true</c> if the amp# is playing a song; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool Playing { get; set; }

        /// <summary>
        /// Gets or sets the filter type of the UI.
        /// </summary>
        /// <value>The filter type of the UI.</value>
        [DataMember]
        public FilterType Filtered { get; set; }

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
        /// Gets an ID of the current song.
        /// </summary>
        [DataMember]
        public int CurrentSongId { get; set; }

        /// <summary>
        /// Gets the name of the current song.
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

        /// <summary>
        /// Gets or sets the amp# program's main volume.
        /// </summary>
        /// <value>The amp# program's main volume.</value>
        [DataMember]
        public float AmpVolume { get; set; }
    }
}
