namespace ER.Application.UnitTests.Mocks;

public static class JwtOptionsMockConfigurator
{
    public static IOptions<JwtSettings> CreateOptions(int expiryMinutes = 30) =>
        Options.Create(new JwtSettings
        {
            Issuer = "ExpenseReports.Api",
            Audience = "ExpenseReports.Client",
            Key = "714f7776-4ed5-4989-bf3e-bcd4b6d9ff4f",
            ExpiryMinutes = expiryMinutes
        });
}
