using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Part : Entity<PartId>
{
    private readonly List<PartIngredient> _ingredients = [];

    public Part(RecipeId recipeId, string name)
    {
        RecipeId = recipeId;
        Name = name;
    }

    public RecipeId RecipeId { get; private set; }
    public Recipe Recipe { get; private set; } = null!;
    public string Name { get; private set; }

    public IReadOnlyList<PartIngredient> Ingredients => _ingredients.AsReadOnly();

    public void AddIngredient(PartIngredient ingredient)
    {
        _ingredients.Add(ingredient);
    }
}