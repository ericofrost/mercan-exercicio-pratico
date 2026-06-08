namespace ER.Domain.Enums;

/// <summary>
/// Business category assigned to an expense submission.
/// </summary>
public enum ExpenseCategory
{
    /// <summary>
    /// Meal-related expense.
    /// </summary>
    Meal = 1,

    /// <summary>
    /// Transportation expense.
    /// </summary>
    Transport = 2,

    /// <summary>
    /// Lodging or accommodation expense.
    /// </summary>
    Lodging = 3,

    /// <summary>
    /// Expense that does not fit another category.
    /// </summary>
    Other = 4
}
