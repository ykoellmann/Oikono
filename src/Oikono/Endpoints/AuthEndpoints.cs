using Microsoft.EntityFrameworkCore;
using Oikono.Data;
using Oikono.DTOs.Auth;
using Oikono.Entities;
using Oikono.Services;
using BCrypt.Net;

namespace Oikono.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/authentication").WithTags("Authentication");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/refresh", RefreshTokenAsync);
    }

    private static async Task<IResult> RegisterAsync(
        RegisterRequest request,
        OikonoDbContext db)
    {
        // Check if user already exists
        if (await db.Users.AnyAsync(u => u.Email == request.Email))
        {
            return Results.BadRequest(new { message = "User with this email already exists" });
        }

        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Results.Created($"/api/users/{user.Id}", new { message = "User created successfully" });
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        OikonoDbContext db,
        JwtService jwtService,
        HttpContext httpContext)
    {
        // Find user
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        // Generate tokens
        var token = jwtService.GenerateToken(user);
        var refreshTokenString = jwtService.GenerateRefreshToken();

        // Save refresh token
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenString,
            Expires = DateTime.UtcNow.AddDays(7),
            Disabled = false,
            UserId = user.Id
        };

        db.RefreshTokens.Add(refreshToken);
        await db.SaveChangesAsync();

        // Set refresh token in HTTP-only cookie
        httpContext.Response.Cookies.Append("refreshToken", refreshTokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = refreshToken.Expires
        });

        return Results.Ok(new AuthResponse(token, refreshTokenString, DateTime.UtcNow.AddHours(1)));
    }

    private static async Task<IResult> RefreshTokenAsync(
        OikonoDbContext db,
        JwtService jwtService,
        HttpContext httpContext)
    {
        // Get refresh token from cookie
        if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenString))
        {
            return Results.Unauthorized();
        }

        // Find refresh token
        var refreshToken = await db.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenString);

        if (refreshToken == null || refreshToken.Disabled || refreshToken.Expires < DateTime.UtcNow)
        {
            return Results.Unauthorized();
        }

        // Generate new tokens
        var token = jwtService.GenerateToken(refreshToken.User);
        var newRefreshTokenString = jwtService.GenerateRefreshToken();

        // Disable old refresh token
        refreshToken.Disabled = true;

        // Save new refresh token
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshTokenString,
            Expires = DateTime.UtcNow.AddDays(7),
            Disabled = false,
            UserId = refreshToken.UserId
        };

        db.RefreshTokens.Add(newRefreshToken);
        await db.SaveChangesAsync();

        // Set new refresh token in HTTP-only cookie
        httpContext.Response.Cookies.Append("refreshToken", newRefreshTokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = newRefreshToken.Expires
        });

        return Results.Ok(new AuthResponse(token, newRefreshTokenString, DateTime.UtcNow.AddHours(1)));
    }
}
