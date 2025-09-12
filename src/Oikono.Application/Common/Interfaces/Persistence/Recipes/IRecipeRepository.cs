using Oikono.Application.Common.Pagination;
using Oikono.Application.Recipes.Queries.Get;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence.Recipes;

public interface IRecipeRepository : IRepository<Recipe, RecipeId>
{
    Task<PagedResult<Recipe>> GetFilteredListAsync(CancellationToken ct,
        GetRecipesQuery filter);
}