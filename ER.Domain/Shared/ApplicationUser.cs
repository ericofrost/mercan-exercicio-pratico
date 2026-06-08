namespace ER.Domain.Shared;

/// <summary>
/// ASP.NET Core Identity user linked one-to-one to an <see cref="Models.Employee"/> via a shared primary key.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Tenant this user belongs to. Used for login resolution and JWT claims.
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// Navigation to the linked employee record.
    /// </summary>
    public required Employee? Employee { get; init; }

    /// <summary>
    /// Initializes a new instance for Entity Framework Core and ASP.NET Identity materialization.
    /// </summary>
    public ApplicationUser()
    {
    }

    /// <summary>
    /// Initializes a new application user with normalized Identity fields.
    /// </summary>
    /// <param name="id">The unique identifier shared with the linked <see cref="Models.Employee"/>.</param>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="userName">The tenant-scoped username stored by Identity.</param>
    /// <param name="email">The user email address.</param>
    /// <param name="emailConfirmed">Whether the email address has been confirmed. Defaults to <c>true</c>.</param>
    /// <param name="employee">Optional navigation to the linked employee record.</param>
    [SetsRequiredMembers]
    public ApplicationUser(Guid id, Guid tenantId, string userName, string email, bool emailConfirmed = true, Employee? employee = null)
    {
        Id = id;
        TenantId = tenantId;
        UserName = userName;
        Email = email;
        NormalizedUserName = userName.ToUpperInvariant();
        NormalizedEmail = IdentityUserNames.NormalizeEmail(email);
        EmailConfirmed = emailConfirmed;
        Employee = employee;
    }

    /// <summary>
    /// Creates an application user from the supplied specification using the tenant-scoped username pattern.
    /// </summary>
    /// <param name="specification">The input values used to construct the user.</param>
    /// <returns>A new <see cref="ApplicationUser"/> instance ready for password assignment by Identity.</returns>
    public static ApplicationUser Create(ApplicationUserSpecification specification)
    {
        var userName = IdentityUserNames.Create(specification.TenantId, specification.Email);

        return new ApplicationUser(specification.EmployeeId, specification.TenantId, userName, specification.Email, specification.EmailConfirmed);
    }
}
