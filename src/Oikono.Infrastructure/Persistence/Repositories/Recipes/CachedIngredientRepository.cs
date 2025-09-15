using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class CachedIngredientRepository : CachedRepository<Ingredient, IngredientId>, IIngredientRepository
{
    private readonly IDistributedCache _cache;
    private readonly IIngredientRepository _decorated;

    public CachedIngredientRepository(IIngredientRepository decorated, IDistributedCache cache) : base(decorated, cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    protected override async IAsyncEnumerable<CacheKey<Ingredient>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield break;
    }
}
