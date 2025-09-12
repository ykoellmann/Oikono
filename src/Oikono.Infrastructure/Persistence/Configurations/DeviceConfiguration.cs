using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class DeviceConfiguration : BaseConfiguration<Device, DeviceId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Device> builder)
    {
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(d => d.Name)
            .IsUnique();
    }
}
