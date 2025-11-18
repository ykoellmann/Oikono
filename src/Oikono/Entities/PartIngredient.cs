namespace Oikono.Entities;

public class PartIngredient
{
    public Guid Id { get; set; }
    public Guid PartId { get; set; }
    public Guid IngredientId { get; set; }
    public double Amount { get; set; }
    public UnitType Unit { get; set; }

    // Navigation properties
    public Part Part { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}
