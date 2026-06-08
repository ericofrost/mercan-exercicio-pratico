namespace ER.Application.UnitTests.Mocks;

public static class TokenGeneratorMockConfigurator
{
    public static void ReturnsToken(Mock<ITokenGeneratorService> tokenGenerator, string token = "jwt.token.value")
    {
        tokenGenerator
            .Setup(t => t.GenerateTokenAsync(It.IsAny<GenerateTokenRequest>()))
            .Returns(new ValueTask<string?>(token));
    }

    public static void ReturnsNull(Mock<ITokenGeneratorService> tokenGenerator)
    {
        tokenGenerator
            .Setup(t => t.GenerateTokenAsync(It.IsAny<GenerateTokenRequest>()))
            .Returns(new ValueTask<string?>((string?)null));
    }

    public static void ReturnsEmpty(Mock<ITokenGeneratorService> tokenGenerator)
    {
        tokenGenerator
            .Setup(t => t.GenerateTokenAsync(It.IsAny<GenerateTokenRequest>()))
            .Returns(new ValueTask<string?>(string.Empty));
    }
}
