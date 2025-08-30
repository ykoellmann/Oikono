﻿using Oikono.Domain.Common.Specification;
using Oikono.Domain.Models;

namespace Oikono.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> Specificate<TEntity, TId>(this IQueryable<TEntity> query,
        ISpecification<TEntity, TId>? specification = null)
        where TEntity : Entity<TId>
        where TId : Id<TId>, new()
    {
        return specification is null ? query : specification.Specificate(query);
    }

    public static IQueryable<TDto> Specificate<TEntity, TId, TDto>(this IQueryable<TEntity> query,
        ISpecification<TEntity, TId, TDto> specification)
        where TEntity : Entity<TId>
        where TId : Id<TId>, new()
    {
        return specification.Specificate(query);
    }
}