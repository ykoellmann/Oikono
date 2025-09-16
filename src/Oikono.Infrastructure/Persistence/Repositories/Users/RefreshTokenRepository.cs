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
        
        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);

        // Sperrt alle aktiven Tokens für diesen User
        var refreshTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.Disabled)
            .Where(rt => rt.Token != entity.Token)
            .ToListAsync(ct);

        // Neues Token einfügen
        var result = await base.AddAsync(entity, userId, ct);

        refreshTokens.ForEach(rt =>
        {
            rt.Disabled = true;
            entity.AddDomainEvent(new ClearCacheEvent<RefreshToken?, RefreshTokenId>(rt));
        });

        await _dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return result;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken && !rt.Disabled)
            .SingleOrDefaultAsync();
        
    }
}