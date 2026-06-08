namespace ER.Application.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<bool> ExistsActiveAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
