using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class PartId : Id<PartId>
{
    public PartId()
    {
    }

    public PartId(Guid value) : base(value)
    {
    }
}