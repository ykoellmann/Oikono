using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class IngredientId : Id<IngredientId>
{
    public IngredientId()
    {
    }

    public IngredientId(Guid value) : base(value)
    {
    }
}