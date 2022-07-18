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
using amp.Shared.Interfaces;

namespace amp.EtoForms.Models;
internal class AlbumTrack : IAlbumTrack<AudioTrack, Album>, INotifyPropertyChanged
{
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private long albumId;
    private long trackId;
    private int queueIndex;
    private int queueIndexAlternate;
    private AudioTrack? track;
    private Album? album;

    /// <inheritdoc cref="IEntityBase{T}.Id"/>
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

    /// <inheritdoc cref="IAlbumTrack{TAudioTrack, TAlbum}.AlbumId"/>
    public long AlbumId
    {
        get => albumId;

        set
        {
            if (albumId != value)
            {
                albumId = value;
                OnPropertyChanged(nameof(AlbumId));
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
                OnPropertyChanged(nameof(AudioTrackId));
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
                OnPropertyChanged(nameof(QueueIndex));
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
                OnPropertyChanged(nameof(QueueIndexAlternate));
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
                OnPropertyChanged(nameof(AudioTrack));
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
                OnPropertyChanged(nameof(Album));
            }
        }
    }

    public string DisplayName => this.GetAudioTrackName();

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

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