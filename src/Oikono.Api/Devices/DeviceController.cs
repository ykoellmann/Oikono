using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Api.Common.Controllers;
using Oikono.Api.Devices.Request;
using Oikono.Api.Devices.Response;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Devices;

[Route("api/[controller]")]
public class DeviceController : Controller<IDeviceRepository, Device, DeviceId, DeviceRequest, DeviceResponse>
{
    public DeviceController(ISender mediator) : base(mediator)
    {
    }
}
