namespace ER.Application.Services.Common;

public record EmployeeDto(Guid Id, Guid TenantId, string Name, string Email, EmployeeRole Role, bool IsActive = true, TenantDto? Tenant = null)
{
    public static EmployeeDto? FromModel(Domain.Models.Employee? model)
    {
        return model is null ? null : new EmployeeDto(model.Id, model.TenantId, model.Name, model.Email, model.Role, model.IsActive);
    }
}