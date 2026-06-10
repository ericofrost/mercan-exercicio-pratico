namespace ER.Application.Services.Expenses;

public class ExpenseByTenantResult : Result<List<ExpenseDto>>
{
    public new static ExpenseByTenantResult Create()
    {
        return new ExpenseByTenantResult
        {
            Data = [],
            Success = true,
            Validation = new ValidationResult()
        };
    }
}