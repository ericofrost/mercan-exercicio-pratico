using ER.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace ER.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the underlying database context.
    /// </summary>
    DbContext Context { get; }
    
    /// <summary>
    /// Gets the repository for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <returns>The repository for the entity type.</returns>
    IGenericRepository<T> Repository<T>() where T : BaseModel;
    
    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Starts a new transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackTransactionAsync();
}