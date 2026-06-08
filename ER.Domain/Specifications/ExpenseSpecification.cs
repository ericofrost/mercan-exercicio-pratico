namespace ER.Domain.Specifications;

/// <summary>
/// Immutable input model used to create or describe an <see cref="Models.Expense"/>.
/// </summary>
/// <param name="TenantId">The owning tenant identifier.</param>
/// <param name="EmployeeId">The submitting employee identifier.</param>
/// <param name="Amount">The expense amount.</param>
/// <param name="ExpenseDate">The date the expense was incurred.</param>
/// <param name="SubmittedAt">The UTC submission timestamp.</param>
/// <param name="Description">Optional expense description.</param>
/// <param name="Currency">The expense currency. Defaults to <see cref="Currency.Eur"/>.</param>
/// <param name="Category">The expense category. Defaults to <see cref="ExpenseCategory.Other"/>.</param>
/// <param name="Status">The approval status. Defaults to <see cref="ExpenseStatus.Pending"/>.</param>
/// <param name="DecidedAt">Optional UTC decision timestamp.</param>
/// <param name="DecidedByEmployeeId">Optional identifier of the deciding manager.</param>
/// <param name="RejectionReason">Optional rejection reason.</param>
/// <param name="Tenant">Optional navigation to the owning tenant.</param>
/// <param name="Employee">Optional navigation to the submitting employee.</param>
/// <param name="DecidedBy">Optional navigation to the deciding manager.</param>
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
