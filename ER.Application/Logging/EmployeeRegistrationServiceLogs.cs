using ER.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ER.Application.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Services.Employee.EmployeeRegistrationService"/>.
/// </summary>
internal static partial class EmployeeRegistrationServiceLogs
{
    [LoggerMessage(EventId = 1101, Level = LogLevel.Warning, Message = "Registration failed: tenant not found or inactive for tenant {TenantId}")]
    public static partial void TenantNotFoundOrInactive(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 1102, Level = LogLevel.Warning, Message = "Registration failed: email already registered for tenant {TenantId}")]
    public static partial void EmailAlreadyRegistered(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 1103, Level = LogLevel.Warning, Message = "Registration failed: identity user creation failed for tenant {TenantId} with error codes {ErrorCodes}")]
    public static partial void IdentityUserCreationFailed(ILogger logger, Guid tenantId, string errorCodes);

    [LoggerMessage(EventId = 1104, Level = LogLevel.Information, Message = "Registration succeeded for tenant {TenantId}, employee {EmployeeId}, user {UserId}, role {Role}")]
    public static partial void RegistrationSucceeded(ILogger logger, Guid tenantId, Guid employeeId, Guid userId, EmployeeRole role);

    [LoggerMessage(EventId = 1105, Level = LogLevel.Error, Message = "Registration failed unexpectedly for tenant {TenantId}")]
    public static partial void RegistrationFailedUnexpectedly(ILogger logger, Exception exception, Guid tenantId);
}
