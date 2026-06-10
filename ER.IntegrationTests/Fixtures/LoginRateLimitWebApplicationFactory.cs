using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ER.IntegrationTests.Fixtures;

public class LoginRateLimitWebApplicationFactory : IntegrationTestWebApplicationFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["LoginRateLimit:Enabled"] = "true",
                ["LoginRateLimit:PermitLimit"] = "3",
                ["LoginRateLimit:WindowSeconds"] = "60",
                ["LoginRateLimit:SegmentsPerWindow"] = "4"
            });
        });
    }
}
