using Oikono.Domain.Models;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Domain.Recipes;

public class PartIngredient : Entity<PartIngredientId>
{
    public PartIngredient(PartId partId, IngredientId ingredientId, double amount, UnitType unit)
    {
        PartId = partId;
        IngredientId = ingredientId;
        Amount = amount;
        Unit = unit;
    }

    public PartId PartId { get; private set; }
    public Part Part { get; private set; } = null!;

    public IngredientId IngredientId { get; private set; }
    public Ingredient Ingredient { get; private set; } = null!;

    public double Amount { get; private set; }
    public UnitType Unit { get; private set; }
}