namespace ER.Domain.Specifications;

/// <summary>
/// Immutable input model used to create or describe a <see cref="Models.Tenant"/>.
/// </summary>
/// <param name="Name">The tenant display name.</param>
/// <param name="MonthlyExpenseLimit">The monthly approved expense limit.</param>
/// <param name="IsActive">Optional active flag. Defaults to <c>true</c>.</param>
/// <param name="Employees">Optional collection of employees belonging to the tenant.</param>
public sealed record TenantSpecification(string Name, decimal MonthlyExpenseLimit, bool? IsActive = true, IEnumerable<Employee>? Employees = null);
