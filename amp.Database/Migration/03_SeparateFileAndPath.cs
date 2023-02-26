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
using FluentMigrator;

namespace amp.Database.Migration;

/// <summary>
/// Drops the useless FileNameNoPath column from the AudioTrack database table and adds the FilePath column to the same table.
/// Implements the <see cref="Migration" />
/// </summary>
/// <seealso cref="Migration" />
[Migration(3)]
public class SeparateFileAndPath : FluentMigrator.Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Alter.Column("FileNameNoPath").OnTable(nameof(AudioTrack)).AsString().Nullable();
        Execute.Sql("UPDATE AudioTrack SET FileNameNoPath = NULL");
        // NOTE, Won't work on SQLite yet. That is why the upper alter.
        Delete.Column("FileNameNoPath").FromTable(nameof(AudioTrack));
        Create.Column(nameof(AudioTrack.FilePath)).OnTable(nameof(AudioTrack)).AsString().WithDefaultValue("");

        Create.Table(nameof(SoftwareMigrationVersion))
            .WithColumn(nameof(SoftwareMigrationVersion.Id)).AsInt64().NotNullable().PrimaryKey()
            .WithColumn(nameof(SoftwareMigrationVersion.Version)).AsInt32().NotNullable();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Alter.Table(nameof(AudioTrack)).AddColumn("FileNameNoPath").AsString().WithDefaultValue("");
        Delete.Column(nameof(AudioTrack.FilePath)).FromTable(nameof(AudioTrack));
        Delete.Table(nameof(SoftwareMigrationVersion));
    }
}