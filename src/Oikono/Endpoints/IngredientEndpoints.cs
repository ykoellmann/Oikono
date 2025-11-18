using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Common;
using Oikono.DTOs.Recipe;
using Oikono.Entities;

namespace Oikono.Endpoints;

public static class IngredientEndpoints
{
    public static void MapIngredientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ingredient")
            .WithTags("Ingredients")
            .RequireAuthorization();

        group.MapGet("", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> GetAllAsync(OikonoDbContext db)
    {
        var ingredients = await db.Ingredients
            .OrderBy(i => i.Name)
            .ToListAsync();

        var response = ingredients.Select(i => new IngredientResponse(i.Id, i.Name)).ToList();
        return Results.Ok(response);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, OikonoDbContext db)
    {
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new IngredientResponse(ingredient.Id, ingredient.Name));
    }

    private static async Task<IResult> CreateAsync(CreateEntityRequest request, OikonoDbContext db)
    {
        if (await db.Ingredients.AnyAsync(i => i.Name == request.Name))
        {
            return Results.BadRequest(new { message = "Ingredient with this name already exists" });
        }

        var ingredient = new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        db.Ingredients.Add(ingredient);
        await db.SaveChangesAsync();

        return Results.Created($"/api/ingredients/{ingredient.Id}", new IngredientResponse(ingredient.Id, ingredient.Name));
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateEntityRequest request, OikonoDbContext db)
    {
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return Results.NotFound();
        }

        if (await db.Ingredients.AnyAsync(i => i.Name == request.Name && i.Id != id))
        {
            return Results.BadRequest(new { message = "Ingredient with this name already exists" });
        }

        ingredient.Name = request.Name;
        await db.SaveChangesAsync();

        return Results.Ok(new IngredientResponse(ingredient.Id, ingredient.Name));
    }

    private static async Task<IResult> DeleteAsync(Guid id, OikonoDbContext db)
    {
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return Results.NotFound();
        }

        db.Ingredients.Remove(ingredient);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
