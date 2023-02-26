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

using amp.Shared.Interfaces;

namespace amp.Shared.Classes;

/// <summary>
/// A class to pair entity identifiers combined with data value.
/// Implements the <see cref="amp.Shared.Interfaces.IEntityBase{T}" />
/// </summary>
/// <typeparam name="T">The type of the <see cref="IdValuePair{T}.Value"/>.</typeparam>
/// <seealso cref="amp.Shared.Interfaces.IEntityBase{T}" />
public class IdValuePair<T> : IEntityBase<long>
{
    /// <inheritdoc cref="IEntityBase{T}"/>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the value part of the pair.
    /// </summary>
    /// <value>The value part of the pair.</value>
    public T? Value { get; set; }
}