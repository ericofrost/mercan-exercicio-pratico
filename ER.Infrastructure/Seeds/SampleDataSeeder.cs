namespace ER.Infrastructure.Seeds;

/// <summary>
/// Seeds sample tenants, employees, and identity users for local development and integration testing.
/// </summary>
public class SampleDataSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<SampleDataSeeder> logger)
{
    /// <summary>
    /// Seeds the database with sample data when it is not already present.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when seeding finishes or is skipped.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when ASP.NET Identity fails to create one of the sample users.
    /// </exception>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<SampleDataSeeder>();

        if (await IsSampleDataPresentAsync(cancellationToken))
        {
            InfrastructureLogs.OperationCompleted(logger, ctx, detail: "Sample data already present. Skipping seed.");
            return;
        }

        await SeedTenantsAsync(cancellationToken);
        await SeedEmployeesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);

        InfrastructureLogs.OperationCompleted(logger, ctx, detail: "Sample data seeded successfully.");
    }

    /// <summary>
    /// Determines whether the canonical sample tenant already exists in the database.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when sample data is already present; otherwise <c>false</c>.</returns>
    private async Task<bool> IsSampleDataPresentAsync(CancellationToken cancellationToken)
    {
        var firstTenantId = SampleTenantData.Acme.Id;

        return await context.Tenants.AnyAsync(t => t.Id == firstTenantId, cancellationToken);
    }

    /// <summary>
    /// Inserts the predefined sample tenants.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when tenants have been saved.</returns>
    private async Task SeedTenantsAsync(CancellationToken cancellationToken)
    {
        await context.Tenants.AddRangeAsync(SampleTenantData.All, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Inserts the predefined sample employees.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when employees have been saved.</returns>
    private async Task SeedEmployeesAsync(CancellationToken cancellationToken)
    {
        await context.Employees.AddRangeAsync(SampleEmployeeData.All, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Creates identity users for each sample employee using the shared default password.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when all sample users have been created.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when ASP.NET Identity fails to create a sample user.
    /// </exception>
    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        foreach (var employeeSeed in SampleEmployeeData.All)
        {
            var user = ApplicationUser.Create(new ApplicationUserSpecification(employeeSeed.TenantId, employeeSeed.Id, employeeSeed.Email, true));

            var createResult = await userManager.CreateAsync(user, SampleUserCredentials.DefaultPassword);

            if (createResult.Succeeded) continue;

            var errorCodes = string.Join(", ", createResult.Errors.Select(error => error.Code));
            var ctx = new LogContext(nameof(SampleDataSeeder), nameof(SeedUsersAsync));
            InfrastructureLogs.OperationRejected(logger, ctx, employeeSeed.TenantId, "Failed to seed identity user", errorCodes: errorCodes);

            throw new InvalidOperationException($"Failed to seed user for employee '{employeeSeed.Id}': {errorCodes}");
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
