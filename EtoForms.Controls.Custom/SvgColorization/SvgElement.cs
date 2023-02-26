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

namespace EtoForms.Controls.Custom.SvgColorization;

/// <summary>
/// An enumeration of SVG element types.
/// </summary>
public enum SvgElement
{
    /// <summary>
    /// The SVG element &lt;a&gt;.
    /// </summary>
    A,

    /// <summary>
    /// All the SVG element types.
    /// </summary>
    All,

    /// <summary>
    /// The SVG element &lt;animate&gt;.
    /// </summary>
    Animate,

    /// <summary>
    /// The SVG element &lt;animateMotion&gt;.
    /// </summary>
    AnimateMotion,

    /// <summary>
    /// The SVG element &lt;animateTransform&gt;.
    /// </summary>
    AnimateTransform,

    /// <summary>
    /// The SVG element &lt;circle&gt;.
    /// </summary>
    Circle,

    /// <summary>
    /// The SVG element &lt;ellipse&gt;.
    /// </summary>
    Ellipse,

    /// <summary>
    /// The SVG element &lt;filter&gt;.
    /// </summary>
    Filter,

    /// <summary>
    /// The SVG element &lt;foreignObject&gt;.
    /// </summary>
    ForeignObject,

    /// <summary>
    /// The SVG element &lt;g&gt;.
    /// </summary>
    G,

    /// <summary>
    /// The SVG element &lt;hatch&gt;.
    /// </summary>
    Hatch,

    /// <summary>
    /// The SVG element &lt;hatchpath&gt;.
    /// </summary>
    Hatchpath,

    /// <summary>
    /// The SVG element &lt;image&gt;.
    /// </summary>
    Image,

    /// <summary>
    /// The SVG element &lt;line&gt;.
    /// </summary>
    Line,

    /// <summary>
    /// The SVG element &lt;linearGradient&gt;.
    /// </summary>
    LinearGradient,

    /// <summary>
    /// The SVG element &lt;marker&gt;.
    /// </summary>
    Marker,

    /// <summary>
    /// The SVG element &lt;mask&gt;.
    /// </summary>
    Mask,

    /// <summary>
    /// The SVG element &lt;&gt;.
    /// </summary>
    Metadata,

    /// <summary>
    /// The SVG element &lt;mpath&gt;.
    /// </summary>
    Mpath,

    /// <summary>
    /// The SVG element &lt;path&gt;.
    /// </summary>
    Path,

    /// <summary>
    /// The SVG element &lt;pattern&gt;.
    /// </summary>
    Pattern,

    /// <summary>
    /// The SVG element &lt;polygon&gt;.
    /// </summary>
    Polygon,

    /// <summary>
    /// The SVG element &lt;polyline&gt;.
    /// </summary>
    Polyline,

    /// <summary>
    /// The SVG element &lt;radialGradient&gt;.
    /// </summary>
    RadialGradient,

    /// <summary>
    /// The SVG element &lt;rect&gt;.
    /// </summary>
    Rect,

    /// <summary>
    /// The SVG element &lt;script&gt;.
    /// </summary>
    Script,

    /// <summary>
    /// The SVG element &lt;set&gt;.
    /// </summary>
    Set,

    /// <summary>
    /// The SVG element &lt;stop&gt;.
    /// </summary>
    Stop,

    /// <summary>
    /// The SVG element &lt;style&gt;.
    /// </summary>
    Style,

    /// <summary>
    /// The SVG element &lt;svg&gt;.
    /// </summary>
    Svg,

    /// <summary>
    /// The SVG element &lt;switch&gt;.
    /// </summary>
    Switch,

    /// <summary>
    /// The SVG element &lt;symbol&gt;.
    /// </summary>
    Symbol,

    /// <summary>
    /// The SVG element &lt;text&gt;.
    /// </summary>
    Text,

    /// <summary>
    /// The SVG element &lt;textPath&gt;.
    /// </summary>
    TextPath,

    /// <summary>
    /// The SVG element &lt;title&gt;.
    /// </summary>
    Title,

    /// <summary>
    /// The SVG element &lt;tspan&gt;.
    /// </summary>
    Tspan,

    /// <summary>
    /// The SVG element &lt;use&gt;.
    /// </summary>
    Use,

    /// <summary>
    /// The SVG element &lt;view&gt;.
    /// </summary>
    View,
}