namespace ER.Application.UnitTests.Builders;

public sealed class ApplicationUserBuilder
{
    private Guid _id = Guid.Parse("22222222-2222-2222-2222-222222222201");
    private Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    private string _email = "user@test.com";
    private Employee? _employee;

    public ApplicationUserBuilder()
    {
        _employee = new EmployeeBuilder().WithId(_id).WithTenantId(_tenantId).WithEmail(_email).Build();
    }

    public ApplicationUserBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        _employee = new EmployeeBuilder().WithId(_id).WithTenantId(_tenantId).WithEmail(_email).Build();
        return this;
    }

    public ApplicationUserBuilder WithEmail(string email)
    {
        _email = email;
        _employee = new EmployeeBuilder().WithId(_id).WithTenantId(_tenantId).WithEmail(_email).Build();
        return this;
    }

    public ApplicationUserBuilder WithEmployee(Employee employee)
    {
        _employee = employee;
        _id = employee.Id;
        return this;
    }

    public ApplicationUserBuilder WithoutEmployee()
    {
        _employee = null;
        return this;
    }

    public ApplicationUser Build()
    {
        var userName = IdentityUserNames.Create(_tenantId, _email);
        return new ApplicationUser(_id, _tenantId, userName, _email, true, _employee);
    }
}
