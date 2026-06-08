namespace ER.Application.Interfaces.Repositories;

/// <summary>
/// Repository contract for tenant existence checks used during registration and validation flows.
/// </summary>
public interface ITenantRepository
{
    /// <summary>
    /// Determines whether an active tenant with the specified identifier exists.
    /// </summary>
    /// <param name="tenantId">The tenant identifier to check.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when an active tenant exists; otherwise <c>false</c>.</returns>
    Task<bool> ExistsActiveAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
