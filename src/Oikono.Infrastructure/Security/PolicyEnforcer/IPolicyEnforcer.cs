using Oikono.Domain.Common.Security;
using ErrorOr;
using MediatR;

namespace Oikono.Infrastructure.Security.PolicyEnforcer;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IRequest<T> request,
        CurrentUser currentUser,
        string policy);
}