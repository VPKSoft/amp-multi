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
using amp.Shared.Interfaces;

namespace amp.DataAccessLayer.DtoClasses;

/// <summary>
/// A class for album tracks.
/// Implements the <see cref="amp.Shared.Interfaces.IAlbumTrack{TAudioTrack, TAlbum}" />
/// Implements the <see cref="INotifyPropertyChanged" />
/// </summary>
/// <seealso cref="amp.Shared.Interfaces.IAlbumTrack{TAudioTrack, TAlbum}" />
/// <seealso cref="INotifyPropertyChanged" />
public sealed class AlbumTrack : IAlbumTrack<AudioTrack, Album>, INotifyPropertyChanged
{
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private long albumId;
    private long trackId;
    private int queueIndex;
    private int queueIndexAlternate;
    private AudioTrack? track;
    private Album? album;

    /// <summary>
    /// Reflects the possible changed into the specified entity class.
    /// </summary>
    /// <param name="albumTrack">The album track entity.</param>
    public void UpdateDataModel(amp.Database.DataModel.AlbumTrack? albumTrack)
    {
        if (albumTrack == null)
        {
            return;
        }

        albumTrack.ModifiedAtUtc = ModifiedAtUtc;
        albumTrack.QueueIndex = QueueIndex;
        albumTrack.QueueIndexAlternate = QueueIndexAlternate;
    }

    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    public long Id { get; set; }

    /// <inheritdoc cref="IModifiedAt.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc
    {
        get => modifiedAtUtc;

        set
        {
            if (modifiedAtUtc != value)
            {
                modifiedAtUtc = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc cref="ICreatedAt.CreatedAtUtc"/>
    public DateTime CreatedAtUtc
    {
        get => createdAtUtc;

        set
        {
            if (createdAtUtc != value)
            {
                createdAtUtc = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.AlbumId"/>
    public long AlbumId
    {
        get => albumId;

        set
        {
            if (albumId != value)
            {
                albumId = value;
                OnPropertyChanged();
            }
        }

    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.AudioTrackId"/>
    public long AudioTrackId
    {
        get => trackId;

        set
        {
            if (trackId != value)
            {
                trackId = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.QueueIndex"/>
    public int QueueIndex
    {
        get => queueIndex;

        set
        {
            if (queueIndex != value)
            {
                queueIndex = value;
                OnPropertyChanged();
            }
        }

    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.QueueIndexAlternate"/>
    public int QueueIndexAlternate
    {
        get => queueIndexAlternate;

        set
        {
            if (queueIndexAlternate != value)
            {
                queueIndexAlternate = value;
                OnPropertyChanged();
            }
        }

    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.AudioTrack"/>
    public AudioTrack? AudioTrack
    {
        get => track;

        set
        {
            if (track != value)
            {
                track = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.Album"/>
    public Album? Album
    {
        get => album;

        set
        {
            if (album != value)
            {
                album = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets the display name for the audio track.
    /// </summary>
    /// <value>The display name.</value>
    public string DisplayName => GenerateDisplayNameFunc?.Invoke(this) ?? string.Empty;

    /// <summary>
    /// Gets or sets the generate display name function.
    /// </summary>
    /// <value>The generate display name function.</value>
    public static Func<AlbumTrack, string>? GenerateDisplayNameFunc { get; set; }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

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