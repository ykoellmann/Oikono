using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class RecipeTagId : Id<RecipeTagId>
{
    public RecipeTagId()
    {
    }

    public RecipeTagId(Guid value) : base(value)
    {
    }
}