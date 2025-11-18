using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Common;
using Oikono.DTOs.Recipe;
using Oikono.Entities;

namespace Oikono.Endpoints;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tag")
            .WithTags("Tags")
            .RequireAuthorization();

        group.MapGet("", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> GetAllAsync(OikonoDbContext db)
    {
        var tags = await db.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();

        var response = tags.Select(t => new TagResponse(t.Id, t.Name)).ToList();
        return Results.Ok(response);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, OikonoDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new TagResponse(tag.Id, tag.Name));
    }

    private static async Task<IResult> CreateAsync(CreateEntityRequest request, OikonoDbContext db)
    {
        if (await db.Tags.AnyAsync(t => t.Name == request.Name))
        {
            return Results.BadRequest(new { message = "Tag with this name already exists" });
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        db.Tags.Add(tag);
        await db.SaveChangesAsync();

        return Results.Created($"/api/tags/{tag.Id}", new TagResponse(tag.Id, tag.Name));
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateEntityRequest request, OikonoDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null)
        {
            return Results.NotFound();
        }

        if (await db.Tags.AnyAsync(t => t.Name == request.Name && t.Id != id))
        {
            return Results.BadRequest(new { message = "Tag with this name already exists" });
        }

        tag.Name = request.Name;
        await db.SaveChangesAsync();

        return Results.Ok(new TagResponse(tag.Id, tag.Name));
    }

    private static async Task<IResult> DeleteAsync(Guid id, OikonoDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null)
        {
            return Results.NotFound();
        }

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
