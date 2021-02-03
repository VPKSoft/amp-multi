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
    /// A class indicating a single saved queue in the amp# database.
    /// </summary>
    [DataContract]
    public class QueueEntryRemote
    {
        /// <summary>
        /// Gets an unique database ID number for a saved queue.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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
}