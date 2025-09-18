using Microsoft.EntityFrameworkCore;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class PartRepository : Repository<Part, PartId>, IPartRepository
{
    private readonly OikonoDbContext _dbContext;

    public PartRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Part>> GetByRecipeIdAsync(RecipeId recipeId, CancellationToken ct)
    {
        return await _dbContext.Parts
            .Where(p => p.RecipeId == recipeId)
            .Include(p => p.Ingredients)
            .ToListAsync(ct);
    }
}
