using ER.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

public static class TenantConfiguration
{
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Tenant>(e =>
        {
            e.Property(t => t.Name).HasMaxLength(200).IsRequired();
            e.Property(t => t.MonthlyExpenseLimit).HasPrecision(18, 2);
        });
    }
}