using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Idempotencies;
using Oikono.Domain.Idempotencies.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Oikono.Infrastructure.Persistence.Repositories.Idempotencies;

public class IdempotencyRepository : Repository<Idempotency, IdempotencyId>, IIdempotencyRepository
{
    private readonly OikonoDbContext _dbContext;

    public IdempotencyRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> RequestExistsAsync(IdempotencyId id, CancellationToken ct)
    {
        return _dbContext.Set<Idempotency>()
            .AnyAsync(x => x.Id == id, ct);
    }
}