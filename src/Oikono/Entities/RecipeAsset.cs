namespace Oikono.Entities;

public class RecipeAsset
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public Guid RecipeId { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
}
