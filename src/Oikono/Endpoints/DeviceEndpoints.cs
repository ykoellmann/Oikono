using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Common;
using Oikono.DTOs.Recipe;
using Oikono.Entities;

namespace Oikono.Endpoints;

public static class DeviceEndpoints
{
    public static void MapDeviceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/device")
            .WithTags("Devices")
            .RequireAuthorization();

        group.MapGet("", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> GetAllAsync(OikonoDbContext db)
    {
        var devices = await db.Devices
            .OrderBy(d => d.Name)
            .ToListAsync();

        var response = devices.Select(d => new DeviceResponse(d.Id, d.Name)).ToList();
        return Results.Ok(response);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, OikonoDbContext db)
    {
        var device = await db.Devices.FindAsync(id);
        if (device == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new DeviceResponse(device.Id, device.Name));
    }

    private static async Task<IResult> CreateAsync(CreateEntityRequest request, OikonoDbContext db)
    {
        if (await db.Devices.AnyAsync(d => d.Name == request.Name))
        {
            return Results.BadRequest(new { message = "Device with this name already exists" });
        }

        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        db.Devices.Add(device);
        await db.SaveChangesAsync();

        return Results.Created($"/api/devices/{device.Id}", new DeviceResponse(device.Id, device.Name));
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateEntityRequest request, OikonoDbContext db)
    {
        var device = await db.Devices.FindAsync(id);
        if (device == null)
        {
            return Results.NotFound();
        }

        if (await db.Devices.AnyAsync(d => d.Name == request.Name && d.Id != id))
        {
            return Results.BadRequest(new { message = "Device with this name already exists" });
        }

        device.Name = request.Name;
        await db.SaveChangesAsync();

        return Results.Ok(new DeviceResponse(device.Id, device.Name));
    }

    private static async Task<IResult> DeleteAsync(Guid id, OikonoDbContext db)
    {
        var device = await db.Devices.FindAsync(id);
        if (device == null)
        {
            return Results.NotFound();
        }

        db.Devices.Remove(device);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
