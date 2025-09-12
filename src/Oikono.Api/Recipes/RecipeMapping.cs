using Mapster;
using Oikono.Api.Recipes.Request;
using Oikono.Application.Common.Pagination;
using Oikono.Application.Recipes.Common;
using Oikono.Application.Recipes.Queries.Get;
using Oikono.Domain.Recipes;

namespace Oikono.Api.Recipes;

public class RecipeMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RecipeRequest, GetRecipesQuery>()
            .MapToConstructor(true);
        
        config.NewConfig<PagedResult<Recipe>, PagedResult<RecipeResult>>()
            .MapToConstructor(true);
    }
}