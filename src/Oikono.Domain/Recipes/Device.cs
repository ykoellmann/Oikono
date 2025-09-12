using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Device : Entity<DeviceId>
{
    public Device(string name)
    {
        Name = name.Trim();
    }

    public string Name { get; private set; } = null!;
}