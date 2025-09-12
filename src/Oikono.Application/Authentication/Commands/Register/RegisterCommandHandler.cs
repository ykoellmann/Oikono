using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Users;
using Oikono.Domain.Users.Specifications;
using ErrorOr;
using Errors = Oikono.Domain.Errors.Errors;

namespace Oikono.Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenProvider = jwtTokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command,
        CancellationToken ct)
    {
        //Check if user exists
        if (!await _userRepository.IsEmailUniqueAsync(command.Email, ct))
            return Errors.User.UserWithGivenEmailAlreadyExists;

        //Create user
        var user = new User(command.FirstName, command.LastName, command.Email, command.Password);
        await _userRepository.AddAsync(user, ct);

        user =
            (await _userRepository.GetByIdAsync(
                user.Id,
                ct,
                Specifications.User.IncludeAuthorization))!;

        //Generate token
        var token = _jwtTokenProvider.GenerateToken(user);
        var newRefreshToken =
            await _refreshTokenRepository.AddAsync(new RefreshToken(user.Id), user.Id, ct);

        return new AuthenticationResult(token, newRefreshToken);
    }
}