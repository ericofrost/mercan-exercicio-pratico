namespace ER.Domain.Specifications;

public sealed record TenantSpecification(string Name, decimal MonthlyExpenseLimit, bool? IsActive = true, IEnumerable<Employee>? Employees = null);
