using Microsoft.Extensions.Caching.Distributed;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Assets;
using Oikono.Domain.Assets.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Assets;

public class CachedAssetRepository : CachedRepository<Asset, AssetId>, IAssetRepository
{
    public CachedAssetRepository(IRepository<Asset, AssetId> decorated, IDistributedCache cache,
        int cacheExpirationMinutes = 30) : base(decorated, cache, cacheExpirationMinutes)
    {
    }

    protected override async IAsyncEnumerable<CacheKey<Asset>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}