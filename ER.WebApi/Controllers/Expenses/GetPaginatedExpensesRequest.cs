namespace ER.WebApi.Controllers.Expenses;

public record GetPaginatedExpensesRequest(
    [property: Range(1, int.MaxValue)] int CurrentPage = 1,
    [property: Range(1, 100)] int RowsPerPage = 20,
    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(100, MinimumLength = 1)] string OrderBy = "Id",
    [property: Required]
    [property: RegularExpression("^(?i)(asc|desc)$", ErrorMessage = "Order must be 'asc' or 'desc'.")] string Order = "asc");
