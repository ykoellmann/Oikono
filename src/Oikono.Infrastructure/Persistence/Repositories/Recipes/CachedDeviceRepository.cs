using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class CachedDeviceRepository : CachedRepository<Device, DeviceId>, IDeviceRepository
{
    private readonly IDistributedCache _cache;
    private readonly IDeviceRepository _decorated;

    public CachedDeviceRepository(IDeviceRepository decorated, IDistributedCache cache) : base(decorated, cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    protected override async IAsyncEnumerable<CacheKey<Device>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}
