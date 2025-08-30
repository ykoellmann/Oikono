using Oikono.Domain.Users;

namespace Oikono.Application.Common.Interfaces.Security;

public interface IJwtTokenProvider
{
    string GenerateToken(User user);
}