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
        
        config.NewConfig(typeof(PagedResult<>), typeof(PagedResult<>))
            .Map("Items", "Items")
            .MapToConstructor(true);

        config.NewConfig<Recipe, RecipeResult>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Portions, src => src.Portions)
            .Map(dest => dest.Calories, src => src.Calories)
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.Tags, src => src.Tags);

        config.NewConfig<Domain.Recipes.Tag, TagResult>()
            .Map(dest => dest.Name, src => src.Name);
    }
}