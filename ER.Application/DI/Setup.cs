using ER.Application.Interfaces.Services;
using ER.Application.Services.Expenses;

namespace ER.Application.DI;

/// <summary>
/// Registers application-layer services in the dependency injection container.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Adds application services, validators, and related dependencies required by the ExpenseReports API.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration.</param>
    public static void ConfigureApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEmployeeRegistrationService, EmployeeRegistrationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<IExpensesService, ExpensesService>();
        
        //Validators
        services.AddScoped<IValidator<PaginationQuery>, PaginationValidator>();
        services.AddScoped<IServiceValidator<LoginRequest,LoginResponse>, LoginValidator>();
        services.AddScoped<IServiceValidator<RegisterEmployeeRequest,RegisterEmployeeResult>, EmployeeRegistrationValidator>();
        services.AddScoped<IServiceValidator<SubmitExpenseRequestDto, bool>, SubmitExpenseValidator>();
        services.AddScoped<IServiceValidator<ChangeExpenseStatusDto, bool>, ExpenseStatusChangeValidator>();
    }
}
