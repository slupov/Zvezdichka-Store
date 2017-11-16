using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            if (!context.Database.GetPendingMigrations().Any())
            {
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

//                //seeding categories
//                if (!context.Categories.Any())
//                {
//                    context.Categories.AddRange(categoriesToSeed);
//                    context.SaveChanges();
//                }

                //seeding products and categories
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
}