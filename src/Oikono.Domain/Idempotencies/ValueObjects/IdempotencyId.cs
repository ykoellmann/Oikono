using Oikono.Domain.Models;

namespace Oikono.Domain.Idempotencies.ValueObjects;

public class IdempotencyId : Id<IdempotencyId>
{
    public IdempotencyId()
    {
    }

    public IdempotencyId(Guid value) : base(value)
    {
    }
}