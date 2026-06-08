namespace ER.Domain.Specifications;

public sealed record ApplicationUserSpecification(Guid TenantId, Guid EmployeeId, string Email, bool EmailConfirmed = true);
