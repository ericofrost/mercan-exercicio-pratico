using ER.IntegrationTests.Fixtures;
using ER.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ER.IntegrationTests.Tests;

public class LoginRateLimitTests(LoginRateLimitWebApplicationFactory factory) : IClassFixture<LoginRateLimitWebApplicationFactory>
{
    [Fact]
    public async Task Login_WhenExceedingPermitLimit_ReturnsTooManyRequestsWithProblemDetails()
    {
        using var client = factory.CreateClient();
        var request = new LoginRequest(SampleTenantData.Acme.Id, "wrong@acme.com", "WrongPass1!");

        for (var i = 0; i < 3; i++)
        {
            var response = await client.PostAsJsonAsync("/api/auth/login", request);
            response.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
        }

        var rateLimitedResponse = await client.PostAsJsonAsync("/api/auth/login", request);

        rateLimitedResponse.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        rateLimitedResponse.Headers.Contains("Retry-After").Should().BeTrue();
        rateLimitedResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problem = await rateLimitedResponse.Content.ReadFromJsonAsync<ProblemDetails>(IntegrationTestJson.Options);
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(429);
        problem.Title.Should().Be("Too many login attempts.");
    }
}
