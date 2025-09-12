using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Idempotencies;
using Oikono.Domain.Idempotencies.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Oikono.Domain.Errors;

namespace Oikono.Application.Common.Behaviours;

public sealed class IdempotentBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommand
    where TResponse : IErrorOr
{
    private readonly IIdempotencyRepository _idempotencyRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserProvider _currentUserProvider;

    public IdempotentBehaviour(IIdempotencyRepository idempotencyRepository, IHttpContextAccessor httpContextAccessor,
        ICurrentUserProvider currentUserProvider)
    {
        _idempotencyRepository = idempotencyRepository;
        _httpContextAccessor = httpContextAccessor;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestId = _httpContextAccessor.HttpContext?.Request.Headers["X-Request-Id"];

        if (string.IsNullOrWhiteSpace(requestId))
            return (dynamic)Errors.Idempotent.RequestIdMissing;

        if (Guid.TryParse(requestId, out var parsedRequestId) == false)
            return (dynamic)Errors.Idempotent.RequestIdInvalid;

        var idempotencyId = new IdempotencyId(parsedRequestId);
        if (await _idempotencyRepository.RequestExistsAsync(idempotencyId, ct))
            return (dynamic)Errors.Idempotent.RequestAlreadyProcessed;

        await _idempotencyRepository.AddAsync(new Idempotency(idempotencyId, typeof(TRequest).Name), null, ct);

        return await next();
    }
}