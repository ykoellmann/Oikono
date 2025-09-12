namespace Oikono.Application.Recipes.Common;

public record RecipeResult(
    string Name,
    int Portions,
    int? Calories,
    int? Rating,
    List<TagResult> Tags
);