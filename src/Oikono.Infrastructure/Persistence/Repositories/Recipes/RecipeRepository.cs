using Microsoft.EntityFrameworkCore;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Application.Common.Pagination;
using Oikono.Application.Recipes.Queries.Get;
using Oikono.Domain.Common.Specification;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;
using Oikono.Infrastructure.Extensions;

namespace Oikono.Infrastructure.Persistence.Repositories.Recipes;

public class RecipeRepository : Repository<Recipe, RecipeId>, IRecipeRepository
{
    private readonly OikonoDbContext _dbContext;

    public RecipeRepository(OikonoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PagedResult<Recipe>> GetFilteredListAsync(CancellationToken ct,
        GetRecipesQuery filter)
    {
        var query = _dbContext.Set<Recipe>()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{filter.Search}%";
            query = query.Where(r => EF.Functions.ILike(r.Name, pattern));
        }

        if (filter.DateFrom.HasValue)
            query = query.Where(r => r.CreatedAt >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(r => r.CreatedAt <= filter.DateTo.Value);

        // Sortierung
        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            var prop = filter.SortBy;
            var asc  = filter.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
            if (asc)
                query = query.OrderBy(r => EF.Property<object>(r, prop));
            else
                query = query.OrderByDescending(r => EF.Property<object>(r, prop));
        }
        else
        {
            // Default-Sort
            query = query.OrderBy(r => r.Name);
        }

        // Paging
        var skip = (Math.Max(filter.Page, 1) - 1) * filter.PageSize;
        query = query.Skip(skip).Take(filter.PageSize);

        query = query.Include(recipe => recipe.Tags)
            .Include(recipe => recipe.Images);
        
        var totalCount = await query.CountAsync(ct);

        var data = await query.ToListAsync(ct);
        
        return new PagedResult<Recipe>(data, totalCount, filter.Page, filter.PageSize);
    }
}