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

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EtoForms.Controls.Custom.SvgColorization;

/// <summary>
/// A class to change colors of SVG elements from SVG data.
/// </summary>
public class SvgColorize
{
    /// <summary>
    /// Gets the SVG document.
    /// </summary>
    /// <value>The SVG document.</value>
    public XDocument SvgDocument { get; private init; }

    /// <summary>
    /// Prevents a default instance of the <see cref="SvgColorize"/> class from being created.
    /// </summary>
    private SvgColorize()
    {
        SvgDocument = new XDocument();
    }

    /// <summary>
    /// Loads the SVG document from a file.
    /// </summary>
    /// <param name="fileName">Name of the file to load the document from.</param>
    /// <returns>A new instance to the <see cref="SvgDocument"/> class.</returns>
    public static SvgColorize LoadFromFile(string fileName)
    {
        return new SvgColorize
        {
            SvgDocument = XDocument.Load(fileName),
        };
    }

    /// <summary>
    /// Loads the SVG document from a stream.
    /// </summary>
    /// <param name="stream">The stream to load the document from.</param>
    /// <returns>A new instance to the <see cref="SvgDocument"/> class.</returns>
    public static SvgColorize LoadFromStream(Stream stream)
    {
        var xDoc = XDocument.Load(stream);

        return new SvgColorize
        {
            SvgDocument = xDoc,
        };
    }

    /// <summary>
    /// Loads the SVG document from a specified byte array.
    /// </summary>
    /// <param name="bytes">The bytes containing the SVG data.</param>
    /// <returns>A new instance to the <see cref="SvgDocument"/> class.</returns>
    public static SvgColorize FromBytes(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        return LoadFromStream(memoryStream);
    }

    /// <summary>
    /// Loads the SVG document from a specified data string.
    /// </summary>
    /// <param name="svgData">The SVG data.</param>
    /// <returns>A new instance to the <see cref="SvgDocument"/> class.</returns>
    public static SvgColorize FromString(string svgData)
    {
        return new SvgColorize
        {
            SvgDocument = XDocument.Parse(svgData),
        };
    }

    /// <summary>
    /// Gets the element name from a specified <see cref="SvgElement"/> enumeration value.
    /// </summary>
    /// <param name="element">The element type.</param>
    /// <returns>A string representing the specified element type.</returns>
    private static string NodeName(SvgElement element)
    {
        var resultString = element.ToString();
        return resultString[0].ToString().ToLowerInvariant() + resultString[1..];
    }

    /// <summary>
    /// A regex to match the fill value of the style attribute.
    /// </summary>
    private static readonly Regex FillRegex = new("fill:.*?;", RegexOptions.Compiled);

    /// <summary>
    /// A regex to match the stroke value of the style attribute.
    /// </summary>
    private static readonly Regex StrokeRegex = new("stroke:.*?;", RegexOptions.Compiled);

    /// <summary>
    /// Colorizes the elements of the SVG document specified by the <see cref="SvgElement"/> element type.
    /// </summary>
    /// <param name="element">The element type which to colorize.</param>
    /// <param name="stroke">The stroke color. A <c>null</c> value will remove the definition.</param>
    /// <returns>An instance of this <see cref="SvgColorize"/> class.</returns>
    public SvgColorize ColorizeElementsStroke(SvgElement element, SvgColor? stroke)
    {
        if (SvgDocument.Root != null)
        {
            ProcessNodes(SvgDocument.Root, element, StrokeRegex, stroke, false);
        }

        return this;
    }

    /// <summary>
    /// Processes the SVG nodes of the specified XML element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="elementType">Type of the element.</param>
    /// <param name="regex">The regex to use for replacing a value.</param>
    /// <param name="color">The color for the value color.</param>
    /// <param name="fill">if set to <c>true</c> change the 'fill' property of the SVG, otherwise 'stroke'.</param>
    private void ProcessNodes(XElement element, SvgElement elementType, Regex regex, SvgColor? color, bool fill)
    {
        if (element.Name.LocalName == NodeName(elementType) || elementType == SvgElement.All)
        {
            var value = element.Attribute("style")?.Value;
            if (value != null)
            {
                value = regex.Replace(value, color == null ? string.Empty : $"{(fill ? "fill:" : "stroke:")}" + color + ";");
                element.SetAttributeValue("style", value);
            }

            value = element.Attribute(fill ? "fill" : "stroke")?.Value;
            if (value != null)
            {
                if (value != "none")
                {
                    value = color == null ? "none" : $"{color}";
                    element.SetAttributeValue(fill ? "fill" : "stroke", value);
                }
            }
        }

        foreach (var xElement in element.Elements())
        {
            ProcessNodes(xElement, elementType, regex, color, fill);
        }
    }

    /// <summary>
    /// Clones this instance.
    /// </summary>
    /// <returns>A new instance of the <see cref="SvgColorize"/> class.</returns>
    public SvgColorize Clone()
    {
        return new SvgColorize
        {
            SvgDocument = XDocument.Parse(SvgDocument.ToString()),
        };
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T:byte[]"/> to <see cref="SvgColorize"/>.
    /// </summary>
    /// <param name="svgData">The SVG data bytes.</param>
    /// <returns>An instance to a <see cref="SvgColorize"/> class.</returns>
    public static implicit operator SvgColorize(byte[] svgData)
    {
        return FromBytes(svgData);
    }

    /// <summary>
    /// Colorizes the elements of the SVG document specified by the <see cref="SvgElement"/> element type.
    /// </summary>
    /// <param name="element">The element type which to colorize.</param>
    /// <param name="fill">The fill color. A <c>null</c> value will remove the definition.</param>
    /// <returns>An instance of this <see cref="SvgColorize"/> class.</returns>
    public SvgColorize ColorizeElementsFill(SvgElement element, SvgColor? fill)
    {
        if (SvgDocument.Root != null)
        {
            ProcessNodes(SvgDocument.Root, element, FillRegex, fill, true);
        }

        return this;
    }

    /// <summary>
    /// Saves the SVG data into a <see cref="MemoryStream"/>.
    /// </summary>
    /// <returns>An instance to a <see cref="MemoryStream"/> class.</returns>
    public MemoryStream ToMemoryStream()
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ToDataString()));
        return memoryStream;
    }

    /// <summary>
    /// Saves the SVG data into a byte array.
    /// </summary>
    /// <returns>A byte array containing the SVG data.</returns>
    public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream();
        SvgDocument.Save(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Saves the SVG data into a string.
    /// </summary>
    /// <returns>A string containing the SVG data.</returns>
    public string ToDataString()
    {
        return SvgDocument.ToString();
    }

    /// <summary>
    /// Removes the fill color definition from the specified element types.
    /// </summary>
    /// <param name="element">The element type.</param>
    /// <returns>An instance of this <see cref="SvgColorize"/> class.</returns>
    public SvgColorize RemoveFill(SvgElement element)
    {
        return ColorizeElementsFill(element, null);
    }

    /// <summary>
    /// Removes the stroke color definition from the specified element types.
    /// </summary>
    /// <param name="element">The element type.</param>
    /// <returns>An instance of this <see cref="SvgColorize"/> class.</returns>
    public SvgColorize RemoveStroke(SvgElement element)
    {
        return ColorizeElementsStroke(element, null);
    }
}