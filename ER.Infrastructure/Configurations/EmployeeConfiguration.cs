using ER.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

public static class EmployeeConfiguration
{
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Employee>(e =>
        {
            e.Property(x => x.Email).HasMaxLength(320).IsRequired();
            e.HasIndex(x => new { x.TenantId, x.Email }).IsUnique();

            e.HasOne(x => x.Tenant)
                .WithMany(t => t.Employees)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}