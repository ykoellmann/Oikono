using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class TagId : Id<TagId>
{
    public TagId()
    {
    }

    public TagId(Guid value) : base(value)
    {
    }
}