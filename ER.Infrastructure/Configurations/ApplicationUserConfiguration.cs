using ER.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

/// <summary>
/// Fluent API configuration for the <see cref="ApplicationUser"/> identity entity.
/// </summary>
public static class ApplicationUserConfiguration
{
    /// <summary>
    /// Applies identity indexes and the one-to-one shared primary key relationship with <see cref="Domain.Models.Employee"/>.
    /// </summary>
    /// <param name="builder">The model builder provided by Entity Framework Core.</param>
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(e =>
        {
            e.HasIndex(u => new { u.TenantId, u.Email });

            e.HasOne(u => u.Employee)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
