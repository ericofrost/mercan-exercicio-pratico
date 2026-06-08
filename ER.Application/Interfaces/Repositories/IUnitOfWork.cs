namespace ER.Application.Interfaces.Repositories;

/// <summary>
/// Coordinates repositories, save changes, and database transactions for a single request scope.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the underlying Entity Framework database context.
    /// </summary>
    DbContext Context { get; }

    /// <summary>
    /// Gets the generic repository for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type deriving from <see cref="BaseModel"/>.</typeparam>
    /// <returns>The repository for the requested entity type.</returns>
    IGenericRepository<T> Repository<T>() where T : BaseModel;

    /// <summary>
    /// Persists all pending changes tracked by the current context.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a new database transaction or participates in the current ambient transaction.
    /// </summary>
    /// <returns>A task that completes when the transaction is ready.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a transaction is already in progress.</exception>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current database transaction when this unit of work owns it.
    /// </summary>
    /// <returns>A task that completes when the transaction has been committed.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is in progress.</exception>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current database transaction when this unit of work owns it.
    /// </summary>
    /// <returns>A task that completes when the transaction has been rolled back.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is in progress.</exception>
    Task RollbackTransactionAsync();
}
