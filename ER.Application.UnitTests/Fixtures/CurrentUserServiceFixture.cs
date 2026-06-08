namespace ER.Application.UnitTests.Fixtures;

public sealed class CurrentUserServiceFixture
{
    private IHttpContextAccessor _accessor = HttpContextMockConfigurator.CreateAccessorWithoutContext();

    public void SetPrincipal(ClaimsPrincipal principal) =>
        _accessor = HttpContextMockConfigurator.CreateAccessorWithPrincipal(principal);

    public void SetNoHttpContext() =>
        _accessor = HttpContextMockConfigurator.CreateAccessorWithoutContext();

    public CurrentUserService CreateSut() => new(_accessor);
}
