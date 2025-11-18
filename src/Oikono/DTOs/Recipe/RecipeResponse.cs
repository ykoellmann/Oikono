using Oikono.Entities;

namespace Oikono.DTOs.Recipe;

public record RecipeResponse(
    Guid Id,
    string Name,
    int Portions,
    int? Calories,
    int? Rating,
    List<TagResponse> Tags,
    List<SideDishResponse> SideDishes,
    List<RecipeAssetResponse> Images,
    DateTime CreatedAt
);

public record RecipeDetailResponse(
    Guid Id,
    string Name,
    int Portions,
    int? Calories,
    int? Rating,
    List<PartResponse> Parts,
    List<StepResponse> Steps,
    List<TagResponse> Tags,
    List<SideDishResponse> SideDishes,
    List<RecipeAssetResponse> Images,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record PartResponse(
    Guid Id,
    string Name,
    List<PartIngredientResponse> Ingredients
);

public record PartIngredientResponse(
    Guid Id,
    IngredientResponse Ingredient,
    double Amount,
    UnitType Unit
);

public record StepResponse(
    Guid Id,
    string Description,
    TimeSpan? Duration,
    int? Temperature,
    DeviceResponse? Device
);

public record IngredientResponse(
    Guid Id,
    string Name
);

public record DeviceResponse(
    Guid Id,
    string Name
);

public record TagResponse(
    Guid Id,
    string Name
);

public record SideDishResponse(
    Guid Id,
    string Name
);

public record RecipeAssetResponse(
    Guid Id,
    string FileName,
    string ContentType
);
