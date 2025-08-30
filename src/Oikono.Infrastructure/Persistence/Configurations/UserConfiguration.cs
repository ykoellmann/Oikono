﻿using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration<User, UserId>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnOrder(0)
            .HasConversion(id => id.Value,
                value => new UserId(value))
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .HasColumnOrder(102)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .HasColumnOrder(104)
            .IsRequired();

        ConfigureEntity(builder);
    }

    public override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.BirthDate)
            .HasConversion(
                date => date.ToDateTime(new TimeOnly()),
                value => new DateOnly(value.Year, value.Month, value.Day))
            .IsRequired();

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId);
    }
}