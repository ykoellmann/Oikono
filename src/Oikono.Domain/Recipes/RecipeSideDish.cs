using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class RecipeSideDish : Entity<RecipeSideDishId>
{
    public RecipeSideDish(RecipeId recipeId, SideDishId sideDishId)
    {
        RecipeId = recipeId;
        SideDishId = sideDishId;
    }

    public RecipeId RecipeId { get; private set; }
    public SideDishId SideDishId { get; private set; }

    public Recipe Recipe { get; private set; } = null!;
    public SideDish SideDish { get; private set; } = null!;
}