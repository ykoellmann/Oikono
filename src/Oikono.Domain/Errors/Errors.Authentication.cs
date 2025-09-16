using ErrorOr;

namespace Oikono.Domain.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Unauthorized("Authentication.InvalidCredentials", "Invalid credentials");

        public static Error InvalidRefreshToken =>
            Error.Unauthorized("Authentication.InvalidRefreshToken", "Invalid refresh token");

        public static Error RefreshTokenExpired =>
            Error.Unauthorized("Authentication.RefreshTokenExpired", "Refresh token expired");

        public static Error AccountNotActive =>
            Error.Validation(code: "Authentication.AccountNotActive",
                description: "Ihr Konto ist noch nicht aktiviert.");
    }
}