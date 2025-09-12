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
            .WithMany()
            .UsingEntity<PartIngredient>(
                r => r.HasOne(p => p.Ingredient).WithMany().HasForeignKey(p => p.IngredientId),
                r => r.HasOne(p => p.Part).WithMany().HasForeignKey(p => p.PartId)
            );
    }
}