using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.Entities;

namespace Oikono.Endpoints;

public static class AssetEndpoints
{
    public static void MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/asset")
            .WithTags("Assets")
            .RequireAuthorization();

        group.MapGet("/{id:guid}", GetAssetAsync);
        group.MapPost("/recipe", UploadRecipeAssetAsync)
            .DisableAntiforgery();
    }

    private static async Task<IResult> GetAssetAsync(
        Guid id,
        OikonoDbContext db)
    {
        var asset = await db.RecipeAssets.FindAsync(id);
        if (asset == null)
        {
            return Results.NotFound();
        }

        return Results.File(asset.Data, asset.ContentType, asset.FileName);
    }

    private static async Task<IResult> UploadRecipeAssetAsync(
        HttpRequest request,
        OikonoDbContext db)
    {
        if (!request.HasFormContentType)
        {
            return Results.BadRequest(new { message = "Invalid content type" });
        }

        var form = await request.ReadFormAsync();
        var file = form.Files.GetFile("file");
        var recipeIdString = form["recipeId"].ToString();

        if (file == null || file.Length == 0)
        {
            return Results.BadRequest(new { message = "No file provided" });
        }

        if (!Guid.TryParse(recipeIdString, out var recipeId))
        {
            return Results.BadRequest(new { message = "Invalid recipe ID" });
        }

        // Validate recipe exists
        if (!await db.Recipes.AnyAsync(r => r.Id == recipeId))
        {
            return Results.BadRequest(new { message = "Recipe not found" });
        }

        // Read file data
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var asset = new RecipeAsset
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            ContentType = file.ContentType ?? "application/octet-stream",
            Data = memoryStream.ToArray(),
            RecipeId = recipeId
        };

        db.RecipeAssets.Add(asset);
        await db.SaveChangesAsync();

        return Results.Created($"/api/assets/{asset.Id}", new { id = asset.Id });
    }
}
