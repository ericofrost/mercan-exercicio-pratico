namespace ER.Application.UnitTests.Builders;

public sealed class LoginRequestBuilder
{
    private Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    private string _email = "user@test.com";
    private string _password = "Pass123!";

    public LoginRequestBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public LoginRequestBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public LoginRequestBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public LoginRequest Build() => new(_tenantId, _email, _password);
}
