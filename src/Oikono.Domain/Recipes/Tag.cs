using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Tag : Entity<TagId>
{
    public Tag(string name)
    {
        Name = name.Trim();
    }

    public string Name { get; private set; } = null!;
    
    private readonly List<Recipe> _recipes = new();
    public IReadOnlyList<Recipe> Recipes => _recipes.AsReadOnly();
}