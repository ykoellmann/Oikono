using Oikono.Domain.Idempotencies;
using Oikono.Domain.Idempotencies.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence;

public interface IIdempotencyRepository : IRepository<Idempotency, IdempotencyId>
{
    Task<bool> RequestExistsAsync(IdempotencyId idempotencyId, CancellationToken ct);
}