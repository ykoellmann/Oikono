using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class UserPermissionId : Id<UserPermissionId>
{
    public UserPermissionId()
    {
    }

    public UserPermissionId(Guid value) : base(value)
    {
    }
}