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

namespace amp.EtoForms.Models;

/// <summary>
/// A class for album.
/// Implements the <see cref="IAlbum" />
/// Implements the <see cref="INotifyPropertyChanged" />
/// </summary>
/// <seealso cref="IAlbum" />
/// <seealso cref="INotifyPropertyChanged" />
public class Album : IAlbum, INotifyPropertyChanged
{
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private string albumName = string.Empty;

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

    /// <inheritdoc cref="IAlbum.AlbumName"/>
    public string AlbumName
    {
        get => albumName;

        set
        {
            if (albumName != value)
            {
                albumName = value;
                OnPropertyChanged(nameof(AlbumName));
            }
        }
    }

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
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return AlbumName;
    }
}