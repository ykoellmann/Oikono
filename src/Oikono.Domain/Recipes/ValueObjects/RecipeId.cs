using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class RecipeId : Id<RecipeId>
{
    public RecipeId()
    {
    }

    public RecipeId(Guid value) : base(value)
    {
    }
}