namespace Oikono.Entities;

public class Part
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid RecipeId { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public ICollection<PartIngredient> PartIngredients { get; set; } = new List<PartIngredient>();
}
