namespace ER.Domain.Configuration;

/// <summary>
/// JWT bearer authentication options bound from the <c>Jwt</c> configuration section.
/// </summary>
public record JwtSettings
{
    /// <summary>
    /// Configuration section name used in <c>appsettings.json</c>.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Token issuer validated by the API and embedded in generated access tokens.
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Token audience validated by the API and embedded in generated access tokens.
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    /// Symmetric signing key used to sign and validate JWT access tokens.
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    /// Access token lifetime in minutes.
    /// </summary>
    public required int ExpiryMinutes { get; init; }
}
