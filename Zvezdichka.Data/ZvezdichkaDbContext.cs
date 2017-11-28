using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.EntityConfigurations;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data
{
    public class ZvezdichkaDbContext : IdentityDbContext<ApplicationUser>
    {
        public ZvezdichkaDbContext(DbContextOptions<ZvezdichkaDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ImageSource> ImageSources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Rating>().ToTable("Ratings");
            builder.Entity<Product>().ToTable("Products");

            builder.AddConfiguration(new ShoppingCartConfiguration());
            builder.AddConfiguration(new CartItemConfiguration());
            builder.AddConfiguration(new CategoryProductConfiguration());
            builder.AddConfiguration(new ImageSourceConfiguration());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=Zvezdichka;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(builder);
        }
    }
}