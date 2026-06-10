var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(ExpensesMappingProfile));
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            };
            return new BadRequestObjectResult(problemDetails);
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureHealthChecks();
builder.Services.ConfigureLoginRateLimiting(builder.Configuration);
builder.Services.ConfigureInfrastructureDependencyInjection(builder.Configuration);
builder.Services.ConfigureApplicationDependencyInjection(builder.Configuration);

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("UnhandledException");
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is not null)
        {
            logger.LogError(exception, "Unhandled exception processing {Path}", context.Request.Path);
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = OperationMessages.UnexpectedError,
                Detail = OperationMessages.UnexpectedError,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
            },
            options: null,
            contentType: "application/problem+json");
    });
});

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
}

app.MapHealthChecksEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseRequestTraceScope();
app.UseLoginRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
