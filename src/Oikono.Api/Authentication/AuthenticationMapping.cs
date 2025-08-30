using Oikono.Api.Authentication.Request;
using Oikono.Api.Authentication.Response;
using Oikono.Application.Authentication.Commands.Register;
using Oikono.Application.Authentication.Common;
using Oikono.Application.Authentication.Queries.Login;
using Mapster;

namespace Oikono.Api.Authentication;

internal class AuthenticationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>()
            .MapToConstructor(true);

        config.NewConfig<LoginRequest, LoginQuery>()
            .MapToConstructor(true);

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .MapToConstructor(true);
    }
}