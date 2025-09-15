using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class TagRepository : Repository<Tag, TagId>, ITagRepository
{
    private readonly OikonoDbContext _dbContext;

    public TagRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
