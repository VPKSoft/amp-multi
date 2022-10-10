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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amp.Database.DataModel;

/// <summary>
/// An entity for the <see cref="FluentMigrator"/> VersionInfo database table.
/// </summary>
[Table(nameof(VersionInfo))]
public class VersionInfo
{
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    [Key]
    public long Version { get; set; }

    /// <summary>
    /// Gets or sets the date and time the migration was applied on.
    /// </summary>
    /// <value>The date and time the migration applied on.</value>
    public DateTime AppliedOn { get; set; }

    /// <summary>
    /// Gets or sets the migration description.
    /// </summary>
    /// <value>The migration description.</value>
    public string Description { get; set; } = string.Empty;
}