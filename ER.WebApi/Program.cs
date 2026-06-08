using ER.Application.DI;
using ER.Infrastructure.DI;
using ER.Infrastructure.Seeds;
using ER.WebApi.Configuration;
using ER.WebApi.Helpers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureHealthChecks();
builder.Services.ConfigureInfrastructureDependencyInjection(builder.Configuration);
builder.Services.ConfigureApplicationDependencyInjection(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
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

app.UseHttpsRedirection();

app.UseRequestTraceScope();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
