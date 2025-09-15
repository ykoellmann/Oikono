using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class DeviceRepository : Repository<Device, DeviceId>, IDeviceRepository
{
    private readonly OikonoDbContext _dbContext;

    public DeviceRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
