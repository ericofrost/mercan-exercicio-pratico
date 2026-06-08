namespace ER.Application.UnitTests.Fixtures;

public sealed class AuthenticationServiceFixture
{
    public Mock<UserManager<ApplicationUser>> UserManager { get; } = UserManagerMockConfigurator.Create();

    public Mock<ITokenGeneratorService> TokenGenerator { get; } = new();

    public Mock<IServiceValidator<LoginRequest, LoginResponse>> Validator { get; } = new();

    public IOptions<Domain.Configuration.JwtSettings> JwtOptions { get; } = JwtOptionsMockConfigurator.CreateOptions();

    public AuthenticationService CreateSut() => new(
        UserManager.Object,
        TokenGenerator.Object,
        Validator.Object,
        JwtOptions,
        NullLogger<AuthenticationService>.Instance);
}
