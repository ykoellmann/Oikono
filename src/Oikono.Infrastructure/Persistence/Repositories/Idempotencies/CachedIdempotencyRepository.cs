﻿using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Idempotencies;
using Oikono.Domain.Idempotencies.ValueObjects;
using Oikono.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Idempotencies;

public class CachedIdempotencyRepository : CachedRepository<Idempotency, IdempotencyId>, IIdempotencyRepository
{
    private readonly IIdempotencyRepository _decorated;
    private readonly IDistributedCache _cache;

    public CachedIdempotencyRepository(IIdempotencyRepository decorated, IDistributedCache cache) : base(decorated,
        cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    protected override async IAsyncEnumerable<CacheKey<Idempotency>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield return new CacheKey<Idempotency>(nameof(RequestExistsAsync),
            changedEvent.Changed.Id.ToString());
    }

    public async Task<bool> RequestExistsAsync(IdempotencyId idempotencyId, CancellationToken ct)
    {
        var cacheKey = new CacheKey<Idempotency>(nameof(RequestExistsAsync), idempotencyId.Value.ToString());

        return await _cache.GetOrCreateAsync(cacheKey, CacheExpiration,
            _ => _decorated.RequestExistsAsync(idempotencyId, ct));
    }
}