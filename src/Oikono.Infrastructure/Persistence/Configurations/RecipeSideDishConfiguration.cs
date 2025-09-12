using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RecipeSideDishConfiguration : BaseConfiguration<RecipeSideDish, RecipeSideDishId>
{
    public override void ConfigureEntity(EntityTypeBuilder<RecipeSideDish> builder)
    {
        builder.HasOne(rs => rs.Recipe)
            .WithMany()
            .HasForeignKey(rs => rs.RecipeId);

        builder.HasOne(rs => rs.SideDish)
            .WithMany()
            .HasForeignKey(rs => rs.SideDishId);

        builder.HasIndex(rs => new { rs.RecipeId, rs.SideDishId })
            .IsUnique();
    }
}
