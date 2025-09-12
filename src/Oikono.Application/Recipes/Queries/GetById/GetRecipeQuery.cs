using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Application.Recipes.Common;

namespace Oikono.Application.Recipes.Queries.GetById;

public record GetRecipeQuery(Guid Id) : IQuery<RecipeDetailResult>;