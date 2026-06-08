namespace ER.Application.UnitTests.Builders;

public sealed class ClaimsPrincipalBuilder
{
    private readonly List<Claim> _claims = [];
    private bool _isAuthenticated = true;

    public ClaimsPrincipalBuilder WithEmployeeId(Guid employeeId)
    {
        _claims.Add(new Claim("sub", employeeId.ToString()));
        return this;
    }

    public ClaimsPrincipalBuilder WithNameIdentifier(Guid employeeId)
    {
        _claims.Add(new Claim(ClaimTypes.NameIdentifier, employeeId.ToString()));
        return this;
    }

    public ClaimsPrincipalBuilder WithTenantId(Guid tenantId)
    {
        _claims.Add(new Claim("tenant_id", tenantId.ToString()));
        return this;
    }

    public ClaimsPrincipalBuilder WithRole(EmployeeRole role)
    {
        _claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        return this;
    }

    public ClaimsPrincipalBuilder Unauthenticated()
    {
        _isAuthenticated = false;
        _claims.Clear();
        return this;
    }

    public ClaimsPrincipal Build()
    {
        var identity = new ClaimsIdentity(_claims, _isAuthenticated ? "TestAuth" : null);
        return new ClaimsPrincipal(identity);
    }
}
