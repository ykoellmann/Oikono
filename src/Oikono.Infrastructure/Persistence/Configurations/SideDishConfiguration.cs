using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class SideDishConfiguration : BaseConfiguration<SideDish, SideDishId>
{
    public override void ConfigureEntity(EntityTypeBuilder<SideDish> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Name)
            .IsUnique();
    }
}
