using Microsoft.Extensions.Logging;

namespace ER.Infrastructure.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Seeds.ApplicationDbContextInitializer"/>.
/// </summary>
internal static partial class ApplicationDbContextInitializerLogs
{
    [LoggerMessage(EventId = 3001, Level = LogLevel.Warning, Message = "Dropping application tables before migrations in development")]
    public static partial void DroppingApplicationTables(ILogger logger);

    [LoggerMessage(EventId = 3002, Level = LogLevel.Information, Message = "Database migrations applied successfully")]
    public static partial void MigrationsApplied(ILogger logger);

    [LoggerMessage(EventId = 3003, Level = LogLevel.Information, Message = "Database initialization completed successfully")]
    public static partial void InitializationCompleted(ILogger logger);

    [LoggerMessage(EventId = 3004, Level = LogLevel.Error, Message = "Database initialization failed during {Operation}")]
    public static partial void InitializationFailed(ILogger logger, Exception exception, string operation);
}
