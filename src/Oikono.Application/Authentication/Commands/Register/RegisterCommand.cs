using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Requests;

namespace Oikono.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IIdempotentCommand<AuthenticationResult>;