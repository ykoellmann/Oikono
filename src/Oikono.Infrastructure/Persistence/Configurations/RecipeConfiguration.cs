using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Assets;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RecipeConfiguration : BaseConfiguration<Recipe, RecipeId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Portions).IsRequired();

        builder
            .HasMany(r => r.Images)
            .WithMany() // kein FK zurück auf Recipe
            .UsingEntity<RecipeAsset>(
                r => r.HasOne(ra => ra.Asset).WithMany().HasForeignKey(ra => ra.AssetId),
                l => l.HasOne(ra => ra.Recipe).WithMany().HasForeignKey(ra => ra.RecipeId));

        builder
            .HasMany(r => r.Tags)
            .WithMany(t => t.Recipes)
            .UsingEntity<RecipeTag>(
                r => r.HasOne(rt => rt.Tag).WithMany().HasForeignKey(rt => rt.TagId),
                l => l.HasOne(rt => rt.Recipe).WithMany().HasForeignKey(ra => ra.RecipeId));

        builder
            .HasMany(r => r.Parts)
            .WithOne(p => p.Recipe);

        builder
            .HasMany(r => r.Steps)
            .WithOne(s => s.Recipe);

        builder
            .HasMany(r => r.SideDishes)
            .WithMany(sd => sd.Recipes) // kein FK zurück auf Recipe
            .UsingEntity<RecipeSideDish>(
                r => r.HasOne(ra => ra.SideDish).WithMany().HasForeignKey(ra => ra.SideDishId),
                l => l.HasOne(ra => ra.Recipe).WithMany().HasForeignKey(ra => ra.RecipeId));
    }
}