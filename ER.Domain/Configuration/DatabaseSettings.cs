namespace ER.Domain.Configuration;

/// <summary>
/// Database initialization options bound from the <c>DatabaseSettings</c> configuration section.
/// </summary>
public sealed record DatabaseSettings
{
    /// <summary>
    /// Configuration section name used in <c>appsettings.json</c>.
    /// </summary>
    public const string SectionName = "DatabaseSettings";

    /// <summary>
    /// When <c>true</c> in Development, drops all public tables before applying pending migrations.
    /// </summary>
    public bool DropDatabaseBeforeMigrations { get; init; }

    /// <summary>
    /// When <c>true</c>, seeds the database with sample tenants, employees, and identity users.
    /// </summary>
    public bool SeedSampleData { get; init; }
}
