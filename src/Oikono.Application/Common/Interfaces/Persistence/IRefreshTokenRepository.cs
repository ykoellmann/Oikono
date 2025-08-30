using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository : IRepository<RefreshToken, RefreshTokenId>;