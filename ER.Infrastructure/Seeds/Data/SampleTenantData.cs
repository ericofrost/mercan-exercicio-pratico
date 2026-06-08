namespace ER.Infrastructure.Seeds.Data;

/// <summary>
/// Predefined sample tenants used by development seeding.
/// </summary>
public static class SampleTenantData
{
    /// <summary>
    /// Sample Acme Corp tenant.
    /// </summary>
    public static readonly Tenant Acme = new(
        Guid.Parse("11111111-1111-1111-1111-111111111101"),
        "Acme Corp",
        10_000m);

    /// <summary>
    /// Sample Globex Inc tenant.
    /// </summary>
    public static readonly Tenant Globex = new(
        Guid.Parse("11111111-1111-1111-1111-111111111102"),
        "Globex Inc",
        15_000m);

    /// <summary>
    /// All predefined sample tenants.
    /// </summary>
    public static IReadOnlyList<Tenant> All { get; } = [Acme, Globex];
}
