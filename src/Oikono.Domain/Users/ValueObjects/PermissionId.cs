using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class PermissionId : Id<PermissionId>
{
    public PermissionId()
    {
    }

    public PermissionId(Guid value) : base(value)
    {
    }
}