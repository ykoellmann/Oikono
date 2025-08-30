using ErrorOr;
using MediatR;

namespace Oikono.Application.Common.Interfaces.MediatR.Requests;

public interface ICommand<TResult> : IRequest<ErrorOr<TResult>>;