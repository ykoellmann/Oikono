using Microsoft.Extensions.Caching.Distributed;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Application.Common.Pagination;
using Oikono.Application.Recipes.Queries.Get;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Oikono.Infrastructure.Extensions;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class CachedRecipeRepository : CachedRepository<Recipe, RecipeId>, IRecipeRepository
{
    private readonly IRecipeRepository _decorated;

    public CachedRecipeRepository(IRecipeRepository decorated, IDistributedCache cache,
        int cacheExpirationMinutes = 10) : base(decorated, cache, cacheExpirationMinutes)
    {
        _decorated = decorated;
    }

    protected override async IAsyncEnumerable<CacheKey<Recipe>> GetCacheKeysAsync<TChanged>(TChanged changedEvent)
    {
        yield return new CacheKey<Recipe>(nameof(GetCacheKeysAsync));
    }

    public async Task<PagedResult<Recipe>> GetFilteredListAsync(CancellationToken ct, GetRecipesQuery filter)
    {
        var cacheKey = new CacheKey<Recipe>(nameof(GetCacheKeysAsync));

        return await Cache.GetOrCreateAsync(cacheKey, CacheExpiration,
            _ => _decorated.GetFilteredListAsync(ct, filter));
    }
}