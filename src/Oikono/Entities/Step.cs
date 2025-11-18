namespace Oikono.Entities;

public class Step
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public TimeSpan? Duration { get; set; }
    public int? Temperature { get; set; }
    public Guid RecipeId { get; set; }
    public Guid? DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public Device? Device { get; set; }
}
