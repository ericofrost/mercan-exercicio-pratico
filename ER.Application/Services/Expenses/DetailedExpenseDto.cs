namespace ER.Application.Services.Expenses;

public record DetailedExpenseDto(Guid Id, Guid TenantId, Guid EmployeeId, decimal Amount, DateOnly ExpenseDate, DateTime SubmittedAt, TenantDto? Tenant, EmployeeDto? Employee, Currency Currency = Currency.Eur, ExpenseCategory Category = ExpenseCategory.Other, ExpenseStatus Status = ExpenseStatus.Pending, string? Description = null, DateTime? DecidedAt = null, Guid? DecidedByEmployeeId = null, string? RejectionReason = null, EmployeeDto? DecidedBy = null)
{
    public static DetailedExpenseDto FromModel(Expense model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        return new DetailedExpenseDto(model.Id, model.TenantId, model.EmployeeId, model.Amount, model.ExpenseDate, model.SubmittedAt, TenantDto.FromModel(model.Tenant!), EmployeeDto.FromModel(model.Employee), model.Currency, model.Category, model.Status, model.Description, model.DecidedAt, model.DecidedByEmployeeId, model.RejectionReason, EmployeeDto.FromModel(model.DecidedBy));
    }
}   