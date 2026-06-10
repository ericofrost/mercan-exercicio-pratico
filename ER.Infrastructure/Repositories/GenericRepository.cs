namespace ER.Infrastructure.Repositories;

/// <summary>
/// Generic Entity Framework Core repository providing basic persistence operations for entities derived from <see cref="BaseModel"/>.
/// </summary>
/// <typeparam name="T">The entity type stored by the repository.</typeparam>
public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseModel
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortProperties = BuildAllowedSortProperties();

    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    /// <inheritdoc />
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);

    /// <inheritdoc />
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.SingleAsync(x => x.Id == entity.Id, cancellationToken);
        
        CopyProperties(entity, entry);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }
    
    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> GetAllWithFiltersAsync(Expression<Func<T, bool>> filter,CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        var source = GetEntityWithIncludes(includes);
        
        return await source.Where(filter).ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        var source = GetEntityWithIncludes(includes);

        return await source.Where(filter).AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await ExistsWithFilterAsync(filter, cancellationToken, []);
    }

    /// <inheritdoc />
    public virtual async Task<(int TotalCount, IEnumerable<T> Records)> GetPaginatedListAsync(Expression<Func<T, bool>> filter, string orderBy, string order, int rowsPerPage, int currentPage, CancellationToken cancellationToken = default)
    {
        var page = Math.Max(currentPage, 1);
        var pageSize = Math.Clamp(rowsPerPage, 1, 100);
        var skip = (page - 1) * pageSize;

        var baseQuery = _dbSet.AsNoTracking().Where(filter);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await ApplyOrdering(baseQuery, orderBy, order)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (totalCount, items);
    }

    /// <summary>
    /// Applies whitelist-validated ordering to a query using translatable <see cref="EF.Property{TProperty}"/> expressions.
    /// </summary>
    private static IQueryable<T> ApplyOrdering(IQueryable<T> query, string orderBy, string order)
    {
        var property = !string.IsNullOrWhiteSpace(orderBy) && AllowedSortProperties.TryGetValue(orderBy, out var canonical)
            ? canonical
            : nameof(BaseModel.Id);

        var descending = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);

        return descending
            ? query.OrderByDescending(e => EF.Property<object>(e, property))
            : query.OrderBy(e => EF.Property<object>(e, property));
    }

    private static Dictionary<string, string> BuildAllowedSortProperties()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.CanRead && IsScalarSortProperty(p))
            .ToDictionary(p => p.Name, p => p.Name, StringComparer.OrdinalIgnoreCase);

        properties.TryAdd(nameof(BaseModel.Id), nameof(BaseModel.Id));

        return properties;
    }

    private static bool IsScalarSortProperty(System.Reflection.PropertyInfo property)
    {
        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        if (type.IsEnum)
            return true;

        if (type == typeof(string) || type == typeof(Guid) || type == typeof(decimal) ||
            type == typeof(DateTime) || type == typeof(DateOnly) || type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan))
            return true;

        return type.IsPrimitive;
    }

    /// <summary>
    /// Builds a queryable source with optional eager-loaded navigation properties.
    /// </summary>
    /// <param name="includes">Navigation properties to include in the query.</param>
    /// <returns>An <see cref="IQueryable{T}"/> prepared for further filtering.</returns>
    protected virtual IQueryable<T> GetEntityWithIncludes(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        return includes.Length > 0 ? includes.Aggregate(query, (current, include) => current.Include(include)) : query;
    }
    
    /// <summary>
    /// Copies Changed properties during update.
    /// </summary>
    /// <param name="source">The source entity</param>
    /// <param name="destination">The destination entity tracked from database.</param>
    private static void CopyProperties(T source, T destination)
    {
        var sourceProps = source.GetType().GetProperties().ToList();

        sourceProps.ForEach(sourceProp =>
            {
                var sourcePropVal = sourceProp.GetValue(source);
                
                var destinationProp = destination.GetType().GetProperty(sourceProp.Name);
                
                var destPropVal = destination.GetType().GetProperty(sourceProp.Name)?.GetValue(source);

                if ((!sourcePropVal?.Equals(destPropVal)) ?? true)
                {
                    destinationProp?.SetValue(destination, sourceProp.GetValue(source));
                }
            }
        );
    }
}
