namespace Oikono.DTOs.Auth;

public record AuthResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt
);
