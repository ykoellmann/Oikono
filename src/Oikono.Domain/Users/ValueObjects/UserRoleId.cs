using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class UserRoleId : Id<UserRoleId>
{
    public UserRoleId()
    {
    }

    public UserRoleId(Guid value) : base(value)
    {
    }
}