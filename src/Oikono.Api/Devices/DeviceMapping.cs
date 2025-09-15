using Mapster;
using Oikono.Api.Devices.Request;
using Oikono.Api.Devices.Response;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Devices;

internal class DeviceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeviceRequest, Domain.Recipes.Device>()
            .MapToConstructor(true);

        config.NewConfig<Domain.Recipes.Device, DeviceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .MapToConstructor(true);

        config.NewConfig<DeviceRequest, CreateCommand<Domain.Recipes.Device, DeviceId, DeviceRequest, DeviceResponse>>()
            .MapToConstructor(true);

        config.NewConfig<(DeviceId, DeviceRequest), UpdateCommand<Domain.Recipes.Device, DeviceId, DeviceRequest, DeviceResponse>>()
            .MapToConstructor(true);
    }
}
