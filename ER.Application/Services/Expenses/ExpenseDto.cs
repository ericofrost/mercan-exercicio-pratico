namespace ER.Application.Services.Expenses;

public record ExpenseDto(Guid Id, Guid TenantId, Guid EmployeeId, decimal Amount, DateOnly ExpenseDate, DateTime SubmittedAt, string EmployeeName, Currency Currency, ExpenseCategory Category, string? Description = null, ExpenseStatus Status = ExpenseStatus.Pending)
{
    public Expense ToModel()
    {
        ArgumentNullException.ThrowIfNull(TenantId);
        ArgumentNullException.ThrowIfNull(EmployeeId);

        return Expense.Create(new ExpenseSpecification(TenantId, EmployeeId, Amount, ExpenseDate, SubmittedAt, Description, Currency, Category, Status));
    }

    public static ExpenseDto FromModel(Expense model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        return new ExpenseDto(model.Id, model.TenantId, model.EmployeeId, model.Amount, model.ExpenseDate, model.SubmittedAt, model.Employee.Name, model.Currency, model.Category, model.Description, model.Status);
    }
}