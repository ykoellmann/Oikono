using Oikono.Domain.Models;

namespace Oikono.Application.Common.Events;

public record ClearCacheEvent<TEntity, TId>(TEntity Changed) : IDomainEvent
    where TEntity : Entity<TId>
    where TId : Id<TId>, new();