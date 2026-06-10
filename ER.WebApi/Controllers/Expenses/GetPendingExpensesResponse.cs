namespace ER.WebApi.Controllers.Expenses;

public record GetPendingExpensesResponse(IEnumerable<Expense> Expenses);
