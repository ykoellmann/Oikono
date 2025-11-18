namespace Oikono.Entities;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<PartIngredient> PartIngredients { get; set; } = new List<PartIngredient>();
}
