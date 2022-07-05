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

using System.Runtime.InteropServices;
using amp.Playback.Classes;
using amp.Playback.Interfaces;
using amp.Shared.Classes;
using amp.Shared.Interfaces;
using BR = VPKSoft.RandomizationUtils.BiasedRandom;
namespace amp.Playback;

/// <summary>
/// A class to generate albumSongs from a specified playlist.
/// Implements the <see cref="BiasedRandomSettingsBase" />
/// </summary>
/// <typeparam name="TSong">The type of the <see cref="IAlbumSong{TSong}"/> <see cref="IAlbumSong{TSong}.Song"/> member.</typeparam>
/// <typeparam name="TAlbumSong">The type of the <see cref="IAlbumSong{TSong}"/>.</typeparam>
/// <seealso cref="BiasedRandomSettingsBase" />
public class PlaybackOrder<TSong, TAlbumSong> : BiasedRandomSettingsBase where TSong : ISong where TAlbumSong : class, IAlbumSong<TSong>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackOrder{TSong, TAlbumSong}"/> class.
    /// </summary>
    /// <param name="settings">The settings for biased randomization.</param>
    /// <param name="updateQueueFunc">A callback function to update the modified queue indices into the database.</param>
    public PlaybackOrder(IBiasedRandomSettings settings, Func<Dictionary<long, int>, Task> updateQueueFunc)
    {
        base.ApplyFrom(settings);
        this.updateQueueFunc = updateQueueFunc;
    }

    /// <summary>
    /// Gets or sets a value indicating whether song index randomization is enabled.
    /// </summary>
    /// <value><c>true</c> if index randomization is enabled; otherwise, <c>false</c>.</value>
    public bool Randomizing { get; set; } = true;

    private int previousSongIndex = -1;

    private readonly Func<Dictionary<long, int>, Task> updateQueueFunc;

    /// <summary>
    /// Toggles the queue of the album song specified by the <paramref name="albumSongId"/>.
    /// </summary>
    /// <param name="albumSongs">The album songs.</param>
    /// <param name="albumSongId">The song identifier.</param>
    public async Task ToggleQueue(List<TAlbumSong>? albumSongs, long albumSongId)
    {
        if (albumSongs == null)
        {
            return;
        }

        if (albumSongs.Any(f => f.QueueIndex > 0))
        {
            var result = albumSongs.Where(f => f.QueueIndex > 0).Select(f => new IdValuePair<int> { Id = f.Id, Value = f.QueueIndex, }).ToList();

            var index = result.FindIndex(f => f.Id == albumSongId);

            if (index != -1)
            {
                var decrease = result[index].Value;
                result[index].Value = 0;

                foreach (var pair in result)
                {
                    if (pair.Value == decrease)
                    {
                        pair.Value = 0;
                    }

                    if (pair.Value > decrease)
                    {
                        pair.Value--;
                    }
                }
            }
            else
            {
                var song = albumSongs.First(f => f.Id == albumSongId);

                foreach (var pair in result)
                {
                    pair.Value++;
                }

                result.Add(new IdValuePair<int> { Id = song.Id, Value = 1, });
            }

            var toUpdate = new Dictionary<long, int>(result.Select(f => new KeyValuePair<long, int>(f.Id, f.Value)));

            await updateQueueFunc(toUpdate);
        }
        else
        {
            var song = albumSongs.First(f => f.Id == albumSongId);
            var newQueueIndex = song.QueueIndex < 1 ? 1 : 0;

            var toUpdate = new Dictionary<long, int> { { albumSongId, newQueueIndex }, };
            await updateQueueFunc(toUpdate);
        }
    }

    /// <summary>
    /// Gets the next song to play.
    /// </summary>
    /// <param name="albumSongs">The album albumSongs.</param>
    /// <returns>An instance of the <see cref="SongResult"/> class.</returns>
    public async Task<SongResult> NextSong(List<TAlbumSong>? albumSongs)
    {
        if (albumSongs == null || albumSongs.Count == 0)
        {
            return SongResult.Empty;
        }

        bool randomized = false, queued = false;

        var iSongIndex = -1;
        var minQueueIndex = albumSongs.Where(f => f.QueueIndex > 0).DefaultIfEmpty().Min(f => f?.QueueIndex) ?? 0;
        var queueIndex = albumSongs.FindIndex(f => f.QueueIndex > 0 && f.QueueIndex == minQueueIndex);

        var updatedSongs = new Dictionary<long, int>();

        if (queueIndex != -1)
        {
            iSongIndex = queueIndex;
            queued = true;
            foreach (var song in albumSongs.Where(f => f.QueueIndex > 0))
            {
                updatedSongs.Add(song.Id, song.QueueIndex - 1);
            }

            if (updatedSongs.Count > 0)
            {
                await updateQueueFunc(updatedSongs);
            }
        }

        if (iSongIndex == -1 && Randomizing)
        {
            iSongIndex = BiasedRandom ? RandomWeighted(albumSongs) : Random.Next(0, albumSongs.Count);

            if (iSongIndex != -1)
            {
                randomized = true;
            }
        }

        if (iSongIndex == -1 && albumSongs.Any())
        {
            iSongIndex = previousSongIndex + 1;
            if (iSongIndex >= albumSongs.Count)
            {
                iSongIndex = 0;
            }
        }

        previousSongIndex = iSongIndex;

        return new SongResult
        {
            SongId = iSongIndex != -1 ? albumSongs[iSongIndex].SongId : 0,
            NextSongIndex = iSongIndex,
            PlayedByRandomize = randomized ? 1 : 0,
            PlayedByUser = queued ? 1 : 0,
        };
    }

    /// <summary>
    /// Loops the current queue so that it will not be consumed in the process.
    /// The previous song is put to the bottom of the queue and the end of the queue is re-randomized.
    /// </summary>
    /// <param name="albumSongs">The songs which queue to adjust.</param>
    /// <param name="previousSong">The previously played song from the queue.</param>
    /// <param name="stackRandomPercentage">The stack random percentage.</param>
    /// <returns>A list of <typeparamref name="TAlbumSong"/> instances which queue index was adjusted or <c>null</c> if there is nothing changed.</returns>
    // TODO::Refactor when the stack queue is actually implemented.
    internal async Task StackQueue(List<TAlbumSong>? albumSongs, TAlbumSong previousSong, int stackRandomPercentage)
    {
        if (albumSongs == null || albumSongs.Count == 0)
        {
            return;
        }

        // Get the songs with a queue index value.
        var queuedSongs =
            albumSongs.Where(f => f.QueueIndex > 0).
                OrderByDescending(f => f.QueueIndex).ToList();

        var result = queuedSongs.Select(f => new IdValuePair<int> { Id = f.SongId, Value = f.QueueIndex, }).ToList();

        // Get the amount of songs in the queue to randomize to a new order.
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

        // Re-index the previous song to the last of the queue.
        var currentSongReQueued = new IdValuePair<int>
        { Id = previousSong.SongId, Value = result.DefaultIfEmpty().Max(f => f?.Value) + 1 ?? 1, };

        result.Add(currentSongReQueued);

        var toUpdate = new Dictionary<long, int>(result.Select(f => new KeyValuePair<long, int>(f.Id, f.Value)));

        // Update the queue re-ordering.
        await updateQueueFunc(toUpdate);
    }

    private static bool InRange(double value, double randomValue, double min, double max, double tolerancePercentage)
    {
        var range = (max - min) / 100 * tolerancePercentage;
        return value <= randomValue + range && value >= randomValue - range;
    }

    private int RandomWeighted(List<TAlbumSong>? albumSongs)
    {

        if (albumSongs == null || !albumSongs.Any())
        {
            return -1;
        }

        var results = new List<TAlbumSong>();

        double valueMin = albumSongs.Min(f => f.Song?.Rating) ?? 0;
        double valueMax = albumSongs.Max(f => f.Song?.Rating) ?? 1000;

        var biased = BR.RandomBiased(valueMin, valueMax, BiasedRating);

        if (BiasedRatingEnabled)
        {
            results.AddRange(albumSongs.FindAll(f =>
                InRange(f.Song?.Rating ?? 500, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumSongs.Min(f => f.Song?.PlayedByUser ?? 0);
        valueMax = albumSongs.Max(f => f.Song?.PlayedByUser ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedPlayedCount);

        if (BiasedPlayedCountEnabled)
        {
            results.AddRange(albumSongs.FindAll(f =>
                InRange(f.Song?.PlayedByUser ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumSongs.Min(f => f.Song?.PlayedByRandomize ?? 0);
        valueMax = albumSongs.Max(f => f.Song?.PlayedByRandomize ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedRandomizedCount);

        if (BiasedRandomizedCountEnabled)
        {
            results.AddRange(albumSongs.FindAll(f =>
                InRange(f.Song?.PlayedByRandomize ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        valueMin = albumSongs.Min(f => f.Song?.SkippedEarlyCount ?? 0);
        valueMax = albumSongs.Max(f => f.Song?.SkippedEarlyCount ?? 0);
        biased = BR.RandomBiased(valueMin, valueMax, BiasedSkippedCount);

        if (BiasedSkippedCountEnabled)
        {
            results.AddRange(albumSongs.FindAll(f =>
                InRange(f.Song?.SkippedEarlyCount ?? 0, biased, valueMin, valueMax, Tolerance)));
        }

        var result = -1;

        if (results.Count > 0)
        {
            var tmpIndex = Random.Next(results.Count);
            result = albumSongs.FindIndex(f => f.Id == results[tmpIndex].Id);
        }

        if (result == -1)
        {
            result = Random.Next(albumSongs.Count);
        }

        return result;
    }
}