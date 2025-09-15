using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Api.Common.Controllers;
using Oikono.Api.Tags.Request;
using Oikono.Api.Tags.Response;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Tags;

[Route("api/[controller]")]
public class TagController : Controller<ITagRepository, Tag, TagId, TagRequest, TagResponse>
{
    public TagController(ISender mediator) : base(mediator)
    {
    }
}
