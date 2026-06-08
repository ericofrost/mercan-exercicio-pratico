namespace ER.Application.UnitTests.Builders;

public sealed class GenerateTokenRequestBuilder
{
    private Guid _employeeId = Guid.Parse("22222222-2222-2222-2222-222222222201");
    private Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    private string _email = "user@test.com";
    private EmployeeRole _role = EmployeeRole.Employee;

    public GenerateTokenRequestBuilder WithEmployeeId(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public GenerateTokenRequestBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public GenerateTokenRequest Build() => new(_employeeId, _tenantId, _email, _role);
}
