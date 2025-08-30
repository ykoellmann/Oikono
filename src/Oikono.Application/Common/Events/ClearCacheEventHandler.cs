using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Models;
using MediatR;

namespace Oikono.Application.Common.Events;

public class ClearCacheEventHandler<TIRepository, TEntity, TId, TChanged> : INotificationHandler<TChanged>
    where TChanged : ClearCacheEvent<TEntity, TId>
    where TIRepository : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : Id<TId>, new()
{
    private readonly TIRepository _repository;

    public ClearCacheEventHandler(TIRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(TChanged changedEvent, CancellationToken ct)
    {
        await _repository.ClearCacheAsync(changedEvent);
    }
}