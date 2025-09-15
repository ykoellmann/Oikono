using Mapster;
using Oikono.Api.SideDishes.Request;
using Oikono.Api.SideDishes.Response;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.SideDishes;

internal class SideDishMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SideDishRequest, Domain.Recipes.SideDish>()
            .MapToConstructor(true);

        config.NewConfig<Domain.Recipes.SideDish, SideDishResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .MapToConstructor(true);

        config.NewConfig<SideDishRequest, CreateCommand<Domain.Recipes.SideDish, SideDishId, SideDishRequest, SideDishResponse>>()
            .MapToConstructor(true);

        config.NewConfig<(SideDishId, SideDishRequest), UpdateCommand<Domain.Recipes.SideDish, SideDishId, SideDishRequest, SideDishResponse>>()
            .MapToConstructor(true);
    }
}
