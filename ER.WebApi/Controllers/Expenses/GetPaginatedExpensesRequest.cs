namespace ER.WebApi.Controllers.Expenses;

public record GetPaginatedExpensesRequest(int CurrentPage = 1, int RowsPerPage = 20, string OrderBy = "Id", string Order = "asc");