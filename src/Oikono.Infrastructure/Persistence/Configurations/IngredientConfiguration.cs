using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class IngredientConfiguration : BaseConfiguration<Ingredient, IngredientId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(i => i.Name)
            .IsUnique();
    }
}
