using System.Text;
using ER.Application.Interfaces.Repositories;
using ER.Domain.Enums;
using ER.Domain.Shared;
using ER.Infrastructure.Context;
using ER.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ER.Infrastructure.DI;

public static class Setup
{
    public static void ConfigureInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        AddAuthenticationConfiguration(services, configuration);
        
        AddIdentityConfiguration(services);

        ConfigureInfrastructureServices(services);
    }

    private static void ConfigureInfrastructureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ITenantRepository, TenantRepository>();
    }

    private static void AddIdentityConfiguration(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = false; // Uniqueness is per-tenant via UserName pattern
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddAuthenticationConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        services.AddAuthorizationBuilder().AddPolicy("ManagerOnly", policy => policy.RequireRole(nameof(EmployeeRole.Manager)));
    }
}