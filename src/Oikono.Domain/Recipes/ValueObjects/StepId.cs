using Oikono.Domain.Models;

namespace Oikono.Domain.Recipes.ValueObjects;

public class StepId : Id<StepId>
{
    public StepId()
    {
    }

    public StepId(Guid value) : base(value)
    {
    }
}