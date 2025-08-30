using Oikono.Domain.Common.Security;

namespace Oikono.Application.Common.Interfaces.Security;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}