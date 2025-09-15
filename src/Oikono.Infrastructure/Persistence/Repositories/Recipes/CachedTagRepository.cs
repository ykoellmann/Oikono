using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class CachedTagRepository : CachedRepository<Tag, TagId>, ITagRepository
{
    private readonly IDistributedCache _cache;
    private readonly ITagRepository _decorated;

    public CachedTagRepository(ITagRepository decorated, IDistributedCache cache) : base(decorated, cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    protected override async IAsyncEnumerable<CacheKey<Tag>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}
