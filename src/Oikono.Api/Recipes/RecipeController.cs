using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Api.Common.Controllers;
using Oikono.Api.Recipes.Request;
using Oikono.Application.Recipes.Queries.Get;

namespace Oikono.Api.Recipes;

[Route("api/recipes")]
public class RecipeController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public RecipeController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([AsParameters] RecipeRequest request)
    {
        var query = _mapper.Map<GetRecipesQuery>(request);
        
        var result = await _mediator.Send(query);
        
        return result.Match(Ok, Problem);
    }
}