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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using amp.EtoForms.Utilities;
using amp.Shared.Enumerations;
using amp.Shared.Interfaces;

namespace amp.EtoForms.Models;

/// <summary>
/// The track entity-independent representation.
/// Implements the <see cref="IAudioTrack" />
/// Implements the <see cref="INotifyPropertyChanged" />
/// </summary>
/// <seealso cref="IAudioTrack" />
/// <seealso cref="INotifyPropertyChanged" />
internal class AudioTrack : IAudioTrack, INotifyPropertyChanged
{
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private int? playedByRandomize;
    private int? playedByUser;
    private int? skippedEarlyCount;
    private string fileName = string.Empty;
    private string? artist;
    private string? album;
    private string? track;
    private string? year;
    private string? lyrics;
    private int? rating;
    private long? fileSizeBytes;
    private double playbackVolume;
    private string? overrideName;
    private string? tagFindString;
    private bool? tagRead;
    private string? fileNameNoPath;
    private string? title;
    private byte[]? songImageData;
    private MusicFileType musicFileType;

    public long Id { get; set; }

    /// <inheritdoc cref="IEntity.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc
    {
        get => modifiedAtUtc;

        set
        {
            if (modifiedAtUtc != value)
            {
                modifiedAtUtc = value;
                OnPropertyChanged(nameof(ModifiedAtUtc));
            }
        }
    }

    /// <inheritdoc cref="IEntity.CreatedAtUtc"/>
    public DateTime CreatedAtUtc
    {
        get => createdAtUtc;

        set
        {
            if (createdAtUtc != value)
            {
                createdAtUtc = value;
                OnPropertyChanged(nameof(CreatedAtUtc));
            }
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByRandomize"/>
    public int? PlayedByRandomize
    {
        get => playedByRandomize;

        set
        {
            if (playedByRandomize != value)
            {
                playedByRandomize = value;
                OnPropertyChanged(nameof(PlayedByRandomize));
            }
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.PlayedByUser"/>
    public int? PlayedByUser
    {
        get => playedByUser;

        set
        {
            if (playedByUser != value)
            {
                playedByUser = value;
                OnPropertyChanged(nameof(PlayedByUser));
            }
        }
    }

    /// <inheritdoc cref="IPlayBackStatistics.SkippedEarlyCount"/>
    public int? SkippedEarlyCount
    {
        get => skippedEarlyCount;

        set
        {
            if (skippedEarlyCount != value)
            {
                skippedEarlyCount = value;
                OnPropertyChanged(nameof(SkippedEarlyCount));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.FileName"/>
    public string FileName
    {
        get => fileName;

        set
        {
            if (fileName != value)
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Artist"/>
    public string? Artist
    {
        get => artist;

        set
        {
            if (artist != value)
            {
                artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Album"/>
    public string? Album
    {
        get => album;

        set
        {
            if (album != value)
            {
                album = value;
                OnPropertyChanged(nameof(Album));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Track"/>
    public string? Track
    {
        get => track;

        set
        {
            if (track != value)
            {
                track = value;
                OnPropertyChanged(nameof(Track));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Year"/>
    public string? Year
    {
        get => year;

        set
        {
            if (year != value)
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Lyrics"/>
    public string? Lyrics
    {
        get => lyrics;

        set
        {
            if (lyrics != value)
            {
                lyrics = value;
                OnPropertyChanged(nameof(Lyrics));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Rating"/>
    public int? Rating
    {
        get => rating;

        set
        {
            if (rating != value)
            {
                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.FileSizeBytes"/>
    public long? FileSizeBytes
    {
        get => fileSizeBytes;

        set
        {
            if (fileSizeBytes != value)
            {
                fileSizeBytes = value;
                OnPropertyChanged(nameof(FileSizeBytes));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.PlaybackVolume"/>
    public double PlaybackVolume
    {
        get => playbackVolume;

        set
        {
            if (Math.Abs(playbackVolume - value) > Globals.FloatingPointTolerance)
            {
                playbackVolume = value;
                OnPropertyChanged(nameof(PlaybackVolume));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.OverrideName"/>
    public string? OverrideName
    {
        get => overrideName;

        set
        {
            if (overrideName != value)
            {
                overrideName = value;
                OnPropertyChanged(nameof(OverrideName));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.TagFindString"/>
    public string? TagFindString
    {
        get => tagFindString;

        set
        {
            if (tagFindString != value)
            {
                tagFindString = value;
                OnPropertyChanged(nameof(TagFindString));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.TagRead"/>
    public bool? TagRead
    {
        get => tagRead;

        set
        {
            if (tagRead != value)
            {
                tagRead = value;
                OnPropertyChanged(nameof(TagRead));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.FileNameNoPath"/>
    public string? FileNameNoPath
    {
        get => fileNameNoPath;

        set
        {
            if (fileNameNoPath != value)
            {
                fileNameNoPath = value;
                OnPropertyChanged(nameof(FileNameNoPath));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.Title"/>
    public string? Title
    {
        get => title;

        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.TrackImageData"/>
    public byte[]? TrackImageData
    {
        get => songImageData;

        set
        {
            if (songImageData != value)
            {
                songImageData = value;
                OnPropertyChanged(nameof(TrackImageData));
            }
        }
    }

    /// <inheritdoc cref="IAudioTrack.MusicFileType"/>
    public MusicFileType MusicFileType
    {
        get => musicFileType;

        set
        {
            if (musicFileType != value)
            {
                musicFileType = value;
                OnPropertyChanged(nameof(MusicFileType));
            }
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
    public string DisplayName => SongDisplayNameGenerate.GetSongName(this);

    /// <summary>
    /// Called when property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
    }
}