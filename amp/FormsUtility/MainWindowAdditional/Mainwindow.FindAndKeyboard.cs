#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

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

using System.Collections.Generic;
using System.Windows.Forms;
using amp.FormsUtility.Songs;
using amp.SQLiteDatabase;
using amp.UtilityClasses;
using VPKSoft.KeySendList;

// ReSharper disable once CheckNamespace
namespace amp
{
    public partial class FormMain
    {
        /// <summary>
        /// An enumeration describing the filter status of the playlist.
        /// </summary>
        internal enum FilterType
        {
            /// <summary>
            /// The playlist is filter with a search string.
            /// </summary>
            SearchFiltered,

            /// <summary>
            /// The playlist is showing queued songs.
            /// </summary>
            QueueFiltered,

            /// <summary>
            /// The playlist is showing song in the alternate queue.
            /// </summary>
            AlternateFiltered,

            /// <summary>
            /// The playlist is not filtered.
            /// </summary>
            NoneFiltered
        }

        /// <summary>
        /// Gets or sets the type of the playlist filtering.
        /// </summary>
        internal FilterType Filtered { get; set; } = FilterType.NoneFiltered; // if the list of files is somehow filtered..

        /// <summary>
        /// Finds the songs with the text in the search box.
        /// </summary>
        /// <param name="onlyIfText">if set to <c>true</c> an empty or white space in the search box doesn't affect the filtering.</param>
        private void Find(bool onlyIfText = false)
        {
            if (onlyIfText)
            {
                if (tbFind.Text.Trim() == string.Empty)
                {
                    return;
                }
            }
            lbMusic.Items.Clear();
            foreach (MusicFile mf in PlayList)
            {
                if (mf.Match(tbFind.Text))
                {
                    lbMusic.Items.Add(mf);
                }
            }
            Filtered = tbFind.Text != string.Empty ? FilterType.SearchFiltered : FilterType.NoneFiltered;
        }

