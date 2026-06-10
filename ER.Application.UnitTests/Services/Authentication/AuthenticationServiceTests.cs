namespace ER.Application.UnitTests.Services.Authentication;

public class AuthenticationServiceTests
{
    private readonly AuthenticationServiceFixture _fixture = new();

    [Fact]
    public async Task LoginAsync_ValidationFails_ReturnsWithoutCallingUserManager()
    {
        var request = new LoginRequestBuilder().Build();
        LoginValidatorMockConfigurator.ValidationFails(_fixture.Validator);
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeFalse();
        _fixture.UserManager.Verify(m => m.CheckPasswordAsync(It.IsAny<Domain.Shared.ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsServiceError()
    {
        var request = new LoginRequestBuilder().Build();
        LoginValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupUsersQuery(_fixture.UserManager, []);
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Service && e.ErrorMessage == "Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsServiceError()
    {
        var user = new ApplicationUserBuilder().Build();
        var request = new LoginRequestBuilder().WithEmail(user.Email!).Build();
        LoginValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupUsersQuery(_fixture.UserManager, [user]);
        UserManagerMockConfigurator.SetupCheckPasswordAsync(_fixture.UserManager, user, request.Password, false);
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Service && e.ErrorMessage == "Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
    {
        var user = new ApplicationUserBuilder().Build();
        var request = new LoginRequestBuilder().WithEmail(user.Email!).Build();
        LoginValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupUsersQuery(_fixture.UserManager, [user]);
        UserManagerMockConfigurator.SetupCheckPasswordAsync(_fixture.UserManager, user, request.Password, true);
        TokenGeneratorMockConfigurator.ReturnsToken(_fixture.TokenGenerator, "signed.jwt.token");
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("signed.jwt.token");
        result.Data.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        _fixture.TokenGenerator.Verify(t => t.GenerateTokenAsync(It.IsAny<Domain.Shared.GenerateTokenRequest>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_TokenGenerationEmpty_ReturnsServiceError()
    {
        var user = new ApplicationUserBuilder().Build();
        var request = new LoginRequestBuilder().WithEmail(user.Email!).Build();
        LoginValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupUsersQuery(_fixture.UserManager, [user]);
        UserManagerMockConfigurator.SetupCheckPasswordAsync(_fixture.UserManager, user, request.Password, true);
        TokenGeneratorMockConfigurator.ReturnsEmpty(_fixture.TokenGenerator);
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Service && e.ErrorMessage == "Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_UnexpectedException_SetsExceptionError()
    {
        var request = new LoginRequestBuilder().Build();
        LoginValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupUsersQueryThrows(_fixture.UserManager, new InvalidOperationException("Database unavailable."));
        var sut = _fixture.CreateSut();

        var result = await sut.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Exception && e.ErrorMessage == OperationMessages.UnexpectedError);
    }
}
