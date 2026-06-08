using ER.Domain.Models;
using ER.Domain.Shared;
using ER.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        ConfigureEntities(builder);
    }

    private static void ConfigureEntities(ModelBuilder builder)
    {
        ApplicationUserConfiguration.Configure(builder);
        EmployeeConfiguration.Configure(builder);
        TenantConfiguration.Configure(builder);
        ExpenseConfiguration.Configure(builder);
    }
}