using ErrorOr;
using MediatR;

namespace Oikono.Application.Common.Interfaces.MediatR.Requests;

public interface IQuery<TResult> : IRequest<ErrorOr<TResult>>;