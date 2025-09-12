using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class SideDish : Entity<SideDishId>
{
    public SideDish(string name)
    {
        Name = name.Trim();
    }

    public string Name { get; private set; } = null!;

    // Optional: Navigation zurück zu den Recipes
    private readonly List<Recipe> _recipes = new();
    public IReadOnlyList<Recipe> Recipes => _recipes.AsReadOnly();
}