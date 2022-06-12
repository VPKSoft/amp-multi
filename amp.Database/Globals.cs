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

namespace amp.Database;

/// <summary>
/// Global definitions for the library.
/// </summary>
public class Globals
{
    private static string? connectionString;

    /// <summary>
    /// Gets or sets the connection string for the amp# database.
    /// </summary>
    /// <value>The connection string.</value>
    public static string ConnectionString
    {
        get => connectionString ?? $"Data Source={DatabaseFile}";

        set => connectionString = value;
    }

    /// <summary>
    /// Gets or sets the database file name of the amp# SQLite database.
    /// </summary>
    /// <value>The database file name of the database.</value>
    public static string DatabaseFile { get; set; } = "amp.ef.sqlite";
}