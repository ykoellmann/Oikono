using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Ingredient : Entity<IngredientId>
{
    public Ingredient(string name)
    {
        Name = name.Trim();
    }

    public string Name { get; private set; } = null!;
    
    private readonly List<PartIngredient> _partIngredients = new();
    public IReadOnlyList<PartIngredient> PartIngredients => _partIngredients.AsReadOnly();
}