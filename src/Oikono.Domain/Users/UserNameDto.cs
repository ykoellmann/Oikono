using Oikono.Domain.Models;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users;

public record UserNameDto(UserId Id, string FirstName, string LastName) : IDto<UserId>;