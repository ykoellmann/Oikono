using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class PartIngredientId : Id<PartIngredientId>
{
    public PartIngredientId()
    {
    }

    public PartIngredientId(Guid value) : base(value)
    {
    }
}