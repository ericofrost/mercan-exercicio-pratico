using ER.Infrastructure.Context;
using ER.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ER.IntegrationTests.Helpers;

public static class TestDatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
        await scope.ServiceProvider.GetRequiredService<SampleDataSeeder>().SeedAsync(cancellationToken);
    }
}
