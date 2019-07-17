﻿#region License
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using amp.UtilityClasses;
using TagLib;
using VPKSoft.LangLib;
using File = TagLib.File;

namespace amp
{
    public partial class FormTagInfo : DBLangEngineWinforms
    {
        public FormTagInfo()
        {
            InitializeComponent();

            DBLangEngine.DBName = "lang.sqlite";

            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }

            DBLangEngine.InitalizeLanguage("amp.Messages");
        }

        MusicFile mf;
        List<IPicture> pictures = new List<IPicture>();

        private int picIndex = -1;

        internal void LoadTag()
        {
            using (File tagFile = File.Create(mf.FullFileName))
            {
                if (tagFile.Tag.Title == null || tagFile.Tag.Title == string.Empty)
                {
                    Text = Path.GetFileNameWithoutExtension(mf.FullFileName);
                }
                else
                {
                    if (tagFile.Tag.FirstAlbumArtist != null && tagFile.Tag.FirstAlbumArtist != string.Empty)
                    {
                        Text = tagFile.Tag.FirstAlbumArtist + " - " + tagFile.Tag.Title;
                    }
                    else
                    {
                        Text = tagFile.Tag.Title;
                    }
                }

                tbAlbum.Text = tagFile.Tag.Album;
                tbArtists.Lines = tagFile.Tag.AlbumArtists;
                tbTitle.Text = tagFile.Tag.Title;
                tbPerformers.Lines = tagFile.Tag.Performers;
                tbComments.Text = tagFile.Tag.Comment;
                tbComposers.Lines = tagFile.Tag.Composers;
                tbCopyright.Text = tagFile.Tag.Copyright;
                tbGenres.Lines = tagFile.Tag.Genres;
                tbConductor.Text = tagFile.Tag.Conductor;
                tbTrack.Text = tagFile.Tag.Track.ToString();
                tbTrackCount.Text = tagFile.Tag.TrackCount.ToString();
                tbYear.Text = tagFile.Tag.Year.ToString();
                tbAudioBitrate.Text = tagFile.Properties.AudioBitrate.ToString();
                tbAudioChannels.Text = tagFile.Properties.AudioChannels.ToString();
                tbAudioSampleRate.Text = tagFile.Properties.AudioSampleRate.ToString();
                tbBitsPerSample.Text = tagFile.Properties.BitsPerSample.ToString();
                tbDuration.Text = tagFile.Properties.Duration.ToString(@"mm\:ss");
                tbFileName.Text = mf.FileNameNoPath;
                tbFullFileName.Text = mf.FullFileName;


                foreach (ICodec codec in tagFile.Properties.Codecs)
                {
                    tbCodecs.Text += codec.Description + ", ";
                }

                tbCodecs.Text = tbCodecs.Text.TrimEnd(' ', ',');

                pictures.AddRange(tagFile.Tag.Pictures);

                tbLyrics.Text = tagFile.Tag.Lyrics;

                ShowNextPic();
            }
            btOK.Focus();
        }

        internal void ShowNextPic()
        {
            picIndex++;
            if (picIndex >= pictures.Count)
            {
                picIndex = 0;
            }
            if (pictures.Count > 0)
            {
                MemoryStream ms = new MemoryStream(pictures[picIndex].Data.Data);
                ms.Position = 0;
                Image im = Image.FromStream(ms);
                pbAlbum.Image = im;
            }
        }

        internal void ShowPreviousPic()
        {
            picIndex--;
            if (picIndex < 0)
            {
                picIndex = 0;
            }
            if (pictures.Count > 0)
            {
                MemoryStream ms = new MemoryStream(pictures[picIndex].Data.Data);
                ms.Position = 0;
                Image im = Image.FromStream(ms);
                pbAlbum.Image = im;
            }
        }

        public static bool Execute(MusicFile mf, Form parent)
        {
            FormTagInfo frm = new FormTagInfo();
            frm.mf = mf;
            frm.Owner = parent;
            return frm.ShowDialog() == DialogResult.OK;
        }

        private void tsbNext_Click(object sender, EventArgs e)
        {
            ShowNextPic();
        }

        private void tsbPrevious_Click(object sender, EventArgs e)
        {
            ShowPreviousPic();
        }

        private void FormTagInfo_Shown(object sender, EventArgs e)
        {
            LoadTag();
        }

        // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
        private void lbFullFileName_Click(object sender, EventArgs e)
        {
            string argument = "/select, \"" + mf.FullFileName + "\"";

            Process.Start("explorer.exe", argument);
        }
    }
}
