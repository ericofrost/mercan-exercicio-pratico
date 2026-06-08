namespace ER.Application.UnitTests.Builders;

public sealed class RegisterEmployeeRequestBuilder
{
    private Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    private string _name = "Test Employee";
    private string _email = "employee@test.com";
    private string _password = "Pass123!";
    private EmployeeRole _role = EmployeeRole.Employee;

    public RegisterEmployeeRequestBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public RegisterEmployeeRequestBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public RegisterEmployeeRequestBuilder WithRole(EmployeeRole role)
    {
        _role = role;
        return this;
    }

    public RegisterEmployeeRequest Build() => new(_tenantId, _name, _email, _password, _role);
}
