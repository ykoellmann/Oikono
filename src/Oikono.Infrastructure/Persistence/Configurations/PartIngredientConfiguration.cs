using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class PartIngredientConfiguration : BaseConfiguration<PartIngredient, PartIngredientId>
{
    public override void ConfigureEntity(EntityTypeBuilder<PartIngredient> builder)
    {
        builder.Property(pi => pi.PartId)
            .HasConversion(
                id => id.Value,
                value => new PartId(value));

        builder.Property(pi => pi.IngredientId)
            .HasConversion(
                id => id.Value,
                value => new IngredientId(value));

        builder.Property(pi => pi.Amount)
            .IsRequired();

        builder.Property(pi => pi.Unit)
            .HasMaxLength(20);

        builder.HasOne(pi => pi.Part)
            .WithMany()
            .HasForeignKey(pi => pi.PartId);

        builder.HasOne(pi => pi.Ingredient)
            .WithMany()
            .HasForeignKey(pi => pi.IngredientId);
    }
}
