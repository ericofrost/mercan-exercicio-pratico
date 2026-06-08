namespace ER.Domain.Models;

/// <summary>
/// Expense submitted by an employee for approval within a tenant.
/// </summary>
public class Expense : BaseModel
{
    /// <summary>
    /// Foreign key to the tenant this expense belongs to.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Foreign key to the employee who submitted the expense.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Expense amount. Must be greater than zero.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// ISO 4217 currency code (BRL, EUR, or USD).
    /// </summary>
    public Currency Currency { get; set; } = Currency.Eur;

    /// <summary>
    /// Category of the expense (meal, transport, lodging, or other).
    /// </summary>
    public ExpenseCategory Category { get; set; } = ExpenseCategory.Other;

    /// <summary>
    /// Description of the expense. Must be between 5 and 500 characters.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date when the expense was incurred.
    /// </summary>
    public DateOnly ExpenseDate { get; set; }

    /// <summary>
    /// Current approval status (pending, approved, or rejected).
    /// </summary>
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;

    /// <summary>
    /// UTC timestamp when the expense was submitted.
    /// </summary>
    public DateTime SubmittedAt { get; set; }

    /// <summary>
    /// UTC timestamp when the expense was approved or rejected. Null while pending.
    /// </summary>
    public DateTime? DecidedAt { get; set; }

    /// <summary>
    /// Foreign key to the manager who approved or rejected the expense. Null while pending.
    /// </summary>
    public Guid? DecidedByEmployeeId { get; set; }

    /// <summary>
    /// Reason provided when the expense is rejected. Required when <see cref="Status"/> is <see cref="ExpenseStatus.Rejected"/>.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Navigation property to the tenant (company) this expense belongs to.
    /// </summary>
    public required Tenant Tenant { get; set; }

    /// <summary>
    /// Navigation property to the employee who submitted the expense.
    /// </summary>
    public required Employee Employee { get; set; }

    /// <summary>
    /// Navigation property to the manager who approved or rejected the expense.
    /// </summary>
    public Employee? DecidedBy { get; set; }

    public static Expense Create(ExpenseSpecification specification)
    {
        return new Expense
        {
            Id = Guid.NewGuid(),
            TenantId = specification.TenantId,
            EmployeeId = specification.EmployeeId,
            Amount = specification.Amount,
            Currency = specification.Currency,
            Category = specification.Category,
            Description = specification.Description,
            ExpenseDate = specification.ExpenseDate,
            Status = specification.Status,
            SubmittedAt = specification.SubmittedAt,
            DecidedAt = specification.DecidedAt,
            DecidedByEmployeeId = specification.DecidedByEmployeeId,
            RejectionReason = specification.RejectionReason,
            Tenant = specification.Tenant!,
            Employee = specification.Employee!,
            DecidedBy = specification.DecidedBy
        };
    }
}
