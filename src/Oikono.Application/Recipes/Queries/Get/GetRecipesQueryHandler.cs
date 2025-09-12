using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Recipes.Common;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Application.Common.Pagination;

namespace Oikono.Application.Recipes.Queries.Get;

public class GetRecipesQueryHandler : IQueryHandler<GetRecipesQuery, PagedResult<RecipeResult>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;

    public GetRecipesQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResult<RecipeResult>>> Handle(GetRecipesQuery query, CancellationToken ct)
    {
        var data = await _recipeRepository.GetFilteredListAsync(ct, query);

        return _mapper.Map<PagedResult<RecipeResult>>(data);
    }
}