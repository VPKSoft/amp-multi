#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

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

namespace amp.EtoForms;

public partial class FormMain
{
    /// <summary>
    /// Gets or sets the current album identifier.
    /// </summary>
    /// <value>The current album identifier.</value>
    private long CurrentAlbumId
    {
        get => Globals.Settings.SelectedAlbum < 1 ? 1 : Globals.Settings.SelectedAlbum;

        set
        {
            if (value < 1)
            {
                return;
            }

            if (Globals.Settings.SelectedAlbum != value)
            {
                suspendAlbumChange = true;
                var index = albums.FindIndex(f => f.Id == value);
                cmbAlbumSelect.SelectedIndex = index;
                suspendAlbumChange = false;

                Globals.Settings.SelectedAlbum = value;
                Globals.SaveSettings();
            }
        }
    }

    private long SelectedAlbumTrackId
    {
        get
        {
            if (gvAudioTracks.SelectedItem != null)
            {
                var albumTrackId = ((Models.AlbumTrack)gvAudioTracks.SelectedItem).Id;
                return albumTrackId;
            }

            return 0;
        }
    }

    private IEnumerable<long> SelectedAlbumTrackIds
    {
        get
        {
            foreach (var selectedItem in gvAudioTracks.SelectedItems)
            {
                var trackId = ((Models.AlbumTrack)selectedItem).Id;
                yield return trackId;
            }
        }
    }

    private bool QueuedItemsInSelection
    {
        get
        {
            foreach (var selectedItem in gvAudioTracks.SelectedItems)
            {
                return ((Models.AlbumTrack)selectedItem).QueueIndex > 0;
            }

            return false;
        }
    }

    private bool AlternateQueuedItemsInSelection
    {
        get
        {
            foreach (var selectedItem in gvAudioTracks.SelectedItems)
            {
                return ((Models.AlbumTrack)selectedItem).QueueIndexAlternate > 0;
            }

            return false;
        }
    }

    private readonly object lockObject = new();

    private readonly Queue<KeyValuePair<string, DateTime>> displayMessageQueue = new();

    private Queue<KeyValuePair<string, DateTime>> DisplayMessageQueue
    {
        get
        {
            lock (lockObject)
            {
                return displayMessageQueue;
            }
        }
    }
}