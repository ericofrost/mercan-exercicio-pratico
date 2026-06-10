namespace ER.Application.Services.Expenses;

public class DetailedExpenseResult : Result<DetailedExpenseDto>
{
    public new static DetailedExpenseResult Create()
    {
        return new DetailedExpenseResult
        {
            Success = true,
            Validation = new ValidationResult()
        };
    }
}