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

using amp.Shared.Extensions;
using Eto.Drawing;

namespace amp.EtoForms.Forms;

partial class FormDialogTrackInfo
{
    private void ModifyTagData()
    {
        trackTag.Album = tbAlbumTag.Text;
        trackTag.Artist = tbArtistTag.Text;
        trackTag.Title = tbTitleTag.Text;
        trackTag.Comment = tbCommentTag.Text;
        trackTag.AlbumArtist = tbAlbumArtistTag.Text;
        trackTag.OriginalArtist = tbOriginalArtistTag.Text;
        trackTag.Copyright = tbCopyrightTag.Text;
        trackTag.Genre = tbGenreTag.Text;
        trackTag.Conductor = tbConductorTag.Text;
        trackTag.TrackNumber = tbTrackNumberTag.Value;
        trackTag.TrackTotal = tbTrackTotalTag.Value;
        trackTag.Year = tbYearTag.Value;
        trackTag.Lyrics.UnsynchronizedLyrics = tbLyricsTag.Text;
    }

    private void ModifyAmpData()
    {
        audioTrack.Album = tbAlbumAmp.Text;
        audioTrack.Artist = tbArtistAmp.Text;
        audioTrack.Title = tbTitleAmp.Text;
        audioTrack.Track = tbTrackNumberAmp.Text;
        audioTrack.Year = tbYearAmp.Text;
        audioTrack.Lyrics = tbLyricsAmp.Text;
        audioTrack.ModifiedAtUtc = DateTime.UtcNow;
        if (copiedFromTag != null)
        {
            using var memoryStream = new MemoryStream();
            copiedFromTag.Save(memoryStream, ImageFormat.Jpeg);
            audioTrack.TrackImageData = memoryStream.ToArray();
        }
        else
        {
            audioTrack.TrackImageData = null;
        }
    }

    private void CopyToAmp()
    {
        if (embeddedImageIndex != -1)
        {
            imageViewAmp.Image = new Bitmap(embeddedImages[embeddedImageIndex], 300, 300);
            copiedFromTag = new Bitmap(embeddedImages[embeddedImageIndex]);
        }

        tbArtistAmp.Text = tbArtistTag.Text;
        tbAlbumAmp.Text = tbAlbumTag.Text;
        tbTrackNumberAmp.Text = tbTrackNumberTag.Text;
        tbYearAmp.Text = tbYearTag.Text;
        tbLyricsAmp.Text = tbLyricsTag.Text;
        tbTitleAmp.Text = tbTitleTag.Text;
        tcTagTabs.SelectedIndex = 1;
        AmpDataChanged = true;
    }

    private void CopyToTag()
    {
        tbArtistTag.Text = tbArtistAmp.Text;
        tbAlbumTag.Text = tbAlbumAmp.Text;
        tbTrackNumberTag.Text = tbTrackNumberAmp.Text;
        tbYearTag.Text = tbYearAmp.Text;
        tbLyricsTag.Text = tbLyricsAmp.Text;
        tbTitleTag.Text = tbTitleAmp.Text;
        tcTagTabs.SelectedIndex = 0;
        TagDataChanged = true;
    }

    private void AttachEvents()
    {
        tbAlbumTag.TextChanged += TagDataTextChanged;
        tbArtistTag.TextChanged += TagDataTextChanged;
        tbTitleTag.TextChanged += TagDataTextChanged;
        tbCommentTag.TextChanged += TagDataTextChanged;
        tbAlbumArtistTag.TextChanged += TagDataTextChanged;
        tbOriginalArtistTag.TextChanged += TagDataTextChanged;
        tbCopyrightTag.TextChanged += TagDataTextChanged;
        tbGenreTag.TextChanged += TagDataTextChanged;
        tbConductorTag.TextChanged += TagDataTextChanged;
        tbTrackNumberTag.TextChanged += TagDataTextChanged;
        tbTrackTotalTag.TextChanged += TagDataTextChanged;
        tbYearTag.TextChanged += TagDataTextChanged;
        tbBitRate.TextChanged += TagDataTextChanged;
        tbAudioChannels.TextChanged += TagDataTextChanged;
        tbSampleRate.TextChanged += TagDataTextChanged;
        tbDuration.TextChanged += TagDataTextChanged;
        tbAudioFormat.TextChanged += TagDataTextChanged;
        tbFileName.TextChanged += TagDataTextChanged;
        tbFileNameFull.TextChanged += TagDataTextChanged;
        tbFileSizeBytes.TextChanged += TagDataTextChanged;
        tbLyricsTag.TextChanged += TagDataTextChanged;

        tbAlbumAmp.TextChanged += AmpDataTextChanged;
        tbArtistAmp.TextChanged += AmpDataTextChanged;
        tbTitleAmp.TextChanged += AmpDataTextChanged;
        tbTrackNumberAmp.TextChanged += AmpDataTextChanged;
        tbYearAmp.TextChanged += AmpDataTextChanged;
        tbLyricsAmp.TextChanged += AmpDataTextChanged;
    }

