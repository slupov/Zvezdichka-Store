using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;

namespace Zvezdichka.Data.Helpers
{
    public static class ZvezdichkaDbContextExtensions
    {
        public static void EnsureSeedData(this ZvezdichkaDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any()) return;

            var categoriesToSeed = new Category[]
            {
                new Category()
                {
                    Name = "Toys"
                },
                new Category()
                {
                    Name = "Shoes"
                },
                new Category()
                {
                    Name = "Trousers"
                },
                new Category()
                {
                    Name = "Shirts"
                }
            };

            //seeding products and categories
            SeedProductsAndCategories(context, categoriesToSeed);
            SeedRatings(context);
            SeedComments(context);
        }

        private static void SeedRatings(ZvezdichkaDbContext context)
        {
            if (context.Ratings.Any()) return;

            var currentDirectory = Path.GetFullPath(@"..\Zvezdichka.Data\Import\Json\RatingsMock.json");
            string json = File.ReadAllText(currentDirectory);
            var ratingsToSeed = JsonConvert.DeserializeObject<List<Rating>>(json);

            Random r = new Random();

            var products = context.Products.ToList();
            var user = context.Users.First();

            foreach (var rating in ratingsToSeed)
            {
                rating.Product = products[r.Next(1, 25)];
                rating.User = user;
            }

            context.Ratings.AddRange(ratingsToSeed);
            context.SaveChanges();
        }

        private static void SeedComments(ZvezdichkaDbContext context)
        {
            if (context.Comments.Any()) return;

            var currentDirectory = Path.GetFullPath(@"..\Zvezdichka.Data\Import\Json\CommentsMock.json");
            string json = File.ReadAllText(currentDirectory);
            var commentsToSeed = JsonConvert.DeserializeObject<List<Comment>>(json);

            Random r = new Random();

            var products = context.Products.ToList();
            var user = context.Users.First();

            //attach product and user to comments
            foreach (var comment in commentsToSeed)
            {
                comment.Product = products[r.Next(1, 25)];
                comment.User = user;
            }

            context.AddRange(commentsToSeed);
            context.Comments.AddRange(commentsToSeed);
                context.SaveChanges(); //BUG:throws null reference
            }

        private static void SeedProductsAndCategories(ZvezdichkaDbContext context, Category[] categoriesToSeed)
        {
            if (!context.Products.Any())
            {
                var currentDirectory = Path.GetFullPath(@"..\Zvezdichka.Data\Import\Json\ProductsMock.json");
                string json = File.ReadAllText(currentDirectory);
                var productsToSeed = JsonConvert.DeserializeObject<List<Product>>(json);

                foreach (var product in productsToSeed)
                {
                    Random r = new Random();
                    product.Categories.Add(new CategoryProduct()
                    {
                        Product = product,
                        Category = categoriesToSeed[r.Next(0, 4)]
                    });
                }

                context.Products.AddRange(productsToSeed);
                context.SaveChanges();
            }
        }
    }
}