using Oikono.Domain.Assets;
using Oikono.Domain.Assets.ValueObjects;
using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class RecipeAsset : Entity<RecipeAssetId>
{
    public RecipeAsset(RecipeId recipeId, AssetId assetId)
    {
        RecipeId = recipeId;
        AssetId = assetId;
    }
    
    public RecipeId RecipeId { get; private set; }
    public AssetId AssetId { get; private set; }
    
    public Recipe Recipe { get; private set; }
    public Asset Asset { get; private set; }
}