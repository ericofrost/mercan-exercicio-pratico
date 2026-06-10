using System.Net.Http.Headers;
using ER.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ER.IntegrationTests.Helpers;

public class AuthApiClient(WebApplicationFactory<Program> factory)
{
    public async Task<LoginResponse> LoginAsync(Guid tenantId, string email, string password, CancellationToken cancellationToken = default)
    {
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(tenantId, email, password), cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LoginResponse>(IntegrationTestJson.Options, cancellationToken))!;
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(Employee employee, CancellationToken cancellationToken = default)
        => await CreateAuthenticatedClientAsync(employee.TenantId, employee.Email, SampleUserCredentials.DefaultPassword, cancellationToken);

    public async Task<HttpClient> CreateAuthenticatedClientAsync(Guid tenantId, string email, string password, CancellationToken cancellationToken = default)
    {
        var login = await LoginAsync(tenantId, email, password, cancellationToken);
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.AccessToken);
        return client;
    }
}
