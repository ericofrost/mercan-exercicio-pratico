namespace ER.Domain.Enums;

/// <summary>
/// Role of an employee within a tenant.
/// </summary>
public enum EmployeeRole
{
    /// <summary>
    /// Standard employee who can submit expenses.
    /// </summary>
    Employee = 1,

    /// <summary>
    /// Manager who can approve or reject expenses.
    /// </summary>
    Manager = 2
}
