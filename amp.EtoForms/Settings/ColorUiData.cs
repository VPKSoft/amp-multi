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

namespace amp.EtoForms.Settings;

/// <summary>
/// A temporary data class for saving the color property data with an UI control.
/// </summary>
internal class ColorUiData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorUiData"/> class.
    /// </summary>
    /// <param name="colorPropertyName">Name of the color property.</param>
    /// <param name="colorDescription">The color description.</param>
    /// <param name="arrayIndex">Index in the array.</param>
    /// <param name="noColorSynchronization">A value indicating whether to disable color synchronization for this property.</param>
    public ColorUiData(string colorPropertyName, string colorDescription, int arrayIndex, bool noColorSynchronization)
    {
        ColorPropertyName = colorPropertyName;
        ColorDescription = colorDescription;
        ArrayIndex = arrayIndex;
        NoColorSynchronization = noColorSynchronization;
    }

    /// <summary>
    /// Gets or sets the name of the color property.
    /// </summary>
    /// <value>The name of the color property.</value>
    internal string ColorPropertyName { get; }

    /// <summary>
    /// Gets or sets the color description.
    /// </summary>
    /// <value>The color description.</value>
    internal string ColorDescription { get; }

    /// <summary>
    /// Gets or sets the index in the array in case the color property is an array of colors.
    /// </summary>
    /// <value>The index in the array.</value>
    internal int ArrayIndex { get; }

    /// <summary>
    /// Gets or sets a value indicating whether disable synchronization for this color property.
    /// </summary>
    /// <value><c>true</c> if to disable synchronization; otherwise, <c>false</c>.</value>
    internal bool NoColorSynchronization { get; }
}