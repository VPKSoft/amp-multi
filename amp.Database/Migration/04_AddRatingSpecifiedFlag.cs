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

using amp.Database.DataModel;
using amp.Shared.Interfaces;
using FluentMigrator;

namespace amp.Database.Migration;

/// <summary>
/// Adds a RatingSpecified flag to the AudioTrack database table and updates its value depending on the default rating being exactly 500.
/// Implements the <see cref="FluentMigrator.Migration" />
/// </summary>
/// <seealso cref="Migration" />
[Migration(4)]
public class AddRatingSpecifiedFlag : FluentMigrator.Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Alter.Table(nameof(AudioTrack))
            .AddColumn(nameof(IAudioTrack.RatingSpecified))
            .AsBoolean()
            .WithDefaultValue(false);

        Execute.Sql($"UPDATE {nameof(AudioTrack)} SET {nameof(IAudioTrack.RatingSpecified)} = 1 WHERE {nameof(IAudioTrack.Rating)} <> 500");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete.Column(nameof(AudioTrack.RatingSpecified)).FromTable(nameof(AudioTrack));
    }
}