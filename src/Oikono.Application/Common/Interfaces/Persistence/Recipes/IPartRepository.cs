using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence.Recipes;

public interface IPartRepository : IRepository<Part, PartId>
{
    Task<List<Part>> GetByRecipeIdAsync(RecipeId recipeId, CancellationToken ct);
}
