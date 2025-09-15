using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oikono.Application.Common.MediatR;

namespace Oikono.Api.Common.Controllers;

public class Controller<TEntity, TId, TRequest, TResponse> : ApiController
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
            .Send(new GetListQuery<TEntity, TResponse>(), ct);

        return result.Match(
            dtos => Ok(dtos),
            Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await _mediator
            .Send(new GetByIdQuery<TEntity, Guid, TResponse>(id), ct);

        return result.Match(
            dto => Ok(dto),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TRequest createDto, CancellationToken ct)
    {
        var result = await _mediator
            .Send(new CreateCommand<TEntity, TRequest, TResponse>(createDto, UserId), ct);

        return result.Match(
            dto => Ok(dto),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TRequest updateDto, CancellationToken ct)
    {
        var result = await _mediator
            .Send(new UpdateCommand<TEntity, Guid, TRequest, TResponse>(id, updateDto, UserId), ct);

        return result.Match(
            _ => Ok(),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator
            .Send(new DeleteCommand<TEntity, Guid>(id, UserId), ct);

        return result.Match(
            _ => Ok(),
            Problem);
    }
}