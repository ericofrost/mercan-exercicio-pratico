using ER.Application.Interfaces.Repositories;
using ER.Domain.Base;
using ER.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ER.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction? _currentTransaction;
    private bool _ownsTransaction;

    public DbContext Context => _dbContext;

    public IGenericRepository<T> Repository<T>() where T : BaseModel
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var value)) return (IGenericRepository<T>)value;
        
        var repositoryType = typeof(IGenericRepository<>).MakeGenericType(type);
        var repositoryInstance = Activator.CreateInstance(repositoryType, _dbContext);
        value = repositoryInstance;

        ArgumentNullException.ThrowIfNull(value);

        _repositories.Add(type, value);

        return (IGenericRepository<T>)value;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);

    public Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        var ambient = _dbContext.Database.CurrentTransaction;
        
        if (ambient is null) return BeginOwnedTransactionAsync();
        
        _currentTransaction = ambient;
        _ownsTransaction = false;
        return Task.CompletedTask;

    }

    private async Task BeginOwnedTransactionAsync()
    {
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        _ownsTransaction = true;
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        if (_ownsTransaction)
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
        }

        _currentTransaction = null;
        _ownsTransaction = false;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        if (_ownsTransaction)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
        }

        _currentTransaction = null;
        _ownsTransaction = false;
    }

    public void Dispose()
    {
        if (_ownsTransaction)
        {
            _currentTransaction?.Dispose();
        }

        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}