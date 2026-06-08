namespace ER.Application.UnitTests.Builders;

public sealed class EmployeeBuilder
{
    private Guid _id = Guid.Parse("22222222-2222-2222-2222-222222222201");
    private Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    private string _name = "Test Employee";
    private string _email = "user@test.com";
    private EmployeeRole _role = EmployeeRole.Employee;
    private bool _isActive = true;

    public EmployeeBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public EmployeeBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public EmployeeBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public EmployeeBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Employee Build() => new(_id, _tenantId, _name, _email, _role, _isActive);
}
