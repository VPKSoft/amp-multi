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

using amp.Shared.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace amp.EtoForms.DtoClasses;

/// <summary>
/// A DTO class for queue track data.
/// Implements the <see cref="IQueueTrack" />
/// </summary>
/// <seealso cref="IQueueTrack" />
public class QueueTrack : IQueueTrack, INotifyPropertyChanged
{
    private long id;
    private long audioTrackId;
    private long queueSnapshotId;
    private int queueIndex;
    private DateTime? modifiedAtUtc;
    private DateTime createdAtUtc;
    private AudioTrack audioTrack = new();

    /// <inheritdoc cref="IEntityBase{T}.Id"/>
    public long Id
    {
        get => id;
        set => SetField(ref id, value);
    }

    /// <inheritdoc cref="IQueueTrack.AudioTrackId"/>
    public long AudioTrackId
    {
        get => audioTrackId;
        set => SetField(ref audioTrackId, value);
    }

    /// <inheritdoc cref="IQueueTrack.QueueSnapshotId"/>
    public long QueueSnapshotId
    {
        get => queueSnapshotId;
        set => SetField(ref queueSnapshotId, value);
    }

    /// <inheritdoc cref="IQueueTrack.QueueIndex"/>
    public int QueueIndex
    {
        get => queueIndex;
        set => SetField(ref queueIndex, value);
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
    /// Gets or sets the audio track of this queue track.
    /// </summary>
    /// <value>The audio track of this queue track.</value>
    public AudioTrack AudioTrack
    {
        get => audioTrack;
        set => SetField(ref audioTrack, value);
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Called when [property changed].
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