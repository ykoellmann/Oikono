using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Users;
using Oikono.Domain.Users.Specifications;
using ErrorOr;
using Errors = Oikono.Domain.Errors.Errors;

namespace Oikono.Application.Authentication.Queries.Login;

internal class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashProvider _passwordHashProvider;

    public LoginQueryHandler(IJwtTokenProvider jwtTokenProvider, IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository, IPasswordHashProvider passwordHashProvider)
    {
        _jwtTokenProvider = jwtTokenProvider;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHashProvider = passwordHashProvider;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken ct)
    {
        if (await _userRepository.GetByEmailAsync(query.Email, ct) is not User user)
            return Errors.Authentication.InvalidCredentials;

        if (!_passwordHashProvider.VerifyPassword(query.Password, user.Password))
            return Errors.Authentication.InvalidCredentials;

        user = (await _userRepository.GetByIdAsync(user.Id, ct, Specifications.User.IncludeAuthorization))!;

        // NEU: Check auf Aktivität
        if (!user.Active)
            return Errors.Authentication.AccountNotActive;

        var token = _jwtTokenProvider.GenerateToken(user);
        var newRefreshToken =
            await _refreshTokenRepository.AddAsync(new RefreshToken(user.Id), user.Id, ct);

        return new AuthenticationResult(token, newRefreshToken);
    }
}