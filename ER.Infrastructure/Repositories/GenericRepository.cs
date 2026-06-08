using System.Linq.Expressions;
using ER.Application.Interfaces.Repositories;
using ER.Domain.Base;
using ER.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ER.Infrastructure.Repositories;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseModel
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        var source = GetEntityWithIncludes(includes);

        return await source.Where(filter).AnyAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await ExistsWithFilterAsync(filter, cancellationToken, []);
    }

    public Task<(int, IEnumerable<T>)> GetPaginatedListAsync(Expression<Func<T, bool>> filter, string orderBy, string order, int rowsPerPage, int currentPage, CancellationToken cancellationToken = default)
    {
        //TODO: Apply pagination

        return null;
    }
    
    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of entities from the database, optionally including related entities specified by one or more include expressions.
    /// </summary>
    /// <param name="includes">An array of lambda expressions specifying the navigation properties to include in the query results.</param>
    /// <returns>
    /// An <see cref="IQueryable{T}"/> of entities with the specified related entities included. If no include expressions are provided, only the primary entities are returned without any related data.
    /// </returns>
    /// <remarks>
    /// This method performs eager loading of specified navigation properties to optimize query performance when related data is required. 
    /// The included entities are incorporated into the query using the <c>Include</c> method in Entity Framework, allowing efficient retrieval of related data.
    /// </remarks>
    private IQueryable<T> GetEntityWithIncludes(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        return includes.Length > 0 ? includes.Aggregate(query, (current, include) => current.Include(include)) : query;
    }
}