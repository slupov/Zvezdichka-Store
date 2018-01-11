﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Faq> Faqs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rating>().ToTable("Ratings");

            builder.AddConfiguration(new ProductConfiguration());
            builder.AddConfiguration(new CategoryConfiguration());
            builder.AddConfiguration(new CategoryProductConfiguration());
            builder.AddConfiguration(new CommentConfiguration());
            builder.AddConfiguration(new FaqConfiguration());

            base.OnModelCreating(builder);
        }

    }
}