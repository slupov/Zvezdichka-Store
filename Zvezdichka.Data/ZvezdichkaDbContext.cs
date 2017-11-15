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

        /// <summary>
        /// adds new DbContextOptions<DbContext> as a parameter by default
        /// </summary>
        public ZvezdichkaDbContext() : this(new DbContextOptions<ZvezdichkaDbContext>())
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Rating>().ToTable("Ratings");

            builder.AddConfiguration(new CartsConfiguration());
            builder.AddConfiguration(new CategoryProductConfiguration());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }
    }
}