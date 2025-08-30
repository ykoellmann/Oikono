using Oikono.Domain.Models;

namespace Oikono.Application.Common.Interfaces.MediatR.Requests;

public interface IIdempotentCommand<TResult> : ICommand<TResult>, IIdempotentCommand;

public interface IIdempotentCommand;