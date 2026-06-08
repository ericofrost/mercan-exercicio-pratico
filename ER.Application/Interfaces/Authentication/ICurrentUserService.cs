namespace ER.Application.Interfaces.Authentication;

/// <summary>
/// Provides access to the authenticated user's identity context extracted from the current HTTP request JWT.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Identifier of the authenticated employee extracted from the JWT
    /// <see cref="ClaimTypes.NameIdentifier"/> or <c>sub</c> claim.
    /// </summary>
    Guid? EmployeeId { get; }

    /// <summary>
    /// Identifier of the authenticated tenant extracted from the JWT <c>tenant_id</c> claim.
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// Role of the authenticated employee extracted from the JWT role claim.
    /// </summary>
    EmployeeRole? Role { get; }

    /// <summary>
    /// Indicates whether the current HTTP request has an authenticated principal.
    /// </summary>
    bool IsAuthenticated { get; }
}
