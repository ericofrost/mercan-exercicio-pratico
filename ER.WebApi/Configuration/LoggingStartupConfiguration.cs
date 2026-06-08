namespace ER.WebApi.Configuration;

/// <summary>
/// Configures structured logging and request correlation for the ExpenseReports API.
/// </summary>
public static class LoggingStartupConfiguration
{
    /// <summary>
    /// Adds a request scope with the ASP.NET Core trace identifier for structured log correlation.
    /// </summary>
    /// <param name="app">The web application pipeline.</param>
    public static void UseRequestTraceScope(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ER.WebApi.Request");

            using (logger.BeginScope(new Dictionary<string, object>
                   {
                       ["TraceId"] = context.TraceIdentifier
                   }))
            {
                await next(context);
            }
        });
    }
}
