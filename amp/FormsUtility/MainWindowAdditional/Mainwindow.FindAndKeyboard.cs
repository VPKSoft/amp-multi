#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

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

// ReSharper disable once CheckNamespace
namespace amp
{
    public partial class FormMain
    {
        internal enum FilterType
        {
            SearchFiltered,
            QueueFiltered,
            AlternateFiltered,
            NoneFiltered
        }

        internal FilterType Filtered { get; set; } = FilterType.NoneFiltered; // if the list of files is somehow filtered..

        private void Find(bool onlyIfText = false)
        {
            if (onlyIfText)
            {
                if (tbFind.Text == string.Empty)
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

        private void HandleKeyDown(ref KeyEventArgs e)
        {
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
                Database.RemoveSongFromAlbum(CurrentAlbum, removeList, Conn);
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
                    Database.SaveOverrideName(ref mf, s, Conn);
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
                        mf.Queue(ref PlayList);
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
                e.KeyCode == Keys.F9)
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

        private void lbMusic_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(ref e); // I had an intention, but I forgot it..
        }
    }
}