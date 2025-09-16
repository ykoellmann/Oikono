using Oikono.Domain.Idempotencies;
using Oikono.Domain.Models;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Oikono.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Oikono.Domain.Assets;
using Oikono.Domain.Recipes;

namespace Oikono.Infrastructure.Persistence;

public class OikonoDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public OikonoDbContext(DbContextOptions<OikonoDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Policy> Policies { get; set; } = null!;
    public DbSet<UserPolicy> UserPolicies { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<UserPermission> UserPermissions { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RefreshToken?> RefreshTokens { get; set; } = null!;

    public DbSet<Idempotency> Idempotencies { get; set; } = null!;

    public DbSet<Asset> Assets { get; set; } = null!;

    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<RecipeAsset> RecipeAssets { get; set; } = null!;

    public DbSet<Part> Parts { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<SideDish> SideDishes { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<PartIngredient> PartIngredients { get; set; } = null!;
    public DbSet<RecipeSideDish> RecipeSideDishes { get; set; } = null!;
    public DbSet<RecipeTag> RecipeTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore(typeof(UserId));
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(OikonoDbContext).Assembly);
        modelBuilder.HasDefaultSchema("Oikono");
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}