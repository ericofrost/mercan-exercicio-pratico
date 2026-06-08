namespace ER.Domain.Helpers;

public static class IdentityUserNames
{
    public static string Create(Guid tenantId, string email) =>
        $"{tenantId}:{email.ToLowerInvariant()}";

    public static string NormalizeEmail(string email) =>
        email.ToUpperInvariant();
}