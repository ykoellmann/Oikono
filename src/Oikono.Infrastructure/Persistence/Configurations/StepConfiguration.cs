using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Infrastructure.Persistence.Configurations;

public class StepConfiguration : BaseConfiguration<Step, StepId>
{
    public override void ConfigureEntity(EntityTypeBuilder<Step> builder)
    {
        builder.Property(s => s.RecipeId)
            .HasConversion(
                id => id.Value,
                value => new RecipeId(value));

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(s => s.Recipe)
            .WithMany(r => r.Steps)
            .HasForeignKey(s => s.RecipeId);

        builder.HasOne(s => s.Device)
            .WithMany()
            .HasForeignKey(s => s.DeviceId);
    }
}
