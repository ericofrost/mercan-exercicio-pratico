using ER.Domain.Shared;

namespace ER.Application.Interfaces.Authentication;

/// <summary>
/// Application service contract for generating JWT access tokens.
/// </summary>
public interface ITokenGeneratorService
{
    /// <summary>
    /// Generates a signed JWT access token from the supplied employee and tenant claims data.
    /// </summary>
    /// <param name="request">The token generation input model.</param>
    /// <returns>The encoded JWT access token, or <c>null</c> when token creation fails.</returns>
    ValueTask<string?> GenerateTokenAsync(GenerateTokenRequest request);
}
