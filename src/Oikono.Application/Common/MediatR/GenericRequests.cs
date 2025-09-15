using System.Windows.Input;
using ErrorOr;
using MediatR;
using Oikono.Application.Common.Interfaces.MediatR.Requests;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Application.Common.MediatR
{
    public record Command<TResult>(UserId UserId) : ICommand<TResult>;

    public record IdCommand<TId, TResult>(TId Id, UserId UserId) : Command<TResult>(UserId);

    public record GetListQuery<TEntity, TResult>() : IQuery<IEnumerable<TResult>>;

    public record GetByIdQuery<TEntity, TId, TResult>(TId Id) : IQuery<TResult>;

    public record CreateCommand<TEntity, TRequest, TResult>(TRequest Request, UserId UserId) : Command<TResult>(UserId);

    public record UpdateCommand<TEntity, TId, TRequest, TResult>(TId Id, TRequest Request, UserId UserId)
        : IdCommand<TId, TResult>(Id, UserId);

    public record DeleteCommand<TEntity, TId>(TId Id, UserId UserId) : IdCommand<TId, Deleted>(Id, UserId);
}