using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class UserId : Id<UserId>
{
    public UserId()
    {
    }

    public UserId(Guid value) : base(value)
    {
    }
}