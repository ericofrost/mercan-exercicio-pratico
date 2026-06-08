namespace ER.Domain.Models;

/// <summary>
/// Represents an employee belonging to a tenant (company).
/// </summary>
public class Employee : Model
{
    /// <summary>
    /// Foreign key to the tenant this employee belongs to.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Employee email address. Must be unique within the tenant.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Role of the employee within the tenant (<see cref="EmployeeRole.Employee"/> or <see cref="EmployeeRole.Manager"/>).
    /// </summary>
    public EmployeeRole Role { get; set; }

    /// <summary>
    /// Navigation property to the tenant (company) this employee belongs to.
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    /// Expenses submitted by this employee.
    /// </summary>
    public ICollection<Expense>? Expenses { get; set; }

    public static Employee Create(EmployeeSpecification specification)
    {
        return new Employee
        {
            Id =  Guid.NewGuid(),
            Name =  specification.Name,
            Email = specification.Email,
            Role = specification.Role,
            TenantId = specification.TenantId,
            Expenses = specification.Expenses?.ToList()
        };
    }
}
