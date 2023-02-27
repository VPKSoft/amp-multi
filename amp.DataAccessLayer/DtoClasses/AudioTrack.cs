#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using amp.Shared.Enumerations;
using amp.Shared.Interfaces;

namespace amp.DataAccessLayer.DtoClasses;

/// <summary>
/// The track entity-independent representation.
/// Implements the <see cref="IAudioTrack" />
/// Implements the <see cref="INotifyPropertyChanged" />
/// </summary>
/// <seealso cref="IAudioTrack" />
/// <seealso cref="INotifyPropertyChanged" />
public sealed class AudioTrack : IAudioTrack, INotifyPropertyChanged
{
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private int? playedByRandomize;
    private int? playedByUser;
    private int? skippedEarlyCount;
    private string fileName = string.Empty;
    private string filePath = string.Empty;
    private string? artist;
    private string? album;
    private string? track;
    private string? year;
    private string? lyrics;
    private int? rating;
    private long? fileSizeBytes;
    private bool ratingSpecified;
    private double playbackVolume;
    private string? overrideName;
    private string? tagFindString;
    private bool? tagRead;
    private string? title;
    private byte[]? trackImageData;
    private MusicFileType musicFileType;

    /// <summary>
    /// Reflects the possible changed into the specified entity class.
    /// </summary>
    /// <param name="audioTrack">The audio track entity.</param>
    public void UpdateDataModel(amp.Database.DataModel.AudioTrack? audioTrack)
    {
        if (audioTrack == null)
        {
            return;
        }

        audioTrack.ModifiedAtUtc = ModifiedAtUtc;
        audioTrack.PlaybackVolume = PlaybackVolume;
        audioTrack.Rating = Rating;
        audioTrack.ModifiedAtUtc = ModifiedAtUtc;
        audioTrack.PlayedByRandomize = PlayedByRandomize;
        audioTrack.PlayedByUser = PlayedByUser;
        audioTrack.SkippedEarlyCount = SkippedEarlyCount;
        audioTrack.FileName = FileName;
        audioTrack.Artist = Artist;
        audioTrack.Album = Album;
        audioTrack.Track = Track;
        audioTrack.Year = Year;
        audioTrack.Lyrics = Lyrics;
        audioTrack.MusicFileType = MusicFileType;
        audioTrack.TagFindString = TagFindString;
        audioTrack.TagRead = TagRead;
        audioTrack.FilePath = FilePath;
        audioTrack.Title = Title;
        audioTrack.TrackImageData = TrackImageData;
        audioTrack.OverrideName = OverrideName;
        audioTrack.FileSizeBytes = FileSizeBytes;
    }

    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    public long Id { get; set; }

    /// <inheritdoc cref="IModifiedAt.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc
    {
        get => modifiedAtUtc;

        set
        {
            if (modifiedAtUtc == value)
            {
                return;
            }

            modifiedAtUtc = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="ICreatedAt.CreatedAtUtc"/>
    public DateTime CreatedAtUtc
    {
        get => createdAtUtc;

        set
        {
            if (createdAtUtc == value)
            {
                return;
            }

            createdAtUtc = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByRandomize"/>
    public int? PlayedByRandomize
    {
        get => playedByRandomize;

        set
        {
            if (playedByRandomize == value)
            {
                return;
            }

            playedByRandomize = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByUser"/>
    public int? PlayedByUser
    {
        get => playedByUser;

        set
        {
            if (playedByUser == value)
            {
                return;
            }

            playedByUser = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.SkippedEarlyCount"/>
    public int? SkippedEarlyCount
    {
        get => skippedEarlyCount;

        set
        {
            if (skippedEarlyCount == value)
            {
                return;
            }

            skippedEarlyCount = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.FileName"/>
    public string FileName
    {
        get => fileName;

        set
        {
            if (fileName == value)
            {
                return;
            }

            fileName = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.FilePath"/>
    public string FilePath
    {
        get => filePath;
        set
        {
            if (filePath == value)
            {
                return;
            }

            filePath = value;
            OnPropertyChanged();
        }

    }

    /// <inheritdoc cref="IAudioTrack.Artist"/>
    public string? Artist
    {
        get => artist;

        set
        {
            if (artist == value)
            {
                return;
            }

            artist = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Album"/>
    public string? Album
    {
        get => album;

        set
        {
            if (album == value)
            {
                return;
            }

            album = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Track"/>
    public string? Track
    {
        get => track;

        set
        {
            if (track == value)
            {
                return;
            }

            track = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Year"/>
    public string? Year
    {
        get => year;

        set
        {
            if (year == value)
            {
                return;
            }

            year = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Lyrics"/>
    public string? Lyrics
    {
        get => lyrics;

        set
        {
            if (lyrics == value)
            {
                return;
            }

            lyrics = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Rating"/>
    public int? Rating
    {
        get => rating;

        set
        {
            if (rating == value)
            {
                return;
            }

            rating = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc  cref="IAudioTrack.FileSizeBytes" />
    public bool RatingSpecified
    {
        get => ratingSpecified;

        set
        {
            if (ratingSpecified == value)
            {
                return;
            }

            ratingSpecified = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.FileSizeBytes"/>
    public long? FileSizeBytes
    {
        get => fileSizeBytes;

        set
        {
            if (fileSizeBytes == value)
            {
                return;
            }

            fileSizeBytes = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.PlaybackVolume"/>
    public double PlaybackVolume
    {
        get => playbackVolume;

        set
        {
            if (!(Math.Abs(playbackVolume - value) > Globals.FloatingPointTolerance))
            {
                return;
            }

            playbackVolume = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.OverrideName"/>
    public string? OverrideName
    {
        get => overrideName;

        set
        {
            if (overrideName == value)
            {
                return;
            }

            overrideName = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.TagFindString"/>
    public string? TagFindString
    {
        get => tagFindString;

        set
        {
            if (tagFindString == value)
            {
                return;
            }

            tagFindString = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.TagRead"/>
    public bool? TagRead
    {
        get => tagRead;

        set
        {
            if (tagRead == value)
            {
                return;
            }

            tagRead = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.Title"/>
    public string? Title
    {
        get => title;

        set
        {
            if (title == value)
            {
                return;
            }

            title = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.TrackImageData"/>
    public byte[]? TrackImageData
    {
        get => trackImageData;

        set
        {
            if (trackImageData == value)
            {
                return;
            }

            trackImageData = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IAudioTrack.MusicFileType"/>
    public MusicFileType MusicFileType
    {
        get => musicFileType;

        set
        {
            if (musicFileType == value)
            {
                return;
            }

            musicFileType = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets the display name for the track.
    /// </summary>
    /// <value>The display name of the track.</value>
    public string DisplayName => GenerateDisplayNameFunc?.Invoke(this) ?? string.Empty;

    /// <summary>
    /// Gets or sets the generate display name function.
    /// </summary>
    /// <value>The generate display name function.</value>
    public static Func<AudioTrack, string>? GenerateDisplayNameFunc { get; set; }

    /// <summary>
    /// Called when property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
    }
}