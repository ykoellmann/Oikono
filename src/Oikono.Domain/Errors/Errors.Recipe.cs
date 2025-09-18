using ErrorOr;

namespace Oikono.Domain.Errors;

public partial class Errors
{
    public static class Recipe
    {
        public static Error NotFound => Error.NotFound("Recipe.NotFound");
        public static Error Creation => Error.Unexpected("Recipe.Creation", "Failed to create recipe");
    }
}