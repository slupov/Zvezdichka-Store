using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.EntityConfigurations;
using Zvezdichka.Data.EntityConfigurations.Checkout;
using Zvezdichka.Data.EntityConfigurations.Distributor;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Checkout;
using Zvezdichka.Data.Models.Distributors;

namespace Zvezdichka.Data
{
    public class ZvezdichkaDbContext : IdentityDbContext<ApplicationUser>
    {
        public ZvezdichkaDbContext(DbContextOptions<ZvezdichkaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Faq> Faqs { get; set; }

        public DbSet<DeliveryOption> DeliveryOptions { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }

        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorShipment> DistributorShipments { get; set; }
        public DbSet<DistributorShipmentProduct> DistributorShipmentProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rating>().ToTable("Ratings");

            builder.AddConfiguration(new ProductConfiguration());
            builder.AddConfiguration(new CategoryConfiguration());
            builder.AddConfiguration(new CategoryProductConfiguration());

            builder.AddConfiguration(new CommentConfiguration());
            builder.AddConfiguration(new FaqConfiguration());

            builder.AddConfiguration(new DeliveryOptionConfiguration());
            builder.AddConfiguration(new PaymentOptionConfiguration());

            builder.AddConfiguration(new DistributorConfiguration());
            builder.AddConfiguration(new DistributorShipmentConfiguration());
        }
    }
}