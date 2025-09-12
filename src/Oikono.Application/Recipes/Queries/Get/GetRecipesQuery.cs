using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Application.Common.Pagination;
using Oikono.Application.Recipes.Common;

namespace Oikono.Application.Recipes.Queries.Get;

public record GetRecipesQuery(
    int Page,
    int PageSize,
    string? Search,
    string? SortBy,
    string SortOrder,
    DateTime? DateFrom,
    DateTime? DateTo
) : IQuery<PagedResult<RecipeResult>>;