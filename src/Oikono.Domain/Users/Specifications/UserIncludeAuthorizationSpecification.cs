using Oikono.Domain.Common.Specification;
using Oikono.Domain.Common.Specification.Include;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users.Specifications;

public partial class UserIncludeAuthorizationSpecification : Specification<User, UserId>
{
    protected override IIncludableSpecification<User> Include(IIncludableSpecification<User> includable)
    {
        return includable
            .Include(u => u.RefreshTokens)
            .Include(u => u.UserPermissions)
            .ThenInclude(up => up.Permission)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.UserPolicies)
            .ThenInclude(up => up.Policy);
    }
}