        /// <summary>
        /// Handles the media key presses (play, pause, next, previous, etc).
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the key was handled by this method, <c>false</c> otherwise.</returns>
        private bool HandleMediaKey(ref KeyEventArgs e)
        {
            if (e.KeyCode == Keys.MediaPlayPause)
            {
                TogglePause();
                e.SuppressKeyPress = true;
                return true;
            }

            if (e.KeyCode == Keys.MediaNextTrack)
            {
                GetNextSong(true);
                e.SuppressKeyPress = true;
                return true;
            }

            if (e.KeyCode == Keys.MediaPreviousTrack)
            {
                GetPrevSong();
                e.SuppressKeyPress = true;
                return true;
            }

            if (e.KeyCode == Keys.MediaStop)
            {
                Pause(); // this software knows no stop..
                e.SuppressKeyPress = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles the key down event with the playlist box which is the other focusable control on the form besides the search box.
        /// If the key is none of the control keys the key is send to the search box and the search box is then focused.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void HandleKeyDown(ref KeyEventArgs e)
        {
            // the media keys are handled in a separate method..
            if (HandleMediaKey(ref e)) 
            {
                return;
            }

            if (e.KeyCode == Keys.Return)
            {
                if (lbMusic.SelectedItem != null)
                {
                    UpdateNPlayed(MFile, Skipped);
                    MFile = lbMusic.SelectedItem as MusicFile;
                    if (MFile != null)
                    {
                        latestSongIndex = MFile.VisualIndex;
                        UpdateNPlayed(MFile, false);
                    }

                    newSong = true;
                    e.Handled = true;
                }

                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                lbMusic.SuspendLayout();
                humanActivity.Enabled = false;
                List<MusicFile> removeList = new List<MusicFile>();
                for (int i = lbMusic.SelectedItems.Count - 1; i >= 0; i--)
                {
                    MusicFile mf = (lbMusic.SelectedItems[i] as MusicFile);
                    removeList.Add(mf);
                    lbMusic.Items.RemoveAt(lbMusic.SelectedIndices[i]);
                    if (mf != null)
                    {
                        MusicFile.RemoveById(ref PlayList, mf.ID);
                    }
                }
                Database.RemoveSongFromAlbum(CurrentAlbum, removeList, Connection);
                humanActivity.Enabled = true;
                lbMusic.ResumeLayout();
                return;
            }

            if (e.KeyCode == Keys.F2)
            {
                if (lbMusic.SelectedItem != null)
                {
                    humanActivity.Enabled = false;
                    MusicFile mf = lbMusic.SelectedItem as MusicFile;
                    string s = FormRename.Execute(mf);
                    Database.SaveOverrideName(ref mf, s, Connection);
                    lbMusic.RefreshItem(lbMusic.SelectedIndex);
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    humanActivity.Enabled = true;
                }

                return;
            }

            if (e.KeyCode == Keys.Add || e.KeyValue == 187)  // Do the queue, LOCATION::QUEUE
            {
                foreach (MusicFile mf in lbMusic.SelectedItems)
                {
                    if (e.Control)
                    {
                        if (playing || Filtered != FilterType.NoneFiltered)
                        {
                            mf.QueueInsert(ref PlayList, Filtered != FilterType.NoneFiltered, PlayList.IndexOf(MFile));
                        }
                        else
                        {
                            mf.QueueInsert(ref PlayList, Filtered != FilterType.NoneFiltered);
                        }
                    }
                    else
                    {
                        mf.Queue(ref PlayList, false);
                    }
                }
                lbMusic.RefreshItems();
                GetQueueCount();

                if (Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
                {
                    ShowQueue();
                }

                if (!PlayList.Exists(f => f.QueueIndex > 0)) // no empty queue..
                {
                    ShowPlayingSong();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Multiply)
            {
                foreach (MusicFile mf in lbMusic.SelectedItems)
                {
                    if (e.Control)
                    {
                        if (playing || Filtered != FilterType.NoneFiltered)
                        {
                            mf.QueueInsertAlternate(ref PlayList, Filtered != FilterType.NoneFiltered, PlayList.IndexOf(MFile));
                        }
                        else
                        {
                            mf.QueueInsertAlternate(ref PlayList, Filtered != FilterType.NoneFiltered);
                        }
                    }
                    else
                    {
                        mf.QueueAlternate(ref PlayList);
                    }
                }
                lbMusic.RefreshItems();

                if (Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
                {
                    ShowQueue();
                }

                if (!PlayList.Exists(f => f.QueueIndex > 0)) // no empty queue..
                {
                    ShowPlayingSong();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.PageDown ||
                e.KeyCode == Keys.PageUp ||
                e.KeyCode == Keys.Shift ||
                e.KeyCode == Keys.Control ||
                e.KeyCode == Keys.Return ||
                e.KeyCode == Keys.F1 ||
                e.KeyCode == Keys.F2 ||
                e.KeyCode == Keys.F4 ||
                e.KeyCode == Keys.F6 ||
                e.KeyCode == Keys.F7 ||
                e.KeyCode == Keys.F8 ||
                e.KeyCode == Keys.F9 ||
                e.Control && e.KeyCode == Keys.F7 && !e.Alt && !e.Shift ||
                e.Control && e.KeyCode == Keys.PageUp && !e.Alt && !e.Shift)
            {
                return;
            }

            if (char.IsLetterOrDigit((char)e.KeyValue) || KeySendList.HasKey(e.KeyCode))
            {
                tbFind.SelectAll();
                tbFind.Focus();
                char key = (char)e.KeyValue;

                SendKeys.Send(
                    char.IsLetterOrDigit(key) ? key.ToString().ToLower() : KeySendList.GetKeyString(e.KeyCode));

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        // handle the key down of the playlist box..
        private void lbMusic_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(ref e);
        }
    }
}