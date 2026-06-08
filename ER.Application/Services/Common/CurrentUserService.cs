using System.Security.Claims;
using ER.Application.Interfaces.Authentication;
using ER.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ER.Application.Services.Common;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid? EmployeeId
    {
        get
        {
            var sub = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tenantId = httpContextAccessor.HttpContext?.User.FindFirstValue("tenant_id");
            return Guid.TryParse(tenantId, out var id) ? id : null;
        }
    }

    public EmployeeRole? Role
    {
        get
        {
            var role = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<EmployeeRole>(role, out var parsedRole) ? parsedRole : null;
        }
    }
}
