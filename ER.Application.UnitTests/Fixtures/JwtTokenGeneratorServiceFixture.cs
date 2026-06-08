namespace ER.Application.UnitTests.Fixtures;

public sealed class JwtTokenGeneratorServiceFixture
{
    public IConfiguration Configuration { get; private set; } = ConfigurationMockConfigurator.CreateValidJwtConfiguration();

    public void UseValidConfiguration() => Configuration = ConfigurationMockConfigurator.CreateValidJwtConfiguration();

    public void UseMissingKeyConfiguration() => Configuration = ConfigurationMockConfigurator.CreateMissingKeyConfiguration();

    public JwtTokenGeneratorService CreateSut() => new(Configuration, NullLogger<JwtTokenGeneratorService>.Instance);
}
