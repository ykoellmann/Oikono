using Oikono.Application.Authentication.Common;
using Oikono.Application.Common.Interfaces.MediatR.Requests;

namespace Oikono.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IQuery<AuthenticationResult>;