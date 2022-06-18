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

namespace amp.EtoForms.Enumerations;
/// <summary>
/// An enumeration describing a part of a song naming formula.
/// </summary>
public enum FormulaType
{
    /// <summary>
    /// There is no formula.
    /// </summary>
    None,

    /// <summary>
    /// The artist part.
    /// </summary>
    Artist,

    /// <summary>
    /// The album part.
    /// </summary>
    Album,

    /// <summary>
    /// The track number part.
    /// </summary>
    TrackNo,

    /// <summary>
    /// The title part.
    /// </summary>
    Title,

    /// <summary>
    /// The queue index part.
    /// </summary>
    QueueIndex,

    /// <summary>
    /// The alternate queue index part.
    /// </summary>
    AlternateQueueIndex,

    /// <summary>
    /// The override (renamed) name part.
    /// </summary>
    Renamed
}