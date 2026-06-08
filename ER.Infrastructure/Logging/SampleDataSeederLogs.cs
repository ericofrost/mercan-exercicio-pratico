namespace ER.Infrastructure.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Seeds.SampleDataSeeder"/>.
/// </summary>
internal static partial class SampleDataSeederLogs
{
    [LoggerMessage(EventId = 3101, Level = LogLevel.Information, Message = "Sample data already present. Skipping seed.")]
    public static partial void SampleDataAlreadyPresent(ILogger logger);

    [LoggerMessage(EventId = 3102, Level = LogLevel.Information, Message = "Sample data seeded successfully.")]
    public static partial void SampleDataSeeded(ILogger logger);

    [LoggerMessage(EventId = 3103, Level = LogLevel.Error, Message = "Failed to seed identity user for employee {EmployeeId} with error codes {ErrorCodes}")]
    public static partial void UserSeedFailed(ILogger logger, Guid employeeId, string errorCodes);
}
