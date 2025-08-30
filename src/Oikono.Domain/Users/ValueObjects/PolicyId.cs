using Oikono.Domain.Models;

namespace Oikono.Domain.Users.ValueObjects;

public class PolicyId : Id<PolicyId>
{
    public PolicyId()
    {
    }

    public PolicyId(Guid value) : base(value)
    {
    }
}