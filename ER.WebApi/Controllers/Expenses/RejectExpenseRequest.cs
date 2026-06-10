namespace ER.WebApi.Controllers.Expenses;

public record RejectExpenseRequest(
    [property: Required(AllowEmptyStrings = false)] string RejectReason);
