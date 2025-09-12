using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class RecipeSideDishId : Id<RecipeSideDishId>
{
    public RecipeSideDishId()
    {
    }

    public RecipeSideDishId(Guid value) : base(value)
    {
    }
}