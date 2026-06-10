namespace ER.WebApi.Controllers.Common;

public record Tenant(Guid Id, string Name, decimal MonthlyExpenseLimit, bool IsActive = true);