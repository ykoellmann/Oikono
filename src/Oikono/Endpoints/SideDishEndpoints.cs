using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Common;
using Oikono.DTOs.Recipe;
using Oikono.Entities;

namespace Oikono.Endpoints;

public static class SideDishEndpoints
{
    public static void MapSideDishEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sidedish")
            .WithTags("SideDishes")
            .RequireAuthorization();

        group.MapGet("", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> GetAllAsync(OikonoDbContext db)
    {
        var sideDishes = await db.SideDishes
            .OrderBy(s => s.Name)
            .ToListAsync();

        var response = sideDishes.Select(s => new SideDishResponse(s.Id, s.Name)).ToList();
        return Results.Ok(response);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, OikonoDbContext db)
    {
        var sideDish = await db.SideDishes.FindAsync(id);
        if (sideDish == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new SideDishResponse(sideDish.Id, sideDish.Name));
    }

    private static async Task<IResult> CreateAsync(CreateEntityRequest request, OikonoDbContext db)
    {
        if (await db.SideDishes.AnyAsync(s => s.Name == request.Name))
        {
            return Results.BadRequest(new { message = "Side dish with this name already exists" });
        }

        var sideDish = new SideDish
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        db.SideDishes.Add(sideDish);
        await db.SaveChangesAsync();

        return Results.Created($"/api/sidedishes/{sideDish.Id}", new SideDishResponse(sideDish.Id, sideDish.Name));
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateEntityRequest request, OikonoDbContext db)
    {
        var sideDish = await db.SideDishes.FindAsync(id);
        if (sideDish == null)
        {
            return Results.NotFound();
        }

        if (await db.SideDishes.AnyAsync(s => s.Name == request.Name && s.Id != id))
        {
            return Results.BadRequest(new { message = "Side dish with this name already exists" });
        }

        sideDish.Name = request.Name;
        await db.SaveChangesAsync();

        return Results.Ok(new SideDishResponse(sideDish.Id, sideDish.Name));
    }

    private static async Task<IResult> DeleteAsync(Guid id, OikonoDbContext db)
    {
        var sideDish = await db.SideDishes.FindAsync(id);
        if (sideDish == null)
        {
            return Results.NotFound();
        }

        db.SideDishes.Remove(sideDish);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
