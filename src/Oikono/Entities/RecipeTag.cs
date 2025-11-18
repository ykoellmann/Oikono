namespace Oikono.Entities;

public class RecipeTag
{
    public Guid RecipeId { get; set; }
    public Guid TagId { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
