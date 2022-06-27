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

using amp.Playback.Interfaces;
using amp.Shared.Interfaces;
using BR = VPKSoft.RandomizationUtils.BiasedRandom;
namespace amp.Playback;

/// <summary>
/// A class to generate songs from a specified playlist.
/// Implements the <see cref="BiasedRandomSettingsBase" />
/// </summary>
/// <typeparam name="TSong">The type of the <see cref="IAlbumSong{TSong}"/> <see cref="IAlbumSong{TSong}.Song"/> member.</typeparam>
/// <typeparam name="TAlbumSong">The type of the <see cref="IAlbumSong{TSong}"/>.</typeparam>
/// <seealso cref="BiasedRandomSettingsBase" />
public class PlaybackOrder<TSong, TAlbumSong> : BiasedRandomSettingsBase where TSong : ISong where TAlbumSong : IAlbumSong<TSong>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackOrder{TSong, TAlbumSong}"/> class.
    /// </summary>
    /// <param name="settings">The settings for biased randomization.</param>
    public PlaybackOrder(IBiasedRandomSettings settings)
    {
        base.ApplyFrom(settings);
    }

    /// <summary>
    /// Gets the index for the next song to play.
    /// </summary>
    /// <param name="albumSongs">The album songs.</param>
    /// <returns>System.Int32.</returns>
    public int NextSongIndex(List<TAlbumSong>? albumSongs)
    {
        if (albumSongs == null || albumSongs.Count == 0)
        {
            return -1;
        }

        var queueIndex = albumSongs.FindIndex(f => f.QueueIndex == 1);

        if (queueIndex != -1)
        {
            // TODO::Update the queue indices
        }

        var iSongIndex = BiasedRandom ? RandomWeighted(albumSongs) : Random.Next(0, albumSongs.Count);

        return iSongIndex;
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