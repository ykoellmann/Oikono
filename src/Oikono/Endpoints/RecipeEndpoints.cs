using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Common;
using Oikono.DTOs.Recipe;
using Oikono.Entities;
using Oikono.Services;

namespace Oikono.Endpoints;

public static class RecipeEndpoints
{
    public static void MapRecipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/recipes")
            .WithTags("Recipes")
            .RequireAuthorization();

        group.MapGet("", GetRecipesAsync);
        group.MapGet("/{id:guid}", GetRecipeByIdAsync);
        group.MapPost("", CreateRecipeAsync);
        group.MapGet("/units", GetUnitsAsync);
    }

    private static async Task<IResult> GetRecipesAsync(
        OikonoDbContext db,
        int page = 1,
        int pageSize = 20,
        string? search = null)
    {
        var query = db.Recipes
            .Include(r => r.RecipeTags).ThenInclude(rt => rt.Tag)
            .Include(r => r.RecipeSideDishes).ThenInclude(rs => rs.SideDish)
            .Include(r => r.RecipeAssets)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Name.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var recipes = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = recipes.Select(r => new RecipeResponse(
            r.Id,
            r.Name,
            r.Portions,
            r.Calories,
            r.Rating,
            r.RecipeTags.Select(rt => new TagResponse(rt.TagId, rt.Tag.Name)).ToList(),
            r.RecipeSideDishes.Select(rs => new SideDishResponse(rs.SideDishId, rs.SideDish.Name)).ToList(),
            r.RecipeAssets.Select(a => new RecipeAssetResponse(a.Id, a.FileName, a.ContentType)).ToList(),
            r.CreatedAt
        )).ToList();

        return Results.Ok(new PagedResponse<RecipeResponse>(response, totalCount, page, pageSize));
    }

    private static async Task<IResult> GetRecipeByIdAsync(
        Guid id,
        OikonoDbContext db)
    {
        var recipe = await db.Recipes
            .Include(r => r.Parts).ThenInclude(p => p.PartIngredients).ThenInclude(pi => pi.Ingredient)
            .Include(r => r.Steps).ThenInclude(s => s.Device)
            .Include(r => r.RecipeTags).ThenInclude(rt => rt.Tag)
            .Include(r => r.RecipeSideDishes).ThenInclude(rs => rs.SideDish)
            .Include(r => r.RecipeAssets)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return Results.NotFound();
        }

        var response = new RecipeDetailResponse(
            recipe.Id,
            recipe.Name,
            recipe.Portions,
            recipe.Calories,
            recipe.Rating,
            recipe.Parts.Select(p => new PartResponse(
                p.Id,
                p.Name,
                p.PartIngredients.Select(pi => new PartIngredientResponse(
                    pi.Id,
                    new IngredientResponse(pi.Ingredient.Id, pi.Ingredient.Name),
                    pi.Amount,
                    pi.Unit
                )).ToList()
            )).ToList(),
            recipe.Steps.Select(s => new StepResponse(
                s.Id,
                s.Description,
                s.Duration,
                s.Temperature,
                s.Device != null ? new DeviceResponse(s.Device.Id, s.Device.Name) : null
            )).ToList(),
            recipe.RecipeTags.Select(rt => new TagResponse(rt.TagId, rt.Tag.Name)).ToList(),
            recipe.RecipeSideDishes.Select(rs => new SideDishResponse(rs.SideDishId, rs.SideDish.Name)).ToList(),
            recipe.RecipeAssets.Select(a => new RecipeAssetResponse(a.Id, a.FileName, a.ContentType)).ToList(),
            recipe.CreatedAt,
            recipe.UpdatedAt
        );

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateRecipeAsync(
        CreateRecipeRequest request,
        OikonoDbContext db,
        CurrentUserService currentUserService)
    {
        var userId = currentUserService.GetUserId();
        if (userId == null)
        {
            return Results.Unauthorized();
        }

        // Validate tags exist
        var tagIds = request.Tags.Distinct().ToList();
        var existingTags = await db.Tags.Where(t => tagIds.Contains(t.Id)).Select(t => t.Id).ToListAsync();
        if (existingTags.Count != tagIds.Count)
        {
            return Results.BadRequest(new { message = "One or more tags do not exist" });
        }

        // Validate side dishes exist
        var sideDishIds = request.SideDishes.Distinct().ToList();
        var existingSideDishes = await db.SideDishes.Where(s => sideDishIds.Contains(s.Id)).Select(s => s.Id).ToListAsync();
        if (existingSideDishes.Count != sideDishIds.Count)
        {
            return Results.BadRequest(new { message = "One or more side dishes do not exist" });
        }

        // Validate ingredients exist
        var ingredientIds = request.Parts.SelectMany(p => p.Ingredients.Select(i => i.IngredientId)).Distinct().ToList();
        var existingIngredients = await db.Ingredients.Where(i => ingredientIds.Contains(i.Id)).Select(i => i.Id).ToListAsync();
        if (existingIngredients.Count != ingredientIds.Count)
        {
            return Results.BadRequest(new { message = "One or more ingredients do not exist" });
        }

        // Validate devices exist
        var deviceIds = request.Steps.Where(s => s.DeviceId.HasValue).Select(s => s.DeviceId!.Value).Distinct().ToList();
        var existingDevices = await db.Devices.Where(d => deviceIds.Contains(d.Id)).Select(d => d.Id).ToListAsync();
        if (existingDevices.Count != deviceIds.Count)
        {
            return Results.BadRequest(new { message = "One or more devices do not exist" });
        }

        // Create recipe
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Portions = request.Portions,
            Calories = request.Calories,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = userId.Value
        };

        // Add parts and ingredients
        foreach (var partRequest in request.Parts)
        {
            var part = new Part
            {
                Id = Guid.NewGuid(),
                Name = partRequest.Name,
                RecipeId = recipe.Id
            };

            foreach (var ingredientRequest in partRequest.Ingredients)
            {
                part.PartIngredients.Add(new PartIngredient
                {
                    Id = Guid.NewGuid(),
                    PartId = part.Id,
                    IngredientId = ingredientRequest.IngredientId,
                    Amount = ingredientRequest.Amount,
                    Unit = ingredientRequest.Unit
                });
            }

            recipe.Parts.Add(part);
        }

        // Add steps
        foreach (var stepRequest in request.Steps)
        {
            recipe.Steps.Add(new Step
            {
                Id = Guid.NewGuid(),
                Description = stepRequest.Description,
                Duration = stepRequest.Duration,
                Temperature = stepRequest.Temperature,
                DeviceId = stepRequest.DeviceId,
                RecipeId = recipe.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Add tags
        foreach (var tagId in tagIds)
        {
            recipe.RecipeTags.Add(new RecipeTag
            {
                RecipeId = recipe.Id,
                TagId = tagId
            });
        }

        // Add side dishes
        foreach (var sideDishId in sideDishIds)
        {
            recipe.RecipeSideDishes.Add(new RecipeSideDish
            {
                RecipeId = recipe.Id,
                SideDishId = sideDishId
            });
        }

        db.Recipes.Add(recipe);
        await db.SaveChangesAsync();

        return Results.Created($"/api/recipes/{recipe.Id}", new { id = recipe.Id });
    }

    private static IResult GetUnitsAsync()
    {
        var units = Enum.GetValues<UnitType>()
            .Select(u => new UnitResponse(u.ToString(), (int)u))
            .ToList();

        return Results.Ok(units);
    }
}
