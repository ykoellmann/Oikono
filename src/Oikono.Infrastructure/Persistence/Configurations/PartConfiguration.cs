using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class PartConfiguration : BaseConfiguration<Part, PartId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Part> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(p => p.Recipe)
            .WithMany(r => r.Parts)
            .HasForeignKey(p => p.RecipeId);

        builder.HasMany(p => p.Ingredients)
            .WithOne(i => i.Part)
            .HasForeignKey(i => i.PartId);
    }
}