using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Assets;
using Oikono.Domain.Assets.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : BaseConfiguration<Asset, AssetId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Role");

        builder.Property(r => r.FileName)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(r => r.ContentType)
            .HasMaxLength(256);

        builder.Property(r => r.Data)
            .IsRequired()
            .HasColumnType("bytea");
    }
}