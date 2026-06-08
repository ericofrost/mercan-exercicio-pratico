namespace ER.Domain.Shared;

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
    public required Employee? Employee { get; init; }

    public static ApplicationUser Create(ApplicationUserSpecification specification)
    {
        var userName = IdentityUserNames.Create(specification.TenantId, specification.Email);
        
        return new ApplicationUser
        {
            Id = specification.EmployeeId,
            TenantId = specification.TenantId,
            UserName = userName,
            Email = specification.Email,
            NormalizedUserName = userName.ToUpperInvariant(),
            NormalizedEmail = IdentityUserNames.NormalizeEmail(specification.Email),
            EmailConfirmed = specification.EmailConfirmed,
            Employee = null
        };
    }
}