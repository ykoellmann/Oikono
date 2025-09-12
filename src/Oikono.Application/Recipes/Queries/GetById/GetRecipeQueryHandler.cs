using ErrorOr;
using MapsterMapper;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Application.Recipes.Common;
using Oikono.Domain.Errors;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Application.Recipes.Queries.GetById;

public class GetRecipeQueryHandler : IQueryHandler<GetRecipeQuery, RecipeDetailResult>
{
    private readonly IRecipeRepository _recipeRepository;
    
    public GetRecipeQueryHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }
    
    public async Task<ErrorOr<RecipeDetailResult>> Handle(GetRecipeQuery request, CancellationToken ct)
    {
        var recipe = await _recipeRepository.GetByIdAsync(new RecipeId(request.Id), ct);

        if (recipe is null)
        {
            return Errors.Recipe.NotFound;
        }

        return new RecipeDetailResult(recipe);
    }
}