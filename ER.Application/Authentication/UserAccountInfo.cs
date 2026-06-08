namespace ER.Application.Authentication;

/// <summary>
/// Read model containing the account data required for authentication and token issuance.
/// </summary>
/// <param name="UserId">The identity user identifier.</param>
/// <param name="EmployeeId">The linked employee identifier.</param>
/// <param name="TenantId">The owning tenant identifier.</param>
/// <param name="Email">The employee email address.</param>
/// <param name="Role">The employee role within the tenant.</param>
/// <param name="IsEmployeeActive">Whether the linked employee record is active.</param>
public record UserAccountInfo(Guid UserId, Guid EmployeeId, Guid TenantId, string Email, EmployeeRole Role, bool IsEmployeeActive);
