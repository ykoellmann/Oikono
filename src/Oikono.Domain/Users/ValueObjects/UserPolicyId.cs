using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class UserPolicyId : Id<UserPolicyId>
{
    public UserPolicyId()
    {
    }

    public UserPolicyId(Guid value) : base(value)
    {
    }
}