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
using VPKSoft.KeySendList;

namespace amp
{
    public partial class MainWindow : DBLangEngineWinforms
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

        private bool HandleKeyDown(ref KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (lbMusic.SelectedItem != null)
                {
                    UpdateNPlayed(mFile, Skipped);
                    mFile = lbMusic.SelectedItem as MusicFile;
                    latestSongIndex = mFile.VisualIndex;
                    UpdateNPlayed(mFile, false);
                    newsong = true;
                    e.Handled = true;
                }
                return true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                lbMusic.SuspendLayout();
                humanActivity.Enabled = false;
                List<MusicFile> removeList = new List<MusicFile>();
                for (int i = lbMusic.SelectedItems.Count - 1; i >= 0; i--)
                {
                    MusicFile mf = (lbMusic.SelectedItems[i] as MusicFile);
                    removeList.Add(mf);
                    lbMusic.Items.RemoveAt(lbMusic.SelectedIndices[i]);
                    MusicFile.RemoveByID(ref PlayList, mf.ID);
                }
                Database.RemoveSongFromAlbum(CurrentAlbum, removeList, conn);
                humanActivity.Enabled = true;
                lbMusic.ResumeLayout();
                return true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (lbMusic.SelectedItem != null)
                {
                    humanActivity.Enabled = false;
                    MusicFile mf = lbMusic.SelectedItem as MusicFile;
                    string s = FormRename.Execute(mf);
                    Database.SaveOverrideName(ref mf, s, conn);
                    lbMusic.RefreshItem(lbMusic.SelectedIndex);
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    humanActivity.Enabled = true;
                }
                return true;
            }
            else if (e.KeyCode == Keys.Add || e.KeyValue == 187)  // Do the queue, LOCATION::QUEUE
            {
                foreach (MusicFile mf in lbMusic.SelectedItems)
                {
                    if (e.Control)
                    {
                        if (playing || Filtered != FilterType.NoneFiltered)
                        {
                            mf.QueueInsert(ref PlayList, Filtered != FilterType.NoneFiltered, PlayList.IndexOf(mFile));
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
                return true;
            }
            else if (e.KeyCode == Keys.Multiply)
            {
                foreach (MusicFile mf in lbMusic.SelectedItems)
                {
                    if (e.Control)
                    {
                        if (playing || Filtered != FilterType.NoneFiltered)
                        {
                            mf.QueueInsertAlternate(ref PlayList, Filtered != FilterType.NoneFiltered, PlayList.IndexOf(mFile));
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
                return true;
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
                return true;
            }

            if (char.IsLetterOrDigit((char)e.KeyValue) || KeySendList.HasKey(e.KeyCode))
            {
                tbFind.SelectAll();
                tbFind.Focus();
                char key = (char)e.KeyValue;

                if (char.IsLetterOrDigit(key))
                {
                    SendKeys.Send(key.ToString().ToLower());
                }
                else
                {
                    SendKeys.Send(KeySendList.GetKeyString(e.KeyCode));
                }
                e.SuppressKeyPress = true;
                e.Handled = true;
                return false;
            }

            return false;
        }

        private void lbMusic_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(ref e); // I had an intention, but I forgot it..
        }
    }
}