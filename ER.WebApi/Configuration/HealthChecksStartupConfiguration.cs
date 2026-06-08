using ER.Infrastructure.Context;
using ER.WebApi.Helpers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ER.WebApi.Configuration;

/// <summary>
/// Registers health check services for the ExpenseReports API.
/// </summary>
public static class HealthChecksStartupConfiguration
{
    /// <summary>
    /// Adds health check services including process liveness and PostgreSQL readiness via EF Core.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    public static void ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck(name: "self", check: () => HealthCheckResult.Healthy("API is running"), tags: ["live"])
            .AddDbContextCheck<ApplicationDbContext>(name: "postgresql", failureStatus: HealthStatus.Unhealthy, tags: ["db", "ready"]);
    }

    /// <summary>
    /// Maps the health checks get endpoint to the web application.
    /// </summary>
    /// <param name="app">Web Application <see cref="WebApplication"/></param>
    public static void MapHealthChecksEndpoint(this WebApplication app)
    {
        app.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteResponse,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            })
            .AllowAnonymous()
            .ShortCircuit();
    }
}