namespace ER.Infrastructure.Repositories;

/// <summary>
/// Generic Entity Framework Core repository providing basic persistence operations for entities derived from <see cref="BaseModel"/>.
/// </summary>
/// <typeparam name="T">The entity type stored by the repository.</typeparam>
public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseModel
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    /// <inheritdoc />
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        var source = GetEntityWithIncludes(includes);

        return await source.Where(filter).AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await ExistsWithFilterAsync(filter, cancellationToken, []);
    }

    /// <inheritdoc />
    public Task<(int, IEnumerable<T>)> GetPaginatedListAsync(Expression<Func<T, bool>> filter, string orderBy, string order, int rowsPerPage, int currentPage, CancellationToken cancellationToken = default)
    {
        // TODO: Apply pagination
        return Task.FromResult<(int, IEnumerable<T>)>((0, []));
    }

    /// <summary>
    /// Builds a queryable source with optional eager-loaded navigation properties.
    /// </summary>
    /// <param name="includes">Navigation properties to include in the query.</param>
    /// <returns>An <see cref="IQueryable{T}"/> prepared for further filtering.</returns>
    private IQueryable<T> GetEntityWithIncludes(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        return includes.Length > 0 ? includes.Aggregate(query, (current, include) => current.Include(include)) : query;
    }
}
