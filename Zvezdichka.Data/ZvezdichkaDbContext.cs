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

        //TODO: implement DbContext DI in services, remove this OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=Zvezdichka;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(builder);
        }
    }
}