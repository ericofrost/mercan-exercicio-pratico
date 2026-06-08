namespace ER.Application.Authentication;

/// <summary>
/// Request payload for authenticating an employee within a tenant.
/// </summary>
/// <param name="TenantId">The tenant scope used to resolve credentials.</param>
/// <param name="Email">The employee email address.</param>
/// <param name="Password">The plain-text password to validate.</param>
public record LoginRequest(Guid TenantId, string Email, string Password);
