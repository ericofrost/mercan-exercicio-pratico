namespace ER.Infrastructure.Context;

/// <summary>
/// Entity Framework Core database context for tenants, employees, expenses, and ASP.NET Identity stores.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    /// <summary>
    /// Tenants available in the multi-tenant system.
    /// </summary>
    public DbSet<Tenant> Tenants => Set<Tenant>();

    /// <summary>
    /// Employees belonging to tenants.
    /// </summary>
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>
    /// Expenses submitted by employees.
    /// </summary>
    public DbSet<Expense> Expenses => Set<Expense>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureEntities(builder);
    }

    /// <summary>
    /// Applies fluent entity configurations for domain and identity models.
    /// </summary>
    /// <param name="builder">The model builder provided by Entity Framework Core.</param>
    private static void ConfigureEntities(ModelBuilder builder)
    {
        ApplicationUserConfiguration.Configure(builder);
        EmployeeConfiguration.Configure(builder);
        TenantConfiguration.Configure(builder);
        ExpenseConfiguration.Configure(builder);
    }
}
