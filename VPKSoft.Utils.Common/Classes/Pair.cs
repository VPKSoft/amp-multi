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

namespace VPKSoft.Utils.Common.Classes;

/// <summary>
/// A simple pair class.
/// </summary>
/// <typeparam name="TFirst">The type of the first member.</typeparam>
/// <typeparam name="TSecond">The type of the second member.</typeparam>
public class Pair<TFirst, TSecond>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pair{TFirst, TSecond}"/> class.
    /// </summary>
    /// <param name="first">The first member value.</param>
    /// <param name="second">The second member value.</param>
    public Pair(TFirst? first, TSecond? second)
    {
        First = first;
        Second = second;
    }

    /// <summary>
    /// Gets or sets the first member.
    /// </summary>
    /// <value>The first member.</value>
    public TFirst? First { get; set; }

    /// <summary>
    /// Gets or sets the second member.
    /// </summary>
    /// <value>The second member.</value>
    public TSecond? Second { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Second?.GetType() == typeof(string) ? Second.ToString() : $"{{ {nameof(First)} = {First}, {nameof(Second)} = {Second} }}";
    }
}