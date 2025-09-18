using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oikono.Application.Common.Interfaces.Persistence;

namespace Oikono.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly OikonoDbContext _dbContext;

    public UnitOfWork(OikonoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EntityFrameworkTransaction(transaction);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private class EntityFrameworkTransaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;
        private bool _disposed;

        public EntityFrameworkTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction.Dispose();
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await _transaction.DisposeAsync();
                _disposed = true;
            }
        }
    }
}
