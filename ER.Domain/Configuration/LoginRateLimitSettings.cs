namespace ER.Domain.Configuration;

/// <summary>
/// Rate limiting options for the login endpoint, bound from the <c>LoginRateLimit</c> configuration section.
/// </summary>
public record LoginRateLimitSettings
{
    /// <summary>
    /// Configuration section name used in <c>appsettings.json</c>.
    /// </summary>
    public const string SectionName = "LoginRateLimit";

    /// <summary>
    /// ASP.NET Core rate limiting policy name applied to <c>POST /api/auth/login</c>.
    /// </summary>
    public const string PolicyName = "Login";

    /// <summary>
    /// When false, the login policy resolves to a no-op limiter (no throttling).
    /// </summary>
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Maximum login requests allowed per client IP within the sliding window.
    /// </summary>
    public int PermitLimit { get; init; } = 10;

    /// <summary>
    /// Sliding window duration in seconds.
    /// </summary>
    public int WindowSeconds { get; init; } = 60;

    /// <summary>
    /// Number of segments per sliding window (granularity).
    /// </summary>
    public int SegmentsPerWindow { get; init; } = 4;
}
