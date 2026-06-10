namespace ER.WebApi.Helpers;

/// <summary>
/// Resolves the client IP address for rate limiting partitions.
/// </summary>
public static class ClientIpResolver
{
    /// <summary>
    /// Returns the remote IP address from the HTTP connection.
    /// When deployed behind a reverse proxy, enable forwarded headers so this reflects the real client IP.
    /// </summary>
    public static string? GetClientIp(HttpContext httpContext)
        => httpContext.Connection.RemoteIpAddress?.ToString();
}
