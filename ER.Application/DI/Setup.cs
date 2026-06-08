using ER.Application.Interfaces.Authentication;
using ER.Application.Services.Authentication;
using ER.Application.Services.Common;
using ER.Application.Services.Employee;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ER.Application.DI;

public static class Setup
{
    public static void ConfigureApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEmployeeRegistrationService, EmployeeRegistrationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>();
    }
}