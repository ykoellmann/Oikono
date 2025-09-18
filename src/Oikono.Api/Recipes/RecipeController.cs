using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Api.Common.Controllers;
using Oikono.Api.Recipes.Request;
using Oikono.Application.Recipes.Commands;
using Oikono.Application.Recipes.Queries.Get;
using Oikono.Application.Recipes.Queries.GetById;
using Oikono.Domain.Recipes;

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
    public async Task<IActionResult> GetAsync([FromQuery] GetRecipesRequest request)
    {
        var query = _mapper.Map<GetRecipesQuery>(request);
        
        var result = await _mediator.Send(query);
        
        var resullt = result.Match(Ok, Problem);

        return resullt;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var query = new GetRecipeQuery(id);
        var result = await _mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateRecipeRequest request)
    {
        var command = _mapper.Map<CreateRecipeCommand>(request);
        command.UserId = UserId;
        
        var result = await _mediator.Send(command);

        return result.Match(Ok, Problem);
    }

    [HttpGet("units")]
    public IActionResult GetUnits()
    {
        var units = Enum.GetValues(typeof(UnitType))
            .Cast<UnitType>()
            .Select(u => new
            {
                label = u.ToString(), // oder ein schönerer Text
                value = (int)u        // enum-Wert als Zahl
            })
            .ToList();

        return Ok(units);
    }
}