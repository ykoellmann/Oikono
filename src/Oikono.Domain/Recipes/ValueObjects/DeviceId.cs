using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class DeviceId : Id<DeviceId>
{
    public DeviceId()
    {
    }

    public DeviceId(Guid value) : base(value)
    {
    }
}