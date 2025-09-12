using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class Step : Entity<StepId>
{
    public Step(RecipeId recipeId, string description, TimeSpan? duration = null, DeviceId? deviceId = null,
        int? temperature = null)
    {
        RecipeId = recipeId;
        Description = description;
        Duration = duration;
        DeviceId = deviceId;
        Temperature = temperature;
    }

    public RecipeId RecipeId { get; private set; }
    public Recipe Recipe { get; private set; } = null!;

    public string Description { get; private set; } = null!;
    public TimeSpan? Duration { get; private set; }
    public int? Temperature { get; private set; } // °C

    public DeviceId? DeviceId { get; private set; }
    public Device? Device { get; private set; }
}