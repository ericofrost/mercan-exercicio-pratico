using ER.Domain.Enums;

namespace ER.Application.Authentication;

public record RegisterEmployeeRequest(Guid TenantId, string Name, string Email, string Password, EmployeeRole Role);
