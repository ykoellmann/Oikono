using ErrorOr;

namespace Oikono.Domain.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error UserWithGivenEmailAlreadyExists =>
            Error.Conflict("User.DuplicateEmail", "User with given Email already exists");

        public static Error UserNotFound =>
            Error.Conflict("User.NotFound", "User not found");
    }
}