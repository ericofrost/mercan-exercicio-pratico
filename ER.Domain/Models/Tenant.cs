namespace ER.Domain.Models;

/// <summary>
/// Represents a tenant (company) in the multi-tenant system.
/// </summary>
public class Tenant : Model
{
    /// <summary>
    /// Maximum total approved expense amount allowed per calendar month for this tenant.
    /// </summary>
    public decimal MonthlyExpenseLimit { get; set; }
    
    /// <summary>
    /// Employees belonging to this tenant
    /// </summary>
    public ICollection<Employee>? Employees { get; set; }

    public static Tenant Create(TenantSpecification specification)
    {
        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = specification.Name,
            MonthlyExpenseLimit = specification.MonthlyExpenseLimit,
            Employees = specification.Employees?.ToList()
        };
    }
}
