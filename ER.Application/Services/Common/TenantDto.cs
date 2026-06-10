namespace ER.Application.Services.Common;

public record TenantDto(Guid Id, string Name, decimal MonthlyExpenseLimit, bool IsActive = true)
{
    public static TenantDto FromModel(Tenant model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        return new TenantDto(model.Id, model.Name, model.MonthlyExpenseLimit, model.IsActive);
    }
}