﻿using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Oikono.Infrastructure.Persistence.Repositories.Users;

public class UserRepository : Repository<User, UserId>, IUserRepository
{
    private readonly OikonoDbContext _dbContext;
    private readonly IPasswordHashProvider _passwordHashProvider;

    public UserRepository(OikonoDbContext dbContext, IPasswordHashProvider passwordHashProvider) : base(dbContext)
    {
        _dbContext = dbContext;
        _passwordHashProvider = passwordHashProvider;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct)
    {
        return await _dbContext.Users.AllAsync(u => u.Email != email, ct);
    }

    public async Task<User> AddAsync(User entity, CancellationToken ct)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.Password = _passwordHashProvider.HashPassword(entity.Password);
        await _dbContext.AddAsync(entity, ct);
        await _dbContext.SaveChangesAsync(ct);

        return entity;
    }

    [Obsolete("This method is replaced by its overload")]
    public override async Task<User> AddAsync(User entity, UserId userId,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}