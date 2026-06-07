using ER.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Configurations;

public class ApplicationUserConfiguration
{
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