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
    
    private readonly List<Part> _parts = new();
    public IReadOnlyList<Part> Parts => _parts.AsReadOnly();
}