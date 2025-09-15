using Mapster;
using Oikono.Api.Ingredients.Request;
using Oikono.Api.Ingredients.Response;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Ingredients;

internal class IngredientMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IngredientRequest, Domain.Recipes.Ingredient>()
            .MapToConstructor(true);

        config.NewConfig<Domain.Recipes.Ingredient, IngredientResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .MapToConstructor(true);

        config.NewConfig<IngredientRequest, CreateCommand<Domain.Recipes.Ingredient, IngredientId, IngredientRequest, IngredientResponse>>()
            .MapToConstructor(true);

        config.NewConfig<(IngredientId, IngredientRequest), UpdateCommand<Domain.Recipes.Ingredient, IngredientId, IngredientRequest, IngredientResponse>>()
            .MapToConstructor(true);
    }
}
