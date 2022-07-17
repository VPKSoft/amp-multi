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

namespace amp.Shared.Extensions;

/// <summary>
/// Extension for <see cref="Collection{T}"/> class and its descendants.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Finds the index.
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="matchPredicate">The match predicate.</param>
    /// <returns>System.Int32.</returns>
    // ReSharper disable once ParameterTypeCanBeEnumerable.Global, changing the type 
    public static int FindIndex<T>(this Collection<T> collection, Predicate<T> matchPredicate)
    {
        var index = collection.Select((item, index) => new { Item = item, Index = index, }).FirstOrDefault(i => matchPredicate(i.Item))?.Index ?? -1;
        return index;
    }

    /// <summary>
    /// Updates or inserts the specified specified value to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="value">The value to either insert or update.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
    // ReSharper disable once IdentifierTypo, Upsert is a database term.
    public static void Upsert<T>(this Collection<T> collection, T value, Predicate<T> match)
    {
        var index = collection.FindIndex(match);
        if (index != -1)
        {
            collection[index] = value;
        }
        else
        {
            collection.Add(value);
        }
    }
}