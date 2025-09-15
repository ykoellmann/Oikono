using MediatR;
using Oikono.Api.Common.Controllers;
using Oikono.Api.SideDishes.Request;
using Oikono.Api.SideDishes.Response;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.SideDishes;

public class SideDishController : Controller<ISideDishRepository, SideDish, SideDishId, SideDishRequest, SideDishResponse>
{
    public SideDishController(ISender mediator) : base(mediator)
    {
    }
}
