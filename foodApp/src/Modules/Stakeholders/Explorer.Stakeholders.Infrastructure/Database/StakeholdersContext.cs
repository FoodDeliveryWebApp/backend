using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<RestaurantRating> RestaurantRatings { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        ConfigureStakeholder(modelBuilder);
        ConfigureRestaurant(modelBuilder);
        ConfigureFood(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigureRestaurantRating(modelBuilder);
    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);
    }

    private static void ConfigureRestaurant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Address)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.PhoneNumber)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.ImageUrl)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Foods)
            .WithOne()
            .HasForeignKey(f => f.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Workers)
            .WithMany();

        modelBuilder.Entity<Restaurant>()
            .HasOne(r => r.Manager)
            .WithMany();
    }

    private static void ConfigureFood(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Food>()
            .Property(f => f.Name)
            .IsRequired();

        modelBuilder.Entity<Food>()
            .Property(f => f.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Food>()
            .Property(f => f.ImageUrl)
            .IsRequired();

        modelBuilder.Entity<Food>()
            .HasOne<Restaurant>()
            .WithMany(r => r.Foods)
            .HasForeignKey(f => f.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(o => o.UserId)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderTime)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .Property(o => o.ApprovalStatus)
            .IsRequired()
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .Property(o => o.Note)
            .HasMaxLength(500);

        // Many-to-many relationship between Order and Food
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Foods)
            .WithMany();
    }

    private static void ConfigureRestaurantRating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RestaurantRating>()
            .Property(r => r.Rating)
            .IsRequired();

        modelBuilder.Entity<RestaurantRating>()
            .Property(r => r.Comment)
            .IsRequired();

        modelBuilder.Entity<RestaurantRating>()
            .Property(r => r.CreatedAt)
            .IsRequired();

        modelBuilder.Entity<RestaurantRating>()
            .HasOne(r => r.RatedBy)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<RestaurantRating>()
            .HasOne(r => r.Restaurant)
            .WithMany()
            .IsRequired();
    }
}