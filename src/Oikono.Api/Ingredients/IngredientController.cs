using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Api.Common.Controllers;
using Oikono.Api.Ingredients.Request;
using Oikono.Api.Ingredients.Response;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Ingredients;

[Route("api/[controller]")]
public class IngredientController : Controller<IIngredientRepository, Ingredient, IngredientId, IngredientRequest, IngredientResponse>
{
    public IngredientController(ISender mediator) : base(mediator)
    {
    }
}
