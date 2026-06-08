using ER.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

/// <summary>
/// Fluent API configuration for the <see cref="Tenant"/> entity.
/// </summary>
public static class TenantConfiguration
{
    /// <summary>
    /// Applies tenant property constraints and precision rules.
    /// </summary>
    /// <param name="builder">The model builder provided by Entity Framework Core.</param>
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Tenant>(e =>
        {
            e.Property(t => t.Name).HasMaxLength(200).IsRequired();
            e.Property(t => t.MonthlyExpenseLimit).HasPrecision(18, 2);
        });
    }
}
