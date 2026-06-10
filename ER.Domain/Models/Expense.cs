using System.ComponentModel;

namespace ER.Domain.Models;

/// <summary>
/// Represents an expense submitted by an employee and reviewed within a tenant boundary.
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
    /// ISO 4217 currency code.
    /// </summary>
    public Currency Currency { get; set; } = Currency.Eur;

    /// <summary>
    /// Category of the expense.
    /// </summary>
    public ExpenseCategory Category { get; set; } = ExpenseCategory.Other;

    /// <summary>
    /// Description of the expense. Must be between 5 and 500 characters when provided.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date when the expense was incurred.
    /// </summary>
    public DateOnly ExpenseDate { get; set; }

    /// <summary>
    /// Current approval status.
    /// </summary>
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;

    /// <summary>
    /// UTC timestamp when the expense was submitted.
    /// </summary>
    public DateTime SubmittedAt { get; set; }

    /// <summary>
    /// UTC timestamp when the expense was approved or rejected. <c>null</c> while pending.
    /// </summary>
    public DateTime? DecidedAt { get; set; }

    /// <summary>
    /// Foreign key to the manager who approved or rejected the expense. <c>null</c> while pending.
    /// </summary>
    public Guid? DecidedByEmployeeId { get; set; }

    /// <summary>
    /// Reason provided when the expense is rejected. Required when <see cref="Status"/> is <see cref="ExpenseStatus.Rejected"/>.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Navigation property to the tenant this expense belongs to.
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    /// Navigation property to the employee who submitted the expense.
    /// </summary>
    public Employee? Employee { get; set; }

    /// <summary>
    /// Navigation property to the manager who approved or rejected the expense.
    /// </summary>
    public Employee? DecidedBy { get; set; }

    /// <summary>
    /// Initializes a new instance for Entity Framework Core materialization.
    /// </summary>
    public Expense()
    {
    }

    /// <summary>
    /// Initializes a new expense with the required submission data and optional approval metadata.
    /// </summary>
    /// <param name="id">The unique expense identifier.</param>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="employeeId">The submitting employee identifier.</param>
    /// <param name="amount">The expense amount.</param>
    /// <param name="expenseDate">The date the expense was incurred.</param>
    /// <param name="submittedAt">The UTC submission timestamp.</param>
    /// <param name="tenant">The owning tenant navigation property.</param>
    /// <param name="employee">The submitting employee navigation property.</param>
    /// <param name="currency">The expense currency. Defaults to <see cref="Currency.Eur"/>.</param>
    /// <param name="category">The expense category. Defaults to <see cref="ExpenseCategory.Other"/>.</param>
    /// <param name="status">The approval status. Defaults to <see cref="ExpenseStatus.Pending"/>.</param>
    /// <param name="description">Optional expense description.</param>
    /// <param name="decidedAt">Optional UTC decision timestamp.</param>
    /// <param name="decidedByEmployeeId">Optional identifier of the deciding manager.</param>
    /// <param name="rejectionReason">Optional rejection reason.</param>
    /// <param name="decidedBy">Optional navigation to the deciding manager.</param>
    [SetsRequiredMembers]
    public Expense(Guid id, Guid tenantId, Guid employeeId, decimal amount, DateOnly expenseDate, DateTime submittedAt, Tenant? tenant, Employee? employee, Currency currency = Currency.Eur, ExpenseCategory category = ExpenseCategory.Other, ExpenseStatus status = ExpenseStatus.Pending, string? description = null, DateTime? decidedAt = null, Guid? decidedByEmployeeId = null, string? rejectionReason = null, Employee? decidedBy = null) : base(id)
    {
        TenantId = tenantId;
        EmployeeId = employeeId;
        Amount = amount;
        ExpenseDate = expenseDate;
        SubmittedAt = submittedAt;
        Tenant = tenant;
        Employee = employee;
        Currency = currency;
        Category = category;
        Status = status;
        Description = description;
        DecidedAt = decidedAt;
        DecidedByEmployeeId = decidedByEmployeeId;
        RejectionReason = rejectionReason;
        DecidedBy = decidedBy;
    }

    /// <summary>
    /// Creates a new expense from the supplied specification, generating a new identifier.
    /// </summary>
    /// <param name="specification">The input values used to construct the expense.</param>
    /// <returns>A new <see cref="Expense"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="ExpenseSpecification.Tenant"/> or <see cref="ExpenseSpecification.Employee"/> is <c>null</c>.
    /// </exception>
    public static Expense Create(ExpenseSpecification specification)
    {
        return new Expense(
            Guid.NewGuid(),
            specification.TenantId,
            specification.EmployeeId,
            specification.Amount,
            specification.ExpenseDate,
            specification.SubmittedAt,
            specification.Tenant,
            specification.Employee,
            specification.Currency,
            specification.Category,
            specification.Status,
            specification.Description,
            specification.DecidedAt,
            specification.DecidedByEmployeeId,
            specification.RejectionReason,
            specification.DecidedBy);
    }

    /// <summary>
    /// Sets the approval or rejection status
    /// </summary>
    /// <param name="status">The approval status</param>
    /// <param name="employeeId">The employee responsible for the status change</param>
    /// <param name="rejectionReason">The reason in case of a rejection</param>
    public void SetApprovalDetails(ExpenseStatus status, Guid employeeId, string? rejectionReason = null)
    {
        Status = status;
        DecidedAt = DateTime.UtcNow;
        DecidedByEmployeeId = employeeId;
        RejectionReason = rejectionReason;
    }

    public bool ValidateApprovalAmount(decimal totalExpenses)
    {
        if(Tenant is null) throw new ArgumentNullException(nameof(Tenant), Description = "This is a newly created expense, it needs to be associated with a tenant.");
        
        return Amount + totalExpenses <= Tenant.MonthlyExpenseLimit;
    }
}
