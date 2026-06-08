namespace ER.Application.UnitTests.Mocks;

public static class ConfigurationMockConfigurator
{
    public static IConfiguration CreateValidJwtConfiguration()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "714f7776-4ed5-4989-bf3e-bcd4b6d9ff4f",
            ["Jwt:Issuer"] = "ExpenseReports.Api",
            ["Jwt:Audience"] = "ExpenseReports.Client",
            ["Jwt:ExpiryMinutes"] = "30"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }

    public static IConfiguration CreateMissingKeyConfiguration()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = "ExpenseReports.Api",
            ["Jwt:Audience"] = "ExpenseReports.Client",
            ["Jwt:ExpiryMinutes"] = "30"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }
}