    private void SetData()
    {
        if (audioTrack.TrackImageData is { Length: > 0, })
        {
            using var stream = new MemoryStream(audioTrack.TrackImageData);
            var bitmap = new Bitmap(stream);
            imageViewAmp.Image = bitmap;
            copiedFromTag = bitmap;
        }
        else
        {
            imageViewAmp.Image = imageOff;
        }

        if (trackTag.EmbeddedPictures.Any())
        {
            foreach (var trackTagEmbeddedPicture in trackTag.EmbeddedPictures)
            {
                if (trackTagEmbeddedPicture.PictureData.Length > 0)
                {
                    using var stream = new MemoryStream(trackTagEmbeddedPicture.PictureData);
                    var bitmap = new Bitmap(stream);
                    embeddedImages.Add(bitmap);
                }
            }

            embeddedImageIndex = 0;
            imageViewTag.Image = embeddedImages[embeddedImageIndex];
        }
        else
        {
            imageViewTag.Image = imageOff;
        }

        tbAlbumTag.Text = trackTag.Album;
        tbArtistTag.Text = trackTag.Artist;
        tbTitleTag.Text = trackTag.Title;
        tbCommentTag.Text = trackTag.Comment;
        tbAlbumArtistTag.Text = trackTag.AlbumArtist;
        tbOriginalArtistTag.Text = trackTag.OriginalArtist;
        tbCopyrightTag.Text = trackTag.Copyright;
        tbGenreTag.Text = trackTag.Genre;
        tbConductorTag.Text = trackTag.Conductor;
        tbTrackNumberTag.Text = $"{trackTag.TrackNumber}";
        tbTrackTotalTag.Text = $"{trackTag.TrackTotal}";
        tbYearTag.Text = $"{trackTag.Year}";
        tbBitRate.Text = $"{trackTag.Bitrate}";
        tbAudioChannels.Text = $"{trackTag.ChannelsArrangement.NbChannels}";
        tbSampleRate.Text = $"{trackTag.SampleRate:F0}";
        tbDuration.Text = $"{TimeSpan.FromSeconds(trackTag.Duration):hh\\:mm\\:ss}";
        tbAudioFormat.Text = $"{trackTag.AudioFormat.Name}";
        tbFileName.Text = $"{audioTrack.FileName}";
        tbFileNameFull.Text = $"{audioTrack.FileNameFull()}";
        tbFileSizeBytes.Text = $"{audioTrack.FileSizeBytes}";
        tbLyricsTag.Text = $"{trackTag.Lyrics.UnsynchronizedLyrics}";

        tbAlbumAmp.Text = audioTrack.Album;
        tbArtistAmp.Text = audioTrack.Artist;
        tbTitleAmp.Text = audioTrack.Title;
        tbTrackNumberAmp.Text = audioTrack.Track;
        tbYearAmp.Text = audioTrack.Year;
        tbLyricsAmp.Text = audioTrack.Lyrics;
    }

    private void UpdateButtonsEnabled()
    {
        btnSaveChanges.Visible = tcTagTabs.SelectedIndex == 0 && tagDataChanged ||
                                 tcTagTabs.SelectedIndex == 1 && ampDataChanged;
        btnClearImage.Visible = tcTagTabs.SelectedIndex == 1 && audioTrack.TrackImageData?.Length > 0;
    }
}