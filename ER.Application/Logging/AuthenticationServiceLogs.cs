namespace ER.Application.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Services.Authentication.AuthenticationService"/>.
/// </summary>
internal static partial class AuthenticationServiceLogs
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Warning, Message = "Login failed for tenant {TenantId}")]
    public static partial void LoginFailed(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Warning, Message = "Token generation returned empty for tenant {TenantId}, employee {EmployeeId}")]
    public static partial void TokenGenerationEmpty(ILogger logger, Guid tenantId, Guid employeeId);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Login succeeded for tenant {TenantId}, employee {EmployeeId}")]
    public static partial void LoginSucceeded(ILogger logger, Guid tenantId, Guid employeeId);
}
