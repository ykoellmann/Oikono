using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Oikono.Infrastructure.Persistence.Repositories.Users;

public class RefreshTokenRepository : Repository<RefreshToken, RefreshTokenId>,
    IRefreshTokenRepository
{
    private readonly OikonoDbContext _dbContext;

    public RefreshTokenRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<RefreshToken> AddAsync(RefreshToken entity, UserId userId,
        CancellationToken ct)
    {
        var refreshTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.Disabled)
            .ToListAsync(ct);

        refreshTokens.ForEach(rt =>
        {
            rt.Disabled = true;
            entity.AddDomainEvent(new ClearCacheEvent<RefreshToken, RefreshTokenId>(rt));
        });

        return await base.AddAsync(entity, userId, ct);
    }
}