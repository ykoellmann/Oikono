using Oikono.Domain.Assets;
using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Recipe : AggregateRoot<RecipeId>
{
    private readonly List<Asset> _images = [];
    private readonly List<Part> _parts = [];
    private readonly List<Step> _steps = [];
    private readonly List<Tag> _tags = [];
    private readonly List<SideDish> _sideDishes = [];

    public Recipe(string name, int portions)
    {
        Name = name;
        Portions = portions;
    }

    public string Name { get; private set; }
    public int Portions { get; private set; }
    public int? Calories { get; private set; }
    public int? Rating { get; private set; }

    // Bilder
    public IReadOnlyList<Asset> Images => _images.AsReadOnly();

    // Teile (z. B. Teig, Füllung)
    public IReadOnlyList<Part> Parts => _parts.AsReadOnly();

    // Zubereitungsschritte
    public IReadOnlyList<Step> Steps => _steps.AsReadOnly();

    // Tags
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    // Beilagen
    public IReadOnlyList<SideDish> SideDishes => _sideDishes.AsReadOnly();
}