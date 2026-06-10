namespace ER.Application.Interfaces.Repositories;

/// <summary>
/// Generic persistence contract for entities deriving from <see cref="BaseModel"/>.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IGenericRepository<T> where T : BaseModel
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an entity on the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filter">Query filter</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Entities to eager load</param>
    /// <returns>A collection of all entities that match the filter.</returns>
    Task<IEnumerable<T>> GetAllWithFiltersAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
    
    /// <summary>
    /// Checks if any entities of type <typeparamref name="T"/> exist in the database 
    /// that satisfy the given filter criteria, with optional related entities included.
    /// </summary>
    /// <param name="filter">An expression to filter the entities of type <typeparamref name="T"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">Optional expressions to include related entities (e.g., for eager loading).</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to <c>true</c> if any entity matching the filter exists; 
    /// otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Checks if any entities of type <typeparamref name="T"/> exist in the database 
    /// that satisfy the given filter criteria, with optional related entities included.
    /// </summary>
    /// <param name="filter">An expression to filter the entities of type <typeparamref name="T"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to <c>true</c> if any entity matching the filter exists; 
    /// otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ExistsWithFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a paginated list of entities based on the provided filter and sorting criteria, along with the total count of records.
    /// </summary>
    /// <param name="filter">A LINQ expression to filter the entities.</param>
    /// <param name="orderBy">The property to order the results by.</param>
    /// <param name="order">The sorting direction, either "asc" for ascending or "desc" for descending order.</param>
    /// <param name="rowsPerPage">The number of rows to return per page.</param>
    /// <param name="currentPage">The current page number to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing the total count of matching records and a paginated collection of entities.</returns>
    Task<(int, IEnumerable<T>)> GetPaginatedListAsync(Expression<Func<T, bool>> filter, string orderBy, string order, int rowsPerPage, int currentPage, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves entities matching the filter with optional related entities included.
    /// </summary>
    /// <param name="collectionSearch">Pagination and sorting parameters for the query.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="include">Optional expression to include related entities.</param>
    /// <param name="predicate">A LINQ expression to filter the entities.</param>
    /// <returns>The matching entities for the requested page.</returns>
    //Task<IEnumerable<T>> GetWithFilterPaginationAndIncludesAsync(RequestResponse collectionSearch, Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}