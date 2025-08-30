using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : BaseConfiguration<RefreshToken, RefreshTokenId>
{
    public override void ConfigureEntity(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");

        builder.Property(rt => rt.UserId)
            .HasConversion(userId => userId.Value,
                guid => new UserId(guid));
    }
}