using Oikono.Domain.Recipes;

namespace Oikono.Api.Recipes.Request;

public class CreateRecipeRequest
{
    public string Name { get; set; }
    public int Portions { get; set; }
    public int? Calories { get; set; }
    public int? Rating { get; set; }
    public List<Guid> Tags { get; set; }
    public List<Guid> SideDishes { get; set; }
    public List<CreateRecipePartRequest> Parts { get; set; }
    public List<CreateRecipeStepRequest> Steps { get; set; }

    public class CreateRecipeStepRequest
    {
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int? Temperature { get; set; }
        public Guid DeviceId { get; set; }
    }

    public class CreateRecipePartRequest
    {
        public string Name { get; set; }
        public List<CreateRecipeIngredientRequest> Ingredients { get; set; }

        public class CreateRecipeIngredientRequest
        {
            public Guid IngredientId { get; set; }
            public double Amount { get; set; }
            public UnitType Unit { get; set; }
        }
    }
}