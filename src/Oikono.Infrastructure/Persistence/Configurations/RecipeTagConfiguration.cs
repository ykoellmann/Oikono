using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class RecipeTagConfiguration : BaseConfiguration<RecipeTag, RecipeTagId>
{
    public override void ConfigureEntity(EntityTypeBuilder<RecipeTag> builder)
    {
        builder.HasOne(rt => rt.Recipe)
            .WithMany()
            .HasForeignKey(rt => rt.RecipeId);

        builder.HasOne(rt => rt.Tag)
            .WithMany()
            .HasForeignKey(rt => rt.TagId);

        builder.HasIndex(rt => new { rt.RecipeId, rt.TagId })
            .IsUnique();
    }
}
