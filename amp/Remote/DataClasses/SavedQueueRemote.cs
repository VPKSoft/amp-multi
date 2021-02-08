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
using System.Runtime.Serialization;

namespace amp.Remote.DataClasses
{
    /// <summary>
    /// A class indicating a single saved queue in the amp# database.
    /// </summary>
    [DataContract]
    public class SavedQueueRemote
    {
        /// <summary>
        /// Gets or sets the database identifier for the saved queue.
        /// </summary>
        /// <value>The database identifier for the saved queue.</value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the saved queue.
        /// </summary>
        /// <value>The name of the saved queue.</value>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the saved queue.
        /// </summary>
        /// <value>The date and time of the saved queue.</value>
        [DataMember]
        public DateTime CreteDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the album the saved queue belongs to.
        /// </summary>
        /// <value>The name of the album the saved queue belongs to</value>
        [DataMember]
        public string AlbumName { get; set; }

        /// <summary>
        /// Gets or sets the total count of the queues belonging to this <see cref="AlbumName" /> album.
        /// </summary>
        /// <value>The total count of the queues belonging to this <see cref="AlbumName" /> album.</value>
        [DataMember]
        public int CountTotal { get; set; }

        /// <summary>
        /// Gets or sets the songs in the saved queue.
        /// </summary>
        /// <value>The  songs in the saved queue.</value>
        [DataMember]
        public List<AlbumSongRemote> QueueSongs { get; set; }
    }
}