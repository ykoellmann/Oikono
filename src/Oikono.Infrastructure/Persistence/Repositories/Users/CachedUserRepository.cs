﻿using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Oikono.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Users;

public class CachedUserRepository : CachedRepository<User, UserId>, IUserRepository
{
    private readonly IDistributedCache _cache;
    private readonly IUserRepository _decorated;

    public CachedUserRepository(IUserRepository decorated, IDistributedCache cache) : base(decorated, cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _decorated.GetByEmailAsync(email, ct);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct)
    {
        var cacheKey = new CacheKey<User>(nameof(IsEmailUniqueAsync), email);

        return await _cache.GetOrCreateAsync(cacheKey, CacheExpiration,
            _ => _decorated.IsEmailUniqueAsync(email, ct));
    }

    public async Task<User> AddAsync(User entity, CancellationToken ct)
    {
        entity.AddDomainEvent(new ClearCacheEvent<User, UserId>(entity));

        var addedEntity = await _decorated.AddAsync(entity, ct);
        var cacheKey = new CacheKey<User>(nameof(GetByIdAsync), addedEntity.Id.Value.ToString());

        return await Cache.GetOrCreateAsync(cacheKey, CacheExpiration, async _ => addedEntity);
    }

    [Obsolete("This method is replaced by its overload")]
    public override async Task<User> AddAsync(User entity, UserId userId,
        CancellationToken ct)
    {
        throw new NotImplementedException("This method is replaced by its overload");
    }

    protected override async IAsyncEnumerable<CacheKey<User>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield return new CacheKey<User>(nameof(GetByEmailAsync),
            changedEvent.Changed.Email);
        yield return new CacheKey<User>(nameof(IsEmailUniqueAsync),
            changedEvent.Changed.Email);
    }
}