using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class PartIngredientConfiguration : BaseConfiguration<PartIngredient, PartIngredientId>
{
    public override void ConfigureEntity(EntityTypeBuilder<PartIngredient> builder)
    {
        builder.Property(pi => pi.Amount)
            .IsRequired();

        builder.Property(pi => pi.Unit)
            .HasMaxLength(20);

        builder
            .HasOne(pi => pi.Part)
            .WithMany();
        
        builder
            .HasOne(pi => pi.Ingredient)
            .WithMany();
    }
}
