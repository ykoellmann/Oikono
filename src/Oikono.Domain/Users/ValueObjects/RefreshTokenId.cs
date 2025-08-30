using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class RefreshTokenId : Id<RefreshTokenId>
{
    public RefreshTokenId()
    {
    }

    public RefreshTokenId(Guid value) : base(value)
    {
    }
}