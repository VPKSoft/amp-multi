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

namespace amp.Playback.Interfaces;

/// <summary>
/// Common properties used by the biased randomization to randomize the playlist.
/// </summary>
public interface IBiasedRandomSettings
{
    /// <summary>
    /// Applies the property values from the specified <see cref="IBiasedRandomSettings"/> implementation to this instance.
    /// </summary>
    /// <param name="settings">The class implementing the <see cref="IBiasedRandomSettings"/>.</param>
    void ApplyFrom(IBiasedRandomSettings settings);

    /// <summary>
    /// Applies the property values to the specified <see cref="IBiasedRandomSettings"/> implementation from this instance.
    /// </summary>
    /// <param name="settings">The class implementing the <see cref="IBiasedRandomSettings"/>.</param>
    void ApplyTo(IBiasedRandomSettings settings);

    /// <summary>
    /// Gets or sets a value indicating whether to use biased randomization with randomizing audio tracks.
    /// </summary>
    bool BiasedRandom { get; set; }

    /// <summary>
    /// Gets or sets the tolerance for biased randomization.
    /// </summary>
    double Tolerance { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased rating.
    /// </summary>        
    double BiasedRating { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased played count.
    /// </summary>
    double BiasedPlayedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased rating in randomization is enabled.
    /// </summary>
    bool BiasedRatingEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased played count in randomization is enabled.
    /// </summary>
    bool BiasedPlayedCountEnabled { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased randomized count.
    /// </summary>
    double BiasedRandomizedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased randomized count in randomization is enabled.
    /// </summary>
    bool BiasedRandomizedCountEnabled { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased skipped count.
    /// </summary>
    double BiasedSkippedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased skipped count in randomization is enabled.
    /// </summary>
    bool BiasedSkippedCountEnabled { get; set; }
}