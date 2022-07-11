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

using amp.Database.Migration;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace amp.Database;

/// <summary>
/// A class to migrate changes into the amp# EF Core database.
/// </summary>
public class Migrate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Migrate"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string to the SQLite database to perform the migration into.</param>
    public Migrate(string connectionString)
    {
        this.connectionString = connectionString;
    }


    /// <summary>
    /// Performs the forward migration for the database.
    /// </summary>
    public void RunMigrateUp()
    {
        var serviceProvider = CreateServices(connectionString);

        // Put the database update into a scope to ensure
        // that all resources will be disposed.
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    /// <summary>
    /// Performs the backward migration for the database.
    /// </summary>
    /// <param name="version">The version of the migration.</param>
    public void RunMigrateDown(long version)
    {
        var serviceProvider = CreateServices(connectionString);

        // Put the database update into a scope to ensure
        // that all resources will be disposed.
        using var scope = serviceProvider.CreateScope();
        RevertDatabase(scope.ServiceProvider, version);
    }

    private readonly string connectionString;

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static IServiceProvider CreateServices(string connectionString, params string[]? tags)
    {
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add SQLite support to FluentMigrator
                .AddSQLite()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(CreateInitialDatabase).Assembly).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Add tag support ([Tags(...)])
            .Configure<RunnerOptions>(opt => opt.Tags = tags ?? Array.Empty<string>())
            // Build the service provider
            .BuildServiceProvider(false);
    }


    /// <summary>
    /// Update the database.
    /// <param name="serviceProvider">The dependency injection provider for the migrations.</param>
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }


    /// <summary>
    /// Reverts the database into a specified version.
    /// <param name="serviceProvider">The dependency injection provider for the migrations.</param>
    /// <param name="version">The version to migrate down to.</param>
    /// </summary>
    private static void RevertDatabase(IServiceProvider serviceProvider, long version)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateDown(version);
    }
}