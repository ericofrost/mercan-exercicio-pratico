namespace ER.WebApi.Controllers.Expenses;

public sealed record SubmitExpenseRequest(
    Guid? Id,
    [param: Required] Guid TenantId,
    [param: Required] Guid EmployeeId,
    [param: Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive.")] decimal Amount,
    [param: NotMinimumDate(ErrorMessage = "ExpenseDate must be a valid date.")] DateOnly ExpenseDate,
    [param: NotMinimumDate(ErrorMessage = "SubmittedAt must be a valid date.")] DateTime SubmittedAt,
    [param: Required(AllowEmptyStrings = false)]
    [param: StringLength(200, MinimumLength = 1)] string EmployeeName,
    Currency Currency,
    ExpenseCategory Category,
    [param: StringLength(500, MinimumLength = 50)] string? Description = null,
    ExpenseStatus Status = ExpenseStatus.Pending)
    : Expense(Id, TenantId, EmployeeId, Amount, ExpenseDate, SubmittedAt, EmployeeName, Currency, Category, Description, Status);
