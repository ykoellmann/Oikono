namespace Oikono.Entities;

public class Device
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<Step> Steps { get; set; } = new List<Step>();
}
