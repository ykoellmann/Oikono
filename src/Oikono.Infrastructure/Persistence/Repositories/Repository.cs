﻿using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Common.Specification;
using Oikono.Domain.Models;
using Oikono.Domain.Users.ValueObjects;
using Oikono.Infrastructure.Extensions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Oikono.Infrastructure.Persistence.Repositories;

public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : Id<TId>, new()
{
    private readonly OikonoDbContext _dbContext;

    protected Repository(OikonoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken ct,
        Specification<TEntity, TId>? specification = null)
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .ToListAsync(ct);
    }

    public virtual async Task<List<TDto>> GetListAsync<TDto>(CancellationToken ct,
        Specification<TEntity, TId, TDto> specification)
        where TDto : IDto<TId>
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .ToListAsync(ct);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct,
        Specification<TEntity, TId>? specification = null)
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public virtual async Task<TDto?> GetByIdAsync<TDto>(TId id, CancellationToken ct,
        Specification<TEntity, TId, TDto> specification)
        where TDto : IDto<TId>
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, UserId userId, CancellationToken ct)
    {
        entity.CreatedBy = userId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.AddAsync(entity, ct);
        await _dbContext.SaveChangesAsync(ct);

        return await _dbContext.Set<TEntity>()
            .SingleAsync(e => e.Id == entity.Id, ct);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, UserId userId, CancellationToken ct)
    {
        entity.AddDomainEvent(new ClearCacheEvent<TEntity, TId>(entity));

        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = entity.CreatedBy;

        await _dbContext.SaveChangesAsync(ct);

        return await _dbContext.Set<TEntity>()
            .SingleAsync(e => e.Id == entity.Id, ct);
    }

    public virtual async Task<Deleted> DeleteAsync(TId id, CancellationToken ct)
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync([id], ct);
        entity!.AddDomainEvent(new ClearCacheEvent<TEntity, TId>(entity));

        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(ct);

        return new Deleted();
    }


    [Obsolete("This method is only available in the cached repository")]
    public Task ClearCacheAsync<TChanged>(TChanged changedEvent) where TChanged : ClearCacheEvent<TEntity, TId>
    {
        throw new NotImplementedException("This method is only available in the cached repository");
    }
}