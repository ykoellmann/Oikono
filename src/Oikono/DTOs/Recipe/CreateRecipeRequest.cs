using Oikono.Entities;

namespace Oikono.DTOs.Recipe;

public record CreateRecipeRequest(
    string Name,
    int Portions,
    int? Calories,
    int? Rating,
    List<Guid> Tags,
    List<Guid> SideDishes,
    List<CreatePartRequest> Parts,
    List<CreateStepRequest> Steps
);

public record CreatePartRequest(
    string Name,
    List<CreatePartIngredientRequest> Ingredients
);

public record CreatePartIngredientRequest(
    Guid IngredientId,
    double Amount,
    UnitType Unit
);

public record CreateStepRequest(
    string Description,
    TimeSpan? Duration,
    int? Temperature,
    Guid? DeviceId
);
