namespace ER.Application.UnitTests.Services.Authentication;

public class JwtTokenGeneratorServiceTests
{
    private readonly JwtTokenGeneratorServiceFixture _fixture = new();

    [Fact]
    public async Task GenerateTokenAsync_ValidConfig_ReturnsNonEmptyJwt()
    {
        _fixture.UseValidConfiguration();
        var request = new GenerateTokenRequestBuilder().Build();
        var sut = _fixture.CreateSut();

        var token = await sut.GenerateTokenAsync(request);

        token.Should().NotBeNullOrWhiteSpace();
        token!.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public async Task GenerateTokenAsync_MissingKey_Throws()
    {
        _fixture.UseMissingKeyConfiguration();
        var request = new GenerateTokenRequestBuilder().Build();
        var sut = _fixture.CreateSut();

        Func<Task> act = async () => _ = await sut.GenerateTokenAsync(request);

        await act.Should().ThrowAsync<Exception>();
    }
}
