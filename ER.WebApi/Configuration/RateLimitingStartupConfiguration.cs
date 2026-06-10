namespace ER.WebApi.Configuration;

/// <summary>
/// Registers login endpoint rate limiting using ASP.NET Core built-in middleware.
/// </summary>
public static class RateLimitingStartupConfiguration
{
    /// <summary>
    /// Adds a partitioned sliding-window rate limiter for the login policy.
    /// When disabled in configuration, the policy resolves to a no-op limiter.
    /// </summary>
    public static IServiceCollection ConfigureLoginRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoginRateLimitSettings>(configuration.GetSection(LoginRateLimitSettings.SectionName));

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            
            options.OnRejected = async (context, cancellationToken) =>
            {
                var httpContext = context.HttpContext;
                
                var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("LoginRateLimit");

                var clientIp = ClientIpResolver.GetClientIp(httpContext) ?? "unknown";
                
                logger.LogWarning("Login rate limit exceeded for client {ClientIp} on {Path}", clientIp, httpContext.Request.Path);

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    httpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
                }
                else
                {
                    var settings = httpContext.RequestServices.GetRequiredService<IOptions<LoginRateLimitSettings>>().Value;
                    
                    httpContext.Response.Headers.RetryAfter = settings.WindowSeconds.ToString();
                }

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Too many login attempts.",
                    Detail = "Login rate limit exceeded. Please try again later.",
                    Type = "https://tools.ietf.org/html/rfc6585#section-4"
                };

                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                
                await httpContext.Response.WriteAsJsonAsync(problemDetails, options: null, contentType: "application/problem+json", cancellationToken: cancellationToken);
            };

            options.AddPolicy(LoginRateLimitSettings.PolicyName, httpContext =>
            {
                var settings = httpContext.RequestServices.GetRequiredService<IOptions<LoginRateLimitSettings>>().Value;
                
                if (!settings.Enabled)
                {
                    return RateLimitPartition.GetNoLimiter("disabled");
                }

                return RateLimitPartition.GetSlidingWindowLimiter(ClientIpResolver.GetClientIp(httpContext) ?? "unknown",
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = settings.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                        SegmentsPerWindow = settings.SegmentsPerWindow,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }

    /// <summary>
    /// Enables the rate limiting middleware. Must be called before <c>UseAuthentication</c> when using endpoint policies.
    /// </summary>
    public static WebApplication UseLoginRateLimiting(this WebApplication app)
    {
        app.UseRateLimiter();
        return app;
    }
}
