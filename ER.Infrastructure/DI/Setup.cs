namespace ER.Infrastructure.DI;

/// <summary>
/// Registers infrastructure services, persistence, identity, authentication, and database initialization components.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Adds infrastructure services required by the ExpenseReports API.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required JWT configuration such as <c>Jwt:Key</c> is missing.
    /// </exception>
    public static void ConfigureInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<SampleDataSeeder>();
        services.AddScoped<ApplicationDbContextInitializer>();

        AddAuthenticationConfiguration(services, configuration);

        AddIdentityConfiguration(services);

        ConfigureInfrastructureServices(services);
    }

    /// <summary>
    /// Registers infrastructure-level application service implementations.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    private static void ConfigureInfrastructureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ITenantRepository, TenantRepository>();
    }

    /// <summary>
    /// Configures ASP.NET Core Identity password, lockout, and store options.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
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

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = IdentityUserNames.AllowedUserNameCharacters;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    /// <summary>
    /// Configures JWT bearer authentication and authorization policies.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <c>Jwt:Key</c> is not configured.
    /// </exception>
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
