using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Models;

namespace Oikono.Api.Common.Controllers;

[AllowAnonymous]
public class Controller<TIRepository, TEntity, TId, TRequest, TResponse> : ApiController
    where TId : Id<TId>, new()
    where TEntity : Entity<TId>
    where TIRepository : class, IRepository<TEntity, TId>
{
    private readonly ISender _mediator;

    public Controller(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _mediator
            .Send(new GetListQuery<TEntity, TId, TResponse>(), ct);

        return result.Match(
            dtos => Ok(dtos),
            Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var typedId = (TId)Activator.CreateInstance(typeof(TId), id)!;
        var result = await _mediator
            .Send(new GetByIdQuery<TEntity, TId, TResponse>(typedId), ct);

        return result.Match(
            dto => Ok(dto),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TRequest createDto, CancellationToken ct)
    {
        var result = await _mediator
            .Send(new CreateCommand<TEntity, TId, TRequest, TResponse>(createDto, UserId), ct);

        return result.Match(
            dto => Ok(dto),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TRequest updateDto, CancellationToken ct)
    {
        var typedId = (TId)Activator.CreateInstance(typeof(TId), id)!;
        var result = await _mediator
            .Send(new UpdateCommand<TEntity, TId, TRequest, TResponse>(typedId, updateDto, UserId), ct);

        return result.Match(
            _ => Ok(),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var typedId = (TId)Activator.CreateInstance(typeof(TId), id)!;
        var result = await _mediator
            .Send(new DeleteCommand<TEntity, TId>(typedId, UserId), ct);

        return result.Match(
            _ => Ok(),
            Problem);
    }
}