namespace Oikono.Entities;

public class Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Portions { get; set; }
    public int? Calories { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Part> Parts { get; set; } = new List<Part>();
    public ICollection<Step> Steps { get; set; } = new List<Step>();
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
    public ICollection<RecipeSideDish> RecipeSideDishes { get; set; } = new List<RecipeSideDish>();
    public ICollection<RecipeAsset> RecipeAssets { get; set; } = new List<RecipeAsset>();
}
