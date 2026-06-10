namespace ER.WebApi.Controllers.Common;

public record Employee(Guid Id, Guid TenantId, string Name, string Email, EmployeeRole Role, bool IsActive = true, Tenant? Tenant = null);