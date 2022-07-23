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

using System.Collections.ObjectModel;
using amp.Playback.Classes;
using amp.Playback.Interfaces;
using amp.Shared.Classes;
using amp.Shared.Extensions;
using amp.Shared.Interfaces;
using BR = VPKSoft.RandomizationUtils.BiasedRandom;

namespace amp.Playback;

/// <summary>
/// A class to generate audio tracks from a specified playlist in weighted randomized order.
/// Implements the <see cref="BiasedRandomSettingsBase" />
/// </summary>
/// <typeparam name="TAudioTrack">The type of the <see cref="IAlbumTrack{TAudioTrack,TAlbum}"/> <see cref="IAlbumTrack{TAudioTrack,TAlbum}.AudioTrack"/> member.</typeparam>
/// <typeparam name="TAlbumTrack">The type of the <see cref="IAlbumTrack{TAudioTrack,TAlbum}"/>.</typeparam>
/// <typeparam name="TAlbum">The type of the <see cref="IAlbumTrack{TAudioTrack, TAlbum}"/> <see cref="IAlbumTrack{TAudioTrack, TAlbum}.Album"/> member.</typeparam>
/// <seealso cref="BiasedRandomSettingsBase" />
public class PlaybackOrder<TAudioTrack, TAlbumTrack, TAlbum> : BiasedRandomSettingsBase where TAudioTrack : IAudioTrack where TAlbum : IAlbum where TAlbumTrack : class, IAlbumTrack<TAudioTrack, TAlbum>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackOrder{TAudioTrack, TAlbumTrack, TAlbum}"/> class.
    /// </summary>
    /// <param name="settings">The settings for biased randomization.</param>
    /// <param name="updateQueueFunc">A callback function to update the modified queue indices into the database.</param>
    public PlaybackOrder(IBiasedRandomSettings settings, Func<Dictionary<long, int>, bool, Task> updateQueueFunc)
    {
        base.ApplyFrom(settings);
        this.updateQueueFunc = updateQueueFunc;
    }

    /// <summary>
    /// Gets or sets a value indicating whether audio track index randomization is enabled.
    /// </summary>
    /// <value><c>true</c> if index randomization is enabled; otherwise, <c>false</c>.</value>
    public bool Randomizing { get; set; } = true;

    private int previousTrackIndex = -1;

    private readonly Func<Dictionary<long, int>, bool, Task> updateQueueFunc;

    private static int? GetQueueIndex(TAlbumTrack? albumTrack, bool alternate)
    {
        if (albumTrack == null)
        {
            return null;
        }

        return alternate ? albumTrack.QueueIndexAlternate : albumTrack.QueueIndex;
    }

    /// <summary>
    /// Toggles the queue of the album audio tracks specified by the <paramref name="albumTrackIds"/>.
    /// </summary>
    /// <param name="albumTracks">The album audio tracks.</param>
    /// <param name="alternate">A value indicating whether to manipulate the alternate queue.</param>
    /// <param name="insert">A value indicating whether the insert the tracks on top of the queue.</param>
    /// <param name="albumTrackIds">The audio tracks identifiers which queue to toggle.</param>
    public async Task ToggleQueue(ObservableCollection<TAlbumTrack>? albumTracks, bool alternate, bool insert, params long[] albumTrackIds)
    {
        if (albumTracks == null)
        {
            return;
        }

        var toUpdate = new Dictionary<long, int>();

        var newIndex = 1;

        if (insert)
        {
            var previouslyQueued = albumTracks.Where(f => GetQueueIndex(f, alternate) > 0).Select(f => f.Id).ToList();

            previouslyQueued = previouslyQueued.Where(f => !albumTrackIds.Contains(f)).ToList();

            foreach (var albumTrack in albumTracks.Where(f => albumTrackIds.Contains(f.Id)))
            {
                toUpdate.Add(albumTrack.Id, newIndex++);
            }

            foreach (var albumTrack in albumTracks.Where(f => previouslyQueued.Contains(f.Id))
                         .OrderBy(f => GetQueueIndex(f, alternate)))
            {
                toUpdate.Add(albumTrack.Id, newIndex++);
            }
        }
        else
        {
            var maxQueueIndex = albumTracks.Count(f => GetQueueIndex(f, alternate) > 0) + 1;

            foreach (var albumTrack in albumTracks.Where(f => albumTrackIds.Contains(f.Id)))
            {
                var index = GetQueueIndex(albumTrack, alternate) > 0 ? 0 : maxQueueIndex++;
                toUpdate.Add(albumTrack.Id, index);
            }

            foreach (var albumTrack in albumTracks.Where(f => GetQueueIndex(f, alternate) > 0).OrderBy(f => GetQueueIndex(f, alternate)))
            {
                if (GetQueueIndex(albumTrack, alternate) != newIndex)
                {
                    if (toUpdate.ContainsKey(albumTrack.Id))
                    {
                        toUpdate[albumTrack.Id] = newIndex;
                    }
                    else
                    {
                        toUpdate.Add(albumTrack.Id, newIndex);
                    }
                }

                newIndex++;
            }
        }

        if (toUpdate.Any())
        {
            await updateQueueFunc(toUpdate, alternate);
        }
    }

    /// <summary>
    /// Gets the next audio track to play.
    /// </summary>
    /// <param name="albumTracks">The album albumTracks.</param>
    /// <returns>An instance of the <see cref="AudioTrackResult"/> class.</returns>
    public async Task<AudioTrackResult> NextTrack(ObservableCollection<TAlbumTrack>? albumTracks)
    {
        if (albumTracks == null || albumTracks.Count == 0)
        {
            return AudioTrackResult.Empty;
        }

        bool randomized = false, queued = false;

        var nextTrackIndex = -1;
        var minQueueIndex = albumTracks.Where(f => f.QueueIndex > 0).DefaultIfEmpty().Min(f => f?.QueueIndex) ?? 0;
        var queueIndex = albumTracks.FindIndex(f => f.QueueIndex > 0 && f.QueueIndex == minQueueIndex);

        var updatedTracks = new Dictionary<long, int>();

        if (queueIndex != -1)
        {
            nextTrackIndex = queueIndex;
            queued = true;
            foreach (var track in albumTracks.Where(f => f.QueueIndex > 0))
            {
                updatedTracks.Add(track.Id, track.QueueIndex - 1);
            }

            if (updatedTracks.Count > 0)
            {
                await updateQueueFunc(updatedTracks, false);
            }
        }

        if (nextTrackIndex == -1 && Randomizing)
        {
            nextTrackIndex = BiasedRandom ? RandomWeighted(albumTracks) : Random.Next(0, albumTracks.Count);

            if (nextTrackIndex != -1)
            {
                randomized = true;
            }
        }

        if (nextTrackIndex == -1 && albumTracks.Any())
        {
            nextTrackIndex = previousTrackIndex + 1;
            if (nextTrackIndex >= albumTracks.Count)
            {
                nextTrackIndex = 0;
            }
        }

        previousTrackIndex = nextTrackIndex;

        return new AudioTrackResult
        {
            AudioTrackId = nextTrackIndex != -1 ? albumTracks[nextTrackIndex].AudioTrackId : 0,
            NextTrackIndex = nextTrackIndex,
            PlayedByRandomize = randomized ? 1 : 0,
            PlayedByUser = queued ? 1 : 0,
        };
    }

    /// <summary>
    /// Scrambles the current queue for the specified tracks with specified reference identifiers.
    /// </summary>
    /// <param name="tracks">The tracks which queue to scramble.</param>
    /// <param name="alternate">A value indicating whether to manipulate the alternate queue.</param>
    /// <param name="albumTrackIds">The audio tracks identifiers which queue to alter.</param>
    public async Task Scramble(IEnumerable<TAlbumTrack> tracks, bool alternate, params long[] albumTrackIds)
    {
        var reRandomIds = tracks
            .Where(f => albumTrackIds.Contains(f.Id) && GetQueueIndex(f, alternate) > 0)
            .Select(f => new KeyValuePair<long, int>(f.Id, GetQueueIndex(f, alternate)!.Value)).ToList();

        var randomIndices = reRandomIds.Select(f => f.Value).ToList();

        var toUpdate = new Dictionary<long, int>();

        var index = reRandomIds.Count - 1;

        while (reRandomIds.Count > 0 && index >= 0)
        {
            var randomIndex = Random.Next(randomIndices.Count);
            if (reRandomIds[index].Value != randomIndices[randomIndex] || reRandomIds.Count == 1)
            {
                toUpdate.Add(reRandomIds[index].Key, randomIndices[randomIndex]);
                reRandomIds.RemoveAt(index);
                randomIndices.RemoveAt(randomIndex);
                index--;
            }
        }

        if (toUpdate.Any())
        {
            await updateQueueFunc(toUpdate, alternate);
        }
    }

    /// <summary>
    /// Scrambles the current queue for the specified tracks.
    /// </summary>
    /// <param name="tracks">The tracks which queue to scramble.</param>
    /// <param name="alternate">A value indicating whether to manipulate the alternate queue.</param>
    public async Task Scramble(IEnumerable<TAlbumTrack> tracks, bool alternate)
    {
        var updateTracks = tracks.Where(f => GetQueueIndex(f, alternate) > 0).ToList();

        var usedIndices = new List<int>();

        var trackIndex = 0;

        var toUpdate = new Dictionary<long, int>();

        while (usedIndices.Count < updateTracks.Count)
        {
            var next = Random.Next(1, updateTracks.Count + 1);
            if ((GetQueueIndex(updateTracks[trackIndex], alternate) != next && !usedIndices.Contains(next)) ||
                (!usedIndices.Contains(next) && usedIndices.Count + 1 == updateTracks.Count))
            {
                if (toUpdate.ContainsKey(updateTracks[trackIndex].Id))
                {
                    toUpdate[updateTracks[trackIndex].Id] = next;
                }
                else
                {
                    toUpdate.Add(updateTracks[trackIndex].Id, next);
                }

                usedIndices.Add(next);
                trackIndex++;
            }
        }

        if (toUpdate.Any())
        {
            await updateQueueFunc(toUpdate, alternate);
        }
    }

    /// <summary>
    /// Clears the queue for the specified tracks.
    /// </summary>
    /// <param name="tracks">The tracks which queue index to clear..</param>
    /// <param name="alternate">A value indicating whether to manipulate the alternate queue.</param>
    public async Task ClearQueue(IEnumerable<TAlbumTrack> tracks, bool alternate)
    {
        var toUpdate = new Dictionary<long, int>();
        foreach (var albumTrack in tracks)
        {
            if (GetQueueIndex(albumTrack, alternate) != 0)
            {
                albumTrack.ModifiedAtUtc = DateTime.UtcNow;
                toUpdate.Add(albumTrack.Id, 0);
            }
        }

        if (toUpdate.Any())
        {
            await updateQueueFunc(toUpdate, alternate);
        }
    }

    /// <summary>
    /// Loops the current queue so that it will not be consumed in the process.
    /// The previous audio track is put to the bottom of the queue and the end of the queue is re-randomized.
    /// </summary>
    /// <param name="albumTracks">The audio tracks which queue to adjust.</param>
    /// <param name="previousTrack">The previously played audio track from the queue.</param>
    /// <param name="stackRandomPercentage">The stack random percentage.</param>
    /// <returns>A list of <typeparamref name="TAlbumTrack"/> instances which queue index was adjusted or <c>null</c> if there is nothing changed.</returns>
    // TODO::Refactor when the stack queue is actually implemented.
    internal async Task StackQueue(List<TAlbumTrack>? albumTracks, TAlbumTrack previousTrack, int stackRandomPercentage)
    {
        if (albumTracks == null || albumTracks.Count == 0)
        {
            return;
        }

        // Get the audio tracks with a queue index value.
        var queuedTracks =
            albumTracks.Where(f => f.QueueIndex > 0).
                OrderByDescending(f => f.QueueIndex).ToList();

        var result = queuedTracks.Select(f => new IdValuePair<int> { Id = f.AudioTrackId, Value = f.QueueIndex, }).ToList();

        // Get the amount of audio tracks in the queue to randomize to a new order.
        var randomizeCount = (int)(result.Count * (stackRandomPercentage / 100.0));

        // Zero or amount of the queue leads to do-nothing-condition.
        if (randomizeCount == 0 || randomizeCount == result.Count || stackRandomPercentage == 0)
        {
            return; // No modifications were made.
        }

        // Get the files which queue index to re-randomize.
        var reRandomFiles = result.Take(randomizeCount).ToList();

        var usedQueueIndices = new List<int>();

        // Get the min-max range for the randomization.
        var min = reRandomFiles.DefaultIfEmpty().Min(f => f?.Value) ?? 0;
        var max = reRandomFiles.DefaultIfEmpty().Max(f => f?.Value) ?? 0;

        // Loop through the files which queue index is to be re-randomized.
        foreach (var reRandomFile in reRandomFiles)
        {
            // The last music file won't be re-randomized.
            if (reRandomFile.Value == max)
            {
                continue;
            }

            // Random new unique queue indices.
            var newQueueIndex = Random.Next(min, max);
            while (usedQueueIndices.Contains(newQueueIndex))
            {
                newQueueIndex = Random.Next(min, max);
            }

            // Add the new randomized queue index to the list.
            usedQueueIndices.Add(newQueueIndex);

            // Save the queue index.
            reRandomFile.Value = newQueueIndex;
        }

        // Re-index the previous audio track to the last of the queue.
        var currentTrackReQueued = new IdValuePair<int>
        { Id = previousTrack.AudioTrackId, Value = result.DefaultIfEmpty().Max(f => f?.Value) + 1 ?? 1, };

        result.Add(currentTrackReQueued);

        var toUpdate = new Dictionary<long, int>(result.Select(f => new KeyValuePair<long, int>(f.Id, f.Value)));

        // Update the queue re-ordering.
        await updateQueueFunc(toUpdate, false);
    }

    private static bool InRange(double value, double randomValue, double min, double max, double tolerancePercentage)
    {
        var range = (max - min) / 100 * tolerancePercentage;
        return value <= randomValue + range && value >= randomValue - range;
    }

    private int RandomWeighted(ObservableCollection<TAlbumTrack>? albumTracks)
    {

        if (albumTracks == null || !albumTracks.Any())
        {
            return -1;
        }

        var results = new List<TAlbumTrack>();

        double valueMin = albumTracks.Min(f => f.AudioTrack?.Rating) ?? 0;
        double valueMax = albumTracks.Max(f => f.AudioTrack?.Rating) ?? 1000;

        var biased = BR.RandomBiased(valueMin, valueMax, BiasedRating);

        if (BiasedRatingEnabled)
        {
            results.AddRange(albumTracks.Where(f =>
                InRange(f.AudioTrack?.Rating ?? 500, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumTracks.Min(f => f.AudioTrack?.PlayedByUser ?? 0);
        valueMax = albumTracks.Max(f => f.AudioTrack?.PlayedByUser ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedPlayedCount);

        if (BiasedPlayedCountEnabled)
        {
            results.AddRange(albumTracks.Where(f =>
                InRange(f.AudioTrack?.PlayedByUser ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumTracks.Min(f => f.AudioTrack?.PlayedByRandomize ?? 0);
        valueMax = albumTracks.Max(f => f.AudioTrack?.PlayedByRandomize ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedRandomizedCount);

        if (BiasedRandomizedCountEnabled)
        {
            results.AddRange(albumTracks.Where(f =>
                InRange(f.AudioTrack?.PlayedByRandomize ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumTracks.Min(f => f.AudioTrack?.SkippedEarlyCount ?? 0);
        valueMax = albumTracks.Max(f => f.AudioTrack?.SkippedEarlyCount ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedSkippedCount);

        if (BiasedSkippedCountEnabled)
        {
            results.AddRange(albumTracks.Where(f =>
                InRange(f.AudioTrack?.SkippedEarlyCount ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        var result = -1;

        if (results.Count > 0)
        {
            var tmpIndex = Random.Next(results.Count);
            result = albumTracks.FindIndex(f => f.Id == results[tmpIndex].Id);
        }

        if (result == -1)
        {
            result = Random.Next(albumTracks.Count);
        }

        return result;
    }
}