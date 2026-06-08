namespace ER.Domain.Helpers;

/// <summary>
/// Helper methods for constructing ASP.NET Identity usernames in a multi-tenant environment.
/// </summary>
public static class IdentityUserNames
{
    /// <summary>
    /// Characters permitted in tenant-scoped usernames ({tenantId}:{email}), including the separator colon.
    /// </summary>
    public const string AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+:";

    /// <summary>
    /// Creates the tenant-scoped username stored by ASP.NET Identity.
    /// </summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="email">The user email address.</param>
    /// <returns>A globally unique username in the form <c>{tenantId}:{email}</c>.</returns>
    public static string Create(Guid tenantId, string email) =>
        $"{tenantId}:{email.ToLowerInvariant()}";

    /// <summary>
    /// Normalizes an email address for Identity storage and lookup.
    /// </summary>
    /// <param name="email">The email address to normalize.</param>
    /// <returns>The upper-invariant normalized email value.</returns>
    public static string NormalizeEmail(string email) =>
        email.ToUpperInvariant();
}
