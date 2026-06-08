namespace ER.Domain.Models;

/// <summary>
/// Represents a tenant company in the multi-tenant expense reporting system.
/// </summary>
public class Tenant : Model
{
    /// <summary>
    /// Maximum total approved expense amount allowed per calendar month for this tenant.
    /// </summary>
    public decimal MonthlyExpenseLimit { get; set; }

    /// <summary>
    /// Employees belonging to this tenant.
    /// </summary>
    public ICollection<Employee>? Employees { get; set; }

    /// <summary>
    /// Initializes a new instance for Entity Framework Core materialization.
    /// </summary>
    public Tenant()
    {
    }

    /// <summary>
    /// Initializes a new tenant with the required business data.
    /// </summary>
    /// <param name="id">The unique tenant identifier.</param>
    /// <param name="name">The tenant display name.</param>
    /// <param name="monthlyExpenseLimit">The monthly approved expense limit.</param>
    /// <param name="isActive">Whether the tenant is active. Defaults to <c>true</c>.</param>
    /// <param name="employees">Optional collection of employees belonging to this tenant.</param>
    [SetsRequiredMembers]
    public Tenant(Guid id, string name, decimal monthlyExpenseLimit, bool isActive = true, ICollection<Employee>? employees = null) : base(id, name, isActive)
    {
        MonthlyExpenseLimit = monthlyExpenseLimit;
        Employees = employees;
    }

    /// <summary>
    /// Creates a new tenant from the supplied specification, generating a new identifier.
    /// </summary>
    /// <param name="specification">The input values used to construct the tenant.</param>
    /// <returns>A new <see cref="Tenant"/> instance.</returns>
    public static Tenant Create(TenantSpecification specification)
    {
        return new Tenant(
            Guid.NewGuid(),
            specification.Name,
            specification.MonthlyExpenseLimit,
            specification.IsActive ?? true,
            specification.Employees?.ToList());
    }
}
