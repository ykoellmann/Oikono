using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence.Recipes;

public interface IIngredientRepository : IRepository<Ingredient, IngredientId>
{
}
