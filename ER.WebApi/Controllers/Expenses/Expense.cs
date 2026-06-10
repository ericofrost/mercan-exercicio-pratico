namespace ER.WebApi.Controllers.Expenses;

public record Expense(Guid? Id, Guid TenantId, Guid EmployeeId, decimal Amount, DateOnly ExpenseDate, DateTime SubmittedAt, string EmployeeName, Currency Currency, ExpenseCategory Category, string? Description = null, ExpenseStatus Status = ExpenseStatus.Pending);