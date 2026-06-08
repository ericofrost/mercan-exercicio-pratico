namespace ER.Application.Authentication;

/// <summary>
/// Successful login response containing the issued JWT access token.
/// </summary>
/// <param name="AccessToken">The signed JWT access token.</param>
/// <param name="ExpiresAt">The UTC expiration timestamp of the access token.</param>
public record LoginResponse(string AccessToken, DateTime ExpiresAt);
