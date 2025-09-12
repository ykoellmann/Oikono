using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Assets;
using Oikono.Domain.Assets.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Assets;

public class AssetRepository : Repository<Asset, AssetId>, IAssetRepository
{
    private readonly OikonoDbContext _dbContext;

    public AssetRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}