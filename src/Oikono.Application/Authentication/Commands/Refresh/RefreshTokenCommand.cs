using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Application.Authentication.Commands.Refresh;

public record RefreshTokenCommand(string TokenToRefresh, UserId UserId) : ICommand<AuthenticationResult>;