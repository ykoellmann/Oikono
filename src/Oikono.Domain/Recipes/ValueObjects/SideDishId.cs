using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class SideDishId : Id<SideDishId>
{
    public SideDishId()
    {
    }

    public SideDishId(Guid value) : base(value)
    {
    }
}