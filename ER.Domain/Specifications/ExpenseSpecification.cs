namespace ER.Domain.Specifications;

public sealed record ExpenseSpecification(
    Guid TenantId,
    Guid EmployeeId,
    decimal Amount,
    DateOnly ExpenseDate,
    DateTime SubmittedAt,
    string? Description = null,
    Currency Currency = Currency.Eur,
    ExpenseCategory Category = ExpenseCategory.Other,
    ExpenseStatus Status = ExpenseStatus.Pending,
    DateTime? DecidedAt = null,
    Guid? DecidedByEmployeeId = null,
    string? RejectionReason = null,
    Tenant? Tenant = null,
    Employee? Employee = null,
    Employee? DecidedBy = null);
