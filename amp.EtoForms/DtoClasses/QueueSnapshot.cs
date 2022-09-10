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

namespace amp.EtoForms.DtoClasses;

/// <summary>
/// An entity to save queues into the database.
/// Implements the <see cref="IQueueSnapshot" />
/// </summary>
/// <seealso cref="IQueueSnapshot" />
public class QueueSnapshot : IQueueSnapshot, INotifyPropertyChanged
{
    private long id;
    private long albumId;
    private string snapshotName = string.Empty;
    private DateTime snapshotDate;
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private IList<QueueTrack>? queueTracks;
    private Album? album;

    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    public long Id
    {
        get => id;
        set => SetField(ref id, value);
    }

    /// <inheritdoc cref="IQueueSnapshot.AlbumId"/>
    public long AlbumId
    {
        get => albumId;
        set => SetField(ref albumId, value);
    }

    /// <inheritdoc cref="IQueueSnapshot.SnapshotName"/>
    public string SnapshotName
    {
        get => snapshotName;
        set => SetField(ref snapshotName, value);
    }

    /// <inheritdoc cref="IQueueSnapshot.SnapshotDate"/>
    public DateTime SnapshotDate
    {
        get => snapshotDate;
        set => SetField(ref snapshotDate, value);
    }

    /// <inheritdoc cref="IModifiedAt.ModifiedAtUtc"/>
    public DateTime? ModifiedAtUtc
    {
        get => modifiedAtUtc;
        set => SetField(ref modifiedAtUtc, value);
    }

    /// <inheritdoc cref="ICreatedAt.CreatedAtUtc"/>
    public DateTime CreatedAtUtc
    {
        get => createdAtUtc;
        set => SetField(ref createdAtUtc, value);
    }

    /// <summary>
    /// Gets or sets the queue tracks belonging to this queue snapshot.
    /// </summary>
    /// <value>The queued tracks.</value>
    public IList<QueueTrack>? QueueTracks
    {
        get => queueTracks;
        set => SetField(ref queueTracks, value);
    }

    /// <summary>
    /// Gets or sets the album of the queue snapshot.
    /// </summary>
    /// <value>The album of the queue snapshot.</value>
    public Album? Album
    {
        get => album;
        set => SetField(ref album, value);
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
    /// Sets the property backing field value and calls the <see cref="OnPropertyChanged"/> for the field property.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <param name="field">The field which value to set.</param>
    /// <param name="value">The value set for the field.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns><c>true</c> if the field value was changed and set, <c>false</c> otherwise.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}