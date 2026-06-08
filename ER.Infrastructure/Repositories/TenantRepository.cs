using ER.Application.Interfaces.Repositories;
using ER.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Repositories;

public class TenantRepository(ApplicationDbContext context) : ITenantRepository
{
    public Task<bool> ExistsActiveAsync(Guid tenantId, CancellationToken cancellationToken = default) => context.Tenants.AnyAsync(t => t.Id == tenantId && t.IsActive, cancellationToken);
}
