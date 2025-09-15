using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class SideDishRepository : Repository<SideDish, SideDishId>, ISideDishRepository
{
    private readonly OikonoDbContext _dbContext;

    public SideDishRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
