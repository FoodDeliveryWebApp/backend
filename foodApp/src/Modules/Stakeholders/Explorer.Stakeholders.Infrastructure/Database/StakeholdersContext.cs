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

    public DbSet<RatingReport> RatingReports { get; set; }
    public DbSet<RestaurantApplication> RestaurantApplications { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        ConfigureRatingReport(modelBuilder);
        ConfigureStakeholder(modelBuilder);
        ConfigureRestaurant(modelBuilder);
        ConfigureFood(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigureRestaurantRating(modelBuilder);
        ConfigureRestaurantApplication(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureRestaurantApplication(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RestaurantApplication>()
            .Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>();

        modelBuilder.Entity<RestaurantApplication>()
            .Property(a => a.RestaurantName).IsRequired();

        modelBuilder.Entity<RestaurantApplication>()
            .Property(a => a.ManagerUsername).IsRequired();
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // UserRole stored as int (no HasConversion): Administrator=0, Manager=1, Worker=2, DeliveryMan=3, Guest=4
        modelBuilder.Entity<User>().HasData(
            new { Id = 1,  Username = "admin",     Password = "admin",     Role = UserRole.Administrator, IsActive = true },
            new { Id = 2,  Username = "manager1",  Password = "manager1",  Role = UserRole.Manager,       IsActive = true },
            new { Id = 3,  Username = "manager2",  Password = "manager2",  Role = UserRole.Manager,       IsActive = true },
            new { Id = 4,  Username = "manager3",  Password = "manager3",  Role = UserRole.Manager,       IsActive = true },
            new { Id = 5,  Username = "worker1",   Password = "worker1",   Role = UserRole.Worker,        IsActive = true },
            new { Id = 6,  Username = "worker2",   Password = "worker2",   Role = UserRole.Worker,        IsActive = true },
            new { Id = 7,  Username = "delivery1", Password = "delivery1", Role = UserRole.DeliveryMan,   IsActive = true },
            new { Id = 21, Username = "gost1",     Password = "gost1",     Role = UserRole.Guest,         IsActive = true },
            new { Id = 22, Username = "gost2",     Password = "gost2",     Role = UserRole.Guest,         IsActive = true },
            new { Id = 23, Username = "gost3",     Password = "gost3",     Role = UserRole.Guest,         IsActive = true }
        );

        modelBuilder.Entity<Person>().HasData(
            new { Id = 1,  UserId = 1,  Name = "Admin",    Surname = "Adminic",  Email = "admin@gmail.com"     },
            new { Id = 2,  UserId = 2,  Name = "Manager",  Surname = "Jedan",    Email = "manager1@gmail.com"  },
            new { Id = 3,  UserId = 3,  Name = "Manager",  Surname = "Dva",      Email = "manager2@gmail.com"  },
            new { Id = 4,  UserId = 4,  Name = "Manager",  Surname = "Tri",      Email = "manager3@gmail.com"  },
            new { Id = 5,  UserId = 5,  Name = "Worker",   Surname = "Jedan",    Email = "worker1@gmail.com"   },
            new { Id = 6,  UserId = 6,  Name = "Worker",   Surname = "Dva",      Email = "worker2@gmail.com"   },
            new { Id = 7,  UserId = 7,  Name = "Delivery", Surname = "Jedan",    Email = "delivery1@gmail.com" },
            new { Id = 21, UserId = 21, Name = "Gost",     Surname = "Jedan",    Email = "gost1@gmail.com"     },
            new { Id = 22, UserId = 22, Name = "Gost",     Surname = "Dva",      Email = "gost2@gmail.com"     },
            new { Id = 23, UserId = 23, Name = "Gost",     Surname = "Tri",      Email = "gost3@gmail.com"     }
        );

        // CuisineType stored as int (no HasConversion): Italian=0, Chinese=1, Serbian=2
        // ManagerId is a shadow FK property generated from the Manager navigation
        modelBuilder.Entity<Restaurant>().HasData(
            new { Id = 1, Name = "Pizzeria Roma", Address = "Bulevar Oslobodjenja 1, Novi Sad", PhoneNumber = "021-555-001", IsActive = true, Cuisine = CuisineType.Italian, ImageUrl = "images/restaurants/roma.jpg",         ManagerId = 2 },
            new { Id = 2, Name = "Kineski Zid",   Address = "Zmaj Jovina 10, Novi Sad",         PhoneNumber = "021-555-002", IsActive = true, Cuisine = CuisineType.Chinese, ImageUrl = "images/restaurants/chinese_rest.jpg", ManagerId = 3 },
            new { Id = 3, Name = "Srpska Kafana", Address = "Dunavska 5, Novi Sad",             PhoneNumber = "021-555-003", IsActive = true, Cuisine = CuisineType.Serbian, ImageUrl = "images/restaurants/kafana.jpg",       ManagerId = 4 }
        );

        modelBuilder.Entity<Food>().HasData(
            new { Id = 1, Name = "Margherita",       Price = 800.00m,  Description = "Classic tomato and mozzarella pizza",    ImageUrl = "images/foods/margarita.jpg",        RestaurantId = 1 },
            new { Id = 2, Name = "Pepperoni",        Price = 950.00m,  Description = "Pizza with pepperoni and mozzarella",    ImageUrl = "images/foods/peperoni.jpg",         RestaurantId = 1 },
            new { Id = 3, Name = "Tiramisu",         Price = 450.00m,  Description = "Traditional Italian dessert",            ImageUrl = "images/foods/tiramisu.jpg",         RestaurantId = 1 },
            new { Id = 4, Name = "Kung Pao Chicken", Price = 700.00m,  Description = "Spicy stir-fried chicken with peanuts",  ImageUrl = "images/foods/kung-pao-chicken.jpg", RestaurantId = 2 },
            new { Id = 5, Name = "Spring Rolls",     Price = 400.00m,  Description = "Crispy vegetable spring rolls",          ImageUrl = "images/foods/spring_roles.jpg",     RestaurantId = 2 },
            new { Id = 6, Name = "Fried Rice",       Price = 550.00m,  Description = "Classic Chinese fried rice",             ImageUrl = "images/foods/fried_rice.jpg",       RestaurantId = 2 },
            new { Id = 7, Name = "Rostilj Mix",      Price = 1200.00m, Description = "Mixed grill platter with sides",         ImageUrl = "images/foods/rostilj.jpg",          RestaurantId = 3 },
            new { Id = 8, Name = "Riblja Corba",     Price = 600.00m,  Description = "Traditional Serbian fish soup",          ImageUrl = "images/foods/riblja-corba.jpg",     RestaurantId = 3 },
            new { Id = 9, Name = "Gibanica",         Price = 350.00m,  Description = "Serbian cheese pie",                     ImageUrl = "images/foods/gibanica.jpg",         RestaurantId = 3 }
        );

        // OrderStatus stored as string (HasConversion<string>)
        modelBuilder.Entity<Order>().HasData(
            new { Id = 1, UserId = 21, OrderTime = new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc), Status = OrderStatus.Delivered,  Note = "Extra napkins please" },
            new { Id = 2, UserId = 22, OrderTime = new DateTime(2024, 3, 2, 13, 30, 0, DateTimeKind.Utc), Status = OrderStatus.Delivered,  Note = ""                    },
            new { Id = 3, UserId = 21, OrderTime = new DateTime(2024, 3, 3, 19, 0, 0, DateTimeKind.Utc), Status = OrderStatus.Preparing,  Note = "No onions"           },
            new { Id = 4, UserId = 23, OrderTime = new DateTime(2024, 3, 4, 20, 15, 0, DateTimeKind.Utc), Status = OrderStatus.PickUp,     Note = ""                    },
            new { Id = 5, UserId = 22, OrderTime = new DateTime(2024, 3, 5, 11, 0, 0, DateTimeKind.Utc), Status = OrderStatus.Rejected,   Note = "Allergy to nuts"     }
        );

        // RatedById and RestaurantId are shadow FKs from navigation properties
        modelBuilder.Entity<RestaurantRating>().HasData(
            new { Id = 1, Rating = 8, Comment = "Odlicna pizza, preporucujem!",        RatedById = 21, RestaurantId = 1, CreatedAt = new DateTime(2024, 3, 2, 10, 0, 0, DateTimeKind.Utc), isDeleted = false },
            new { Id = 2, Rating = 6, Comment = "Solidno, ali malo spora dostava.",    RatedById = 22, RestaurantId = 1, CreatedAt = new DateTime(2024, 3, 3, 11, 0, 0, DateTimeKind.Utc), isDeleted = false },
            new { Id = 3, Rating = 9, Comment = "Fantastican kineski, sveze i ukusno.",RatedById = 21, RestaurantId = 2, CreatedAt = new DateTime(2024, 3, 4, 14, 0, 0, DateTimeKind.Utc), isDeleted = false },
            new { Id = 4, Rating = 5, Comment = "Ocekivao sam vise od kafane.",        RatedById = 23, RestaurantId = 3, CreatedAt = new DateTime(2024, 3, 5, 16, 0, 0, DateTimeKind.Utc), isDeleted = false },
            new { Id = 5, Rating = 7, Comment = "Dobra atmosfera i ukusna hrana.",     RatedById = 22, RestaurantId = 3, CreatedAt = new DateTime(2024, 3, 6, 18, 0, 0, DateTimeKind.Utc), isDeleted = false }
        );

        // RatingReportStatus stored as string; OrderId and ManagerId are shadow FKs
        modelBuilder.Entity<RatingReport>().HasData(
            new { Id = 1, OrderId = 1, ManagerId = 2, Comment = "Guest reported an issue with order quality.",      Status = RatingReportStatus.Pending,  CreatedAt = new DateTime(2024, 3, 2, 9, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, OrderId = 2, ManagerId = 3, Comment = "Late delivery, compensation approved.",           Status = RatingReportStatus.Approved, CreatedAt = new DateTime(2024, 3, 3, 10, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, OrderId = 5, ManagerId = 4, Comment = "Order rejected due to allergen miscommunication.",Status = RatingReportStatus.Rejected, CreatedAt = new DateTime(2024, 3, 6, 8, 0, 0, DateTimeKind.Utc) }
        );
    }

    private static void ConfigureRatingReport(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RatingReport>()
            .Property(r => r.Comment)
            .IsRequired();

        modelBuilder.Entity<RatingReport>()
            .Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();

        modelBuilder.Entity<RatingReport>()
            .HasOne(r => r.Order)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<RatingReport>()
            .HasOne(r => r.Manager)
            .WithMany()
            .IsRequired();
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