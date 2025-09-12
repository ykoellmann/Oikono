using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class RecipeTag : Entity<RecipeTagId>
{
    public RecipeTag(RecipeId recipeId, TagId tagId)
    {
        RecipeId = recipeId;
        TagId = tagId;
    }

    public RecipeId RecipeId { get; private set; }
    public TagId TagId { get; private set; }

    public Recipe Recipe { get; private set; } = null!;
    public Tag Tag { get; private set; } = null!;
}