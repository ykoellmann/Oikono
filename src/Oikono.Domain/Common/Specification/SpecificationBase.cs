using Oikono.Domain.Common.Specification.Include;
using Oikono.Domain.Common.Specification.Order;
using Oikono.Domain.Models;
using Oikono.Domain.Users;

namespace Oikono.Domain.Common.Specification;

public class SpecificationBase<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : Id<TId>, new()
{
    protected virtual bool AsNoTracking => false;
    protected virtual bool AsSplitQuery => false;
    protected virtual bool IgnoreQueryFilters => false;

    protected virtual IIncludableSpecification<TEntity> Include(IIncludableSpecification<TEntity> includable)
    {
        return null;
    }

    protected virtual IOrderedSpecification<TEntity> Order(IOrderedSpecification<TEntity> ordered)
    {
        return null;
    }
}