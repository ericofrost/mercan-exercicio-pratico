using Microsoft.Extensions.Logging;

namespace ER.Application.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Services.Authentication.JwtTokenGeneratorService"/>.
/// </summary>
internal static partial class JwtTokenGeneratorServiceLogs
{
    [LoggerMessage(EventId = 1201, Level = LogLevel.Debug, Message = "JWT access token created for tenant {TenantId}, employee {EmployeeId}, expiry {ExpiryMinutes} minutes")]
    public static partial void TokenCreated(ILogger logger, Guid tenantId, Guid employeeId, int expiryMinutes);

    [LoggerMessage(EventId = 1202, Level = LogLevel.Error, Message = "JWT configuration is missing or invalid")]
    public static partial void JwtConfigurationInvalid(ILogger logger, Exception exception);
}
