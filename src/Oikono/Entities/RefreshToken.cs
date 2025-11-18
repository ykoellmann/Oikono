namespace Oikono.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool Disabled { get; set; }
    public Guid UserId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
}
