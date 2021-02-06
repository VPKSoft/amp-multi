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
using System.Runtime.Serialization;

namespace amp.Remote.DataClasses
{
    /// <summary>
    /// A class indicating a single song in the amp# database.
    /// </summary>
    [DataContract]
    public class AlbumSongRemote
    {
        /// <summary>
        /// Gets an unique database ID number for the song.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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

        /// <summary>
        /// Checks if the properties of this music file instance matches the given search string.
        /// </summary>
        /// <param name="search">The search string.</param>
        /// <returns><c>true</c> if one of the properties of this music file instance matches the search string, <c>false</c> otherwise.</returns>
        public bool Match(string search)
        {
            if (search.Trim() == string.Empty)
            {
                return true;
            }
            search = search.ToUpper().Trim();
            bool found1 = Artist.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Album.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Title.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Year.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Track.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          FullFileName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          OverrideName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          TagStr.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1;

            string[] search2 = search.Split(' ');
            if (search2.Length <= 1 || found1)
            {
                return found1;
            }
            bool found2 = true;
            foreach(string str in search2)
            {
                var tmpStr = str.ToUpper();
                found2 &= Artist.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Album.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Title.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Year.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          Track.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          FullFileName.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          OverrideName.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                          TagStr.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) != -1;
            }
            return found2;
        }
    }
}
