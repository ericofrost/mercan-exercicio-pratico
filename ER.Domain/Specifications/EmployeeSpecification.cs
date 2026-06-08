namespace ER.Domain.Specifications;

public sealed record EmployeeSpecification(Guid TenantId, string Name, string Email, EmployeeRole Role, bool? IsActive = false, Tenant? Tenant = null, IEnumerable<Expense>? Expenses = null);