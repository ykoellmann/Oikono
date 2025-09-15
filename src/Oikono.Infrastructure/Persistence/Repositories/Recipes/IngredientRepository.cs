using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class IngredientRepository : Repository<Ingredient, IngredientId>, IIngredientRepository
{
    private readonly OikonoDbContext _dbContext;

    public IngredientRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
