namespace Oikono.Entities;

public class RecipeSideDish
{
    public Guid RecipeId { get; set; }
    public Guid SideDishId { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public SideDish SideDish { get; set; } = null!;
}
