using System.ComponentModel.DataAnnotations;

namespace Oikono.Api.Recipes.Request;

public class RecipeRequest
{
    [Range(1, int.MaxValue)] 
    public int Page { get; init; } = 1;

    [Range(1, 1000)] 
    public int PageSize { get; init; } = 20;

    public string? Search { get; init; }

    // Erlaubte Felder: z. B. "createdAt", "name", "status"
    public string? SortBy { get; init; } = "createdAt";

    // "asc" oder "desc"
    public string SortOrder { get; init; } = "desc";

    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}