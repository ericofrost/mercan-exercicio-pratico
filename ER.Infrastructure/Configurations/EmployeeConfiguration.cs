namespace ER.Infrastructure.Configurations;

/// <summary>
/// Fluent API configuration for the <see cref="Employee"/> entity.
/// </summary>
public static class EmployeeConfiguration
{
    /// <summary>
    /// Applies employee indexes, property constraints, and tenant relationship mapping.
    /// </summary>
    /// <param name="builder">The model builder provided by Entity Framework Core.</param>
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
