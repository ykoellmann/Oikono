using System.Security.Cryptography;
using Oikono.Domain.Models;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users;

public class RefreshToken : Entity<RefreshTokenId>
{
    public RefreshToken(UserId userId)
    {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        Expires = DateTime.UtcNow.AddDays(3);
        UserId = userId;
    }

    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool Disabled { get; set; }
    public bool Expired => Disabled || Expires < DateTime.UtcNow;
    public UserId UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}