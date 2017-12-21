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

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Rating>().ToTable("Ratings");
            builder.Entity<Product>().ToTable("Products");

            builder.AddConfiguration(new CartItemConfiguration());
            builder.AddConfiguration(new CategoryProductConfiguration());
            builder.AddConfiguration(new CommentConfiguration());

            base.OnModelCreating(builder);
        }

    }
}