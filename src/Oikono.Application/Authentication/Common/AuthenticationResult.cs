using Oikono.Domain.Users;

namespace Oikono.Application.Authentication.Common;

public record AuthenticationResult(
    string Token,
    RefreshToken RefreshToken);