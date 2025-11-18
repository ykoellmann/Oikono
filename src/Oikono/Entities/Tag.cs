namespace Oikono.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
}
