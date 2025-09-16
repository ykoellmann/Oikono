using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Users;
using Oikono.Domain.Users.Specifications;
using ErrorOr;
using Errors = Oikono.Domain.Errors.Errors;

namespace Oikono.Application.Authentication.Commands.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthenticationResult>
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository,
        IJwtTokenProvider jwtTokenProvider)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request,
        CancellationToken ct)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(request.TokenToRefresh, ct);

        if (token is not { Expired: false })
            return Errors.Authentication.RefreshTokenExpired;

        //Get user with userId and check if given refresh token is users last refresh token. Only one can be valid for one user at a time.
        var user = await _userRepository.GetByIdAsync(token.UserId, ct,
            Specifications.User.IncludeAuthorization);

        if (user is null)
            return Errors.User.UserNotFound;

        if (!user.HasActiveRefreshToken)
            return Errors.Authentication.RefreshTokenExpired;

        if (user.ActiveRefreshToken!.Token != request.TokenToRefresh)
            return Errors.Authentication.InvalidRefreshToken;

        var newRefreshToken =
            await _refreshTokenRepository.AddAsync(new RefreshToken(user.Id), user.Id, ct);
        var jwtToken = _jwtTokenProvider.GenerateToken(user);

        return new AuthenticationResult(jwtToken, newRefreshToken);
    }
}