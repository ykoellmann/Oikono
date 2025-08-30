﻿using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : BaseConfiguration<Role, RoleId>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnOrder(0)
            .HasConversion(id => id.Value,
                value => new RoleId(value))
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .HasColumnOrder(102)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .HasColumnOrder(104)
            .IsRequired();

        ConfigureEntity(builder);
    }

    public override void ConfigureEntity(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(128);
    }
}