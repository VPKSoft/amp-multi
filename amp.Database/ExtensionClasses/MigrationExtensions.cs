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

namespace amp.Database.ExtensionClasses;

/// <summary>
/// Extension methods for the <see cref="FluentMigrator"/>.
/// </summary>
public static class MigrationExtensions
{
    private const string TriggerTemplate = @"CREATE TRIGGER Trigger_{0}_RowVersion_{1}
AFTER {1} ON {0}
BEGIN
  UPDATE {0}
  SET RowVersion = (SELECT MAX(Counter) FROM Sequence)
  WHERE ROWID = NEW.ROWID;

  UPDATE Sequence SET Counter = Counter + 1;
END";

    private const string DropTriggerTemplate = @"DROP TRIGGER Trigger_{0}_RowVersion_{1}";

    /// <summary>
    /// Creates the row version simulation triggers for a specified table.
    /// </summary>
    /// <param name="migration">The migration instance.</param>
    /// <param name="tableName">Name of the table.</param>
    public static void CreateRowVersionTriggers(this FluentMigrator.Migration migration, string tableName)
    {
        migration.Execute.Sql(string.Format(TriggerTemplate, tableName, "INSERT"));
        migration.Execute.Sql(string.Format(TriggerTemplate, tableName, "UPDATE"));
    }

    /// <summary>
    /// Drops the row version simulation triggers from a specified table.
    /// </summary>
    /// <param name="migration">The migration instance.</param>
    /// <param name="tableName">Name of the table.</param>
    public static void DropRowVersionTriggers(this FluentMigrator.Migration migration, string tableName)
    {
        migration.Execute.Sql(string.Format(DropTriggerTemplate, tableName, "INSERT"));
        migration.Execute.Sql(string.Format(DropTriggerTemplate, tableName, "UPDATE"));
    }
}