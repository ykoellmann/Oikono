﻿namespace Oikono.Domain.Common.Specification.Include;

public interface IIncludableSpecification<TEntity>
    where TEntity : class;

public interface IIncludableSpecification<TEntity, TProperty> : IIncludableSpecification<TEntity>
    where TEntity : class;