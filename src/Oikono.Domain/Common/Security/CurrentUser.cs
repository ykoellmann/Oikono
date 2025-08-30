using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Common.Security;

public record CurrentUser(
    UserId Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> Roles);