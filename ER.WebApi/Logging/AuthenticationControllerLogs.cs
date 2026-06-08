using ER.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ER.WebApi.Logging;

/// <summary>
/// Source-generated log definitions for <see cref="Controllers.AuthenticationController"/>.
/// </summary>
internal static partial class AuthenticationControllerLogs
{
    [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "Login request received for tenant {TenantId}")]
    public static partial void LoginStarted(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 2002, Level = LogLevel.Warning, Message = "Login request rejected with HTTP 401 for tenant {TenantId}")]
    public static partial void LoginRejected(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 2003, Level = LogLevel.Information, Message = "Login request completed with HTTP 200 for tenant {TenantId}")]
    public static partial void LoginCompleted(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 2004, Level = LogLevel.Information, Message = "Registration request received for tenant {TenantId} with role {Role}")]
    public static partial void RegistrationStarted(ILogger logger, Guid tenantId, EmployeeRole role);

    [LoggerMessage(EventId = 2005, Level = LogLevel.Warning, Message = "Registration request rejected with HTTP 400 for tenant {TenantId}")]
    public static partial void RegistrationRejected(ILogger logger, Guid tenantId);

    [LoggerMessage(EventId = 2006, Level = LogLevel.Information, Message = "Registration request completed with HTTP 201 for tenant {TenantId}, employee {EmployeeId}, user {UserId}")]
    public static partial void RegistrationCompleted(ILogger logger, Guid tenantId, Guid employeeId, Guid userId);
}
