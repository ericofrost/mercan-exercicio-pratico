namespace ER.Application.Services.Common;

/// <summary>
/// Reads the authenticated employee, tenant, and role from JWT claims in the current HTTP context.
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    /// <inheritdoc />
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    /// <inheritdoc />
    public Guid? EmployeeId
    {
        get
        {
            var sub = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    /// <inheritdoc />
    public Guid? TenantId
    {
        get
        {
            var tenantId = httpContextAccessor.HttpContext?.User.FindFirstValue("tenant_id");
            
            return Guid.TryParse(tenantId, out var id) ? id : null;
        }
    }

    /// <inheritdoc />
    public EmployeeRole? Role
    {
        get
        {
            var role = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            
            return Enum.TryParse<EmployeeRole>(role, out var parsedRole) ? parsedRole : null;
        }
    }
}
