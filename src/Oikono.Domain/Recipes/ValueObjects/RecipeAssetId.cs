using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class RecipeAssetId : Id<RecipeAssetId>
{
    public RecipeAssetId()
    {
    }

    public RecipeAssetId(Guid value) : base(value)
    {
    }
}