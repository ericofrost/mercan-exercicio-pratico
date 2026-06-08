using ER.Domain.Enums;

namespace ER.Application.Authentication;

public record UserAccountInfo(Guid UserId, Guid EmployeeId, Guid TenantId, string Email, EmployeeRole Role, bool IsEmployeeActive);
