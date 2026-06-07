using ER.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ER.Infrastructure.Identity;

/// <summary>
/// Application user for identity.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Tenant this user belongs to. Used for login resolution and JWT claims.
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// Navigation to the Employee record.
    /// </summary>
    public required Employee Employee { get; init; }
}