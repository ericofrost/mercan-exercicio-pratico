using ER.Domain.Enums;

namespace ER.Application.Authentication;

/// <summary>
/// Request payload for registering a new employee and linked identity user within a tenant.
/// </summary>
/// <param name="TenantId">The tenant under which the employee will be created.</param>
/// <param name="Name">The employee display name.</param>
/// <param name="Email">The employee email address.</param>
/// <param name="Password">The initial password for the identity user.</param>
/// <param name="Role">The employee role within the tenant.</param>
public record RegisterEmployeeRequest(Guid TenantId, string Name, string Email, string Password, EmployeeRole Role);
