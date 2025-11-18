namespace Oikono.Entities;

public class SideDish
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<RecipeSideDish> RecipeSideDishes { get; set; } = new List<RecipeSideDish>();
}
