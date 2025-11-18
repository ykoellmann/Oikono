using Microsoft.EntityFrameworkCore;
using Oikono.Entities;

namespace Oikono.Data;

public class OikonoDbContext : DbContext
{
    public OikonoDbContext(DbContextOptions<OikonoDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<PartIngredient> PartIngredients { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SideDish> SideDishes { get; set; }
    public DbSet<RecipeTag> RecipeTags { get; set; }
    public DbSet<RecipeSideDish> RecipeSideDishes { get; set; }
    public DbSet<RecipeAsset> RecipeAssets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        // RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();
            entity.Property(e => e.Disabled).HasDefaultValue(false);

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Recipe
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Part
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.Parts)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PartIngredient
        modelBuilder.Entity<PartIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Part)
                .WithMany(p => p.PartIngredients)
                .HasForeignKey(e => e.PartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Ingredient)
                .WithMany(i => i.PartIngredients)
                .HasForeignKey(e => e.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Step
        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.Steps)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Ingredient
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Device
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Tag
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // SideDish
        modelBuilder.Entity<SideDish>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // RecipeTag (Junction table)
        modelBuilder.Entity<RecipeTag>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.TagId });

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeTags)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RecipeSideDish (Junction table)
        modelBuilder.Entity<RecipeSideDish>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.SideDishId });

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeSideDishes)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SideDish)
                .WithMany(s => s.RecipeSideDishes)
                .HasForeignKey(e => e.SideDishId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RecipeAsset
        modelBuilder.Entity<RecipeAsset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Data).IsRequired();

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeAssets)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
