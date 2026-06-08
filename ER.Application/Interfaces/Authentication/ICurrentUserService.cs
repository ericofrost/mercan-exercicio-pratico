using ER.Domain.Enums;

namespace ER.Application.Interfaces.Authentication;

public interface ICurrentUserService
{
    Guid? EmployeeId { get; }

    Guid? TenantId { get; }

    EmployeeRole? Role { get; }

    bool IsAuthenticated { get; }
}
