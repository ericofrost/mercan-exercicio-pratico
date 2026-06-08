using ER.Domain.Configuration;
using ER.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ER.Infrastructure.Seeds;

/// <summary>
/// Applies database migrations and optional sample seed data during application startup.
/// </summary>
public class ApplicationDbContextInitializer(ApplicationDbContext context, IOptions<DatabaseSettings> databaseSettings, IHostEnvironment environment, SampleDataSeeder sampleDataSeeder, ILogger<ApplicationDbContextInitializer> logger)
{
    /// <summary>
    /// Application tables in dependency order (dependents first) so drops succeed without orphan FK violations.
    /// </summary>
    private static readonly string[] ApplicationTablesInDropOrder =
    [
        "Expenses",
        "AspNetUserTokens",
        "AspNetUserLogins",
        "AspNetUserClaims",
        "AspNetUserRoles",
        "AspNetRoleClaims",
        "AspNetUsers",
        "Employees",
        "AspNetRoles",
        "Tenants",
        "__EFMigrationsHistory"
    ];
    
    /// <summary>
    /// Migrates the database and optionally drops existing tables and seeds sample data according to configuration.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when initialization succeeds.</returns>
    /// <exception cref="Exception">
    /// Rethrows any exception encountered during drop, migration, or seeding after rolling back the initialization transaction.
    /// </exception>
    /// <remarks>
    /// PostgreSQL may auto-commit DDL statements; the explicit transaction primarily guarantees atomic rollback of seed operations on failure.
    /// </remarks>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (await ShouldDropTablesAsync(cancellationToken))
                await DropApplicationTablesAsync(cancellationToken);

            await ApplyMigrationsAsync(cancellationToken);

            if (databaseSettings.Value.SeedSampleData)
                await sampleDataSeeder.SeedAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed.");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Determines whether public tables should be dropped before applying pending migrations.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when drop is enabled for Development and pending migrations exist; otherwise <c>false</c>.</returns>
    private async Task<bool> ShouldDropTablesAsync(CancellationToken cancellationToken)
    {
        if (!environment.IsDevelopment())
            return false;

        if (!databaseSettings.Value.DropDatabaseBeforeMigrations)
            return false;

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);

        return pendingMigrations.Any();
    }

    

    /// <summary>
    /// Drops only tables mapped by <see cref="ApplicationDbContext"/> when they exist in the public schema.
    /// </summary>
    /// <param name="context">The database context used to execute the drop statements.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the drop pass finishes.</returns>
    private static async Task DropApplicationTablesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        foreach (var tableName in ApplicationTablesInDropOrder)
        {
            await DropTableIfExistsAsync(context, tableName, cancellationToken);
        }
    }

    /// <summary>
    /// Drops a single table when it exists. Safe to call repeatedly.
    /// </summary>
    /// <param name="context">The database context used to execute the drop statement.</param>
    /// <param name="tableName">The whitelisted table name to drop.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the existence check and optional drop finish.</returns>
    private static async Task DropTableIfExistsAsync(ApplicationDbContext context, string tableName, CancellationToken cancellationToken)
    {
        var dropTableSql = $"""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.tables
                    WHERE table_schema = 'public'
                      AND table_name = '{tableName}'
                ) THEN
                    EXECUTE format('DROP TABLE public.%I CASCADE', '{tableName}');
                END IF;
            END $$;
            """;

        await context.Database.ExecuteSqlRawAsync(dropTableSql, cancellationToken);
    }

    /// <summary>
    /// Drops application tables using the current database context.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when application tables have been dropped.</returns>
    private async Task DropApplicationTablesAsync(CancellationToken cancellationToken) =>
        await DropApplicationTablesAsync(context, cancellationToken);

    /// <summary>
    /// Applies all pending Entity Framework Core migrations.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when migrations have been applied.</returns>
    private async Task ApplyMigrationsAsync(CancellationToken cancellationToken) =>
        await context.Database.MigrateAsync(cancellationToken);
}
