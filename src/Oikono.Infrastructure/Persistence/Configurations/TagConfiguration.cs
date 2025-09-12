using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class TagConfiguration : BaseConfiguration<Tag, TagId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Name)
            .IsUnique();
    }
}
