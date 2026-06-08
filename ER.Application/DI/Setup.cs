using ER.Application.Interfaces.Authentication;
using ER.Application.Services.Authentication;
using ER.Application.Services.Common;
using ER.Application.Services.Employee;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ER.Application.DI;

/// <summary>
/// Registers application-layer services in the dependency injection container.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Adds application services required by the ExpenseReports API.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration.</param>
    public static void ConfigureApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEmployeeRegistrationService, EmployeeRegistrationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>();
    }
}
