using System.Linq.Expressions;
using Oikono.Domain.Common.Specification;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users.Specifications;

public partial class UserNameDtoSpecification : Specification<User, UserId, UserNameDto>
{
    protected override Expression<Func<User, UserNameDto>> Map()
    {
        return user => new UserNameDto(user.Id, user.FirstName, user.LastName);
    }
}