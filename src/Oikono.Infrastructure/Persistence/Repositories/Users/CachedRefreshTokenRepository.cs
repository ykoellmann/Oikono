using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Users;

public class CachedRefreshTokenRepository : CachedRepository<RefreshToken, RefreshTokenId>, IRefreshTokenRepository
{
    public CachedRefreshTokenRepository(IRefreshTokenRepository decorated, IDistributedCache cache) :
        base(decorated, cache)
    {
    }

    protected override async IAsyncEnumerable<CacheKey<RefreshToken>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}