using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Assets.ValueObjects;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RecipeAssetConfiguration : BaseConfiguration<RecipeAsset, RecipeAssetId>
{
    public override void ConfigureEntity(EntityTypeBuilder<RecipeAsset> builder)
    {
        builder.HasOne(ra => ra.Recipe)
            .WithMany()
            .HasForeignKey(ra => ra.RecipeId);

        builder.HasOne(ra => ra.Asset)
            .WithMany()
            .HasForeignKey(ra => ra.AssetId);
    }
}
