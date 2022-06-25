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

namespace amp.EtoForms.Settings.Interfaces;

/// <summary>
/// A class for biased randomization setting properties.
/// Implements the <see cref="amp.EtoForms.Settings.Interfaces.IBiasedRandomSettings" />
/// </summary>
/// <seealso cref="amp.EtoForms.Settings.Interfaces.IBiasedRandomSettings" />
internal abstract class BiasedRandomSettingsBase : IBiasedRandomSettings
{
    internal static void ApplyFromTo(IBiasedRandomSettings settingsFrom, IBiasedRandomSettings settingsTo)
    {
        settingsTo.BiasedRandom = settingsFrom.BiasedRandom;
        settingsTo.Tolerance = settingsFrom.Tolerance;
        settingsTo.BiasedRating = settingsFrom.BiasedRating;
        settingsTo.BiasedPlayedCount = settingsFrom.BiasedPlayedCount;
        settingsTo.BiasedRatingEnabled = settingsFrom.BiasedRatingEnabled;
        settingsTo.BiasedPlayedCountEnabled = settingsFrom.BiasedPlayedCountEnabled;
        settingsTo.BiasedRandomizedCount = settingsFrom.BiasedRandomizedCount;
        settingsTo.BiasedPlayedCountEnabled = settingsFrom.BiasedPlayedCountEnabled;
        settingsTo.BiasedSkippedCount = settingsFrom.BiasedSkippedCount;
        settingsTo.BiasedSkippedCountEnabled = settingsFrom.BiasedSkippedCountEnabled;
    }

    /// <inheritdoc cref="IBiasedRandomSettings.ApplyFrom"/>
    public virtual void ApplyFrom(IBiasedRandomSettings settings)
    {
        ApplyFromTo(settings, this);
    }

    /// <inheritdoc cref="IBiasedRandomSettings.ApplyTo"/>
    public virtual void ApplyTo(IBiasedRandomSettings settings)
    {
        settings.ApplyFrom(this);
    }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedRandom"/>
    public virtual bool BiasedRandom { get; set; }

    /// <inheritdoc cref="IBiasedRandomSettings.Tolerance"/>
    public virtual double Tolerance { get; set; }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedRating"/>
    public virtual double BiasedRating { get; set; }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedPlayedCount"/>
    public virtual double BiasedPlayedCount { get; set; }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedRatingEnabled"/>
    public virtual bool BiasedRatingEnabled
    {
        get => BiasedRating >= 0;

        set
        {
            if (!value)
            {
                BiasedRating = -1;
            }
        }
    }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedPlayedCountEnabled"/>
    public virtual bool BiasedPlayedCountEnabled
    {
        get => BiasedPlayedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedPlayedCount = -1;
            }
        }
    }

    /// <summary>
    /// Gets the random number generator.
    /// </summary>
    /// <value>The random number generator.</value>
    internal static Random Random { get; } = new();

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedRandomizedCount"/>
    public virtual double BiasedRandomizedCount { get; set; } = -1;

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedRandomizedCountEnabled"/>
    public virtual bool BiasedRandomizedCountEnabled
    {
        get => BiasedRandomizedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedRandomizedCount = -1;
            }
        }
    }

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedSkippedCount"/>
    public virtual double BiasedSkippedCount { get; set; } = -1;

    /// <inheritdoc cref="IBiasedRandomSettings.BiasedSkippedCountEnabled"/>
    public virtual bool BiasedSkippedCountEnabled
    {
        get => BiasedSkippedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedSkippedCount = -1;
            }
        }
    }
}