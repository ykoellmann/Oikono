using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class CachedSideDishRepository : CachedRepository<SideDish, SideDishId>, ISideDishRepository
{
    private readonly IDistributedCache _cache;
    private readonly ISideDishRepository _decorated;

    public CachedSideDishRepository(ISideDishRepository decorated, IDistributedCache cache) : base(decorated, cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    protected override async IAsyncEnumerable<CacheKey<SideDish>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}
