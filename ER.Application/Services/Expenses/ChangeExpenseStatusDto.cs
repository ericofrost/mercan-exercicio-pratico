namespace ER.Application.Services.Expenses;

public record ChangeExpenseStatusDto(Guid ExpenseId, ExpenseStatus ExpenseStatus, string? RejectReason = null);
