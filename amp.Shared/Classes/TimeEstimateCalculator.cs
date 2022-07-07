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

namespace amp.Shared.Classes;

/// <summary>
/// A class to calculate time estimates for amount / seconds.
/// </summary>
public class TimeEstimateCalculator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeEstimateCalculator"/> class.
    /// </summary>
    public TimeEstimateCalculator()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeEstimateCalculator"/> class.
    /// </summary>
    /// <param name="maxItems">The maximum amount of items to use as statistics.</param>
    public TimeEstimateCalculator(double maxItems)
    {
        MaxItems = maxItems;
    }

    /// <summary>
    /// Adds the statistic data to enable estimation calculation.
    /// </summary>
    /// <param name="start">The start time.</param>
    /// <param name="end">The end time.</param>
    /// <param name="amount">The amount of items between the start and end time.</param>
    public void AddData(DateTime start, DateTime end, double amount)
    {
        if (amount == 0)
        {
            return;
        }

        HandledItems += amount;
        countItems.Enqueue((start, end, amount));

        while (countItems.Sum(f => f.count) > MaxItems && countItems.Count > 0)
        {
            countItems.Dequeue();
        }
    }

    /// <summary>
    /// Calculates the estimate time for the operation being ready.
    /// </summary>
    /// <param name="totalCount">The current total count.</param>
    /// <returns>An estimate <see cref="DateTime"/> or <c>null</c> if the estimate couldn't be calculated.</returns>
    public DateTime? Estimate(double totalCount)
    {
        if (countItems.Count == 0)
        {
            return null;
        }

        var weightedSeconds = countItems.Sum(f => (f.end - f.start).TotalSeconds * f.count);
        var totalSeconds = countItems.Sum(f => (f.end - f.start).TotalSeconds);
        var totalAmount = countItems.Sum(f => f.count);
        if (totalAmount == 0)
        {
            return null;
        }

        var secondsPerItem = CalculateWeighted ? weightedSeconds / totalAmount : totalSeconds / totalAmount;

        return DateTime.Now.AddSeconds(secondsPerItem * (totalCount - HandledItems));
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use weighted average for the time estimate calculation.
    /// </summary>
    /// <value><c>true</c> if to use weighted average for the calculation; otherwise, <c>false</c>.</value>
    public bool CalculateWeighted { get; set; }

    /// <summary>
    /// Gets the amount of handled items.
    /// </summary>
    /// <value>The amount handled items.</value>
    public double HandledItems { get; private set; }

    /// <summary>
    /// Gets or sets the maximum amount of items to use for the calculation.
    /// </summary>
    /// <value>The maximum amount of items to use for the calculation.</value>
    public double MaxItems { get; set; } = 100;

    /// <summary>
    /// The item statistic data for the calculation.
    /// </summary>
    private readonly Queue<(DateTime start, DateTime end, double count)> countItems = new();
}