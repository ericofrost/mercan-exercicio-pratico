namespace ER.Domain.Shared;

/// <summary>
/// Input model containing the claims source data used to generate a JWT access token.
/// </summary>
/// <param name="EmployeeId">The authenticated employee identifier placed in the <c>sub</c> claim.</param>
/// <param name="TenantId">The tenant identifier placed in the <c>tenant_id</c> claim.</param>
/// <param name="Email">The employee email placed in the JWT email claim.</param>
/// <param name="Role">The employee role placed in the role claim.</param>
public record GenerateTokenRequest(Guid EmployeeId, Guid TenantId, string Email, EmployeeRole Role);
