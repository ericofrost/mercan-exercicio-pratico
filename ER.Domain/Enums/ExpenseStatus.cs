namespace ER.Domain.Enums;

/// <summary>
/// Approval status of an expense submission.
/// </summary>
public enum ExpenseStatus
{
    /// <summary>
    /// Expense is awaiting manager review.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Expense was approved by a manager.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Expense was rejected by a manager.
    /// </summary>
    Rejected = 3
}
