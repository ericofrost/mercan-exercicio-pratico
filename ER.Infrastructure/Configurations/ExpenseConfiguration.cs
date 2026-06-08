using ER.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

/// <summary>
/// Fluent API configuration for the <see cref="Expense"/> entity.
/// </summary>
public static class ExpenseConfiguration
{
    /// <summary>
    /// Applies expense property constraints, indexes, and relationship mappings.
    /// </summary>
    /// <param name="builder">The model builder provided by Entity Framework Core.</param>
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Expense>(e =>
        {
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.RejectionReason).HasMaxLength(1000);

            e.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Employee)
                .WithMany(emp => emp.Expenses)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.DecidedBy)
                .WithMany()
                .HasForeignKey(x => x.DecidedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.TenantId, x.Status, x.SubmittedAt });
        });
    }
}
