using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Application.Recipes.Common;
using Oikono.Domain.Recipes;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Application.Recipes.Commands;

public class CreateRecipeCommand : ICommand<RecipeDetailResult>
{
    public string Name { get; set; }
    public int Portions { get; set; }
    public int? Calories { get; set; }
    public int? Rating { get; set; }
    public List<Guid> Tags { get; set; }
    public List<Guid> SideDishes { get; set; }
    public List<CreateRecipePart> Parts { get; set; }
    public List<CreateRecipeStep> Steps { get; set; }
    public UserId UserId { get; set; }

    public class CreateRecipeStep
    {
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int? Temperature { get; set; }
        public Guid DeviceId { get; set; }
    }

    public class CreateRecipePart
    {
        public string Name { get; set; }
        public List<CreateRecipeIngredient> Ingredients { get; set; }

        public class CreateRecipeIngredient
        {
            public Guid IngredientId { get; set; }
            public double Amount { get; set; }
            public UnitType Unit { get; set; }
        }
    }
}