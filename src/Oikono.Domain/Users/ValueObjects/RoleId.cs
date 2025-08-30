using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class RoleId : Id<RoleId>
{
    public RoleId()
    {
    }

    public RoleId(Guid value) : base(value)
    {
    }
}