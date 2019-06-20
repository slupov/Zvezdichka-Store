using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services;

namespace Zvezdichka.Tests.Services
{
    public class ProductsDataServiceTests
    {
        public ProductsDataServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public void ShouldAddProduct()
        {
            //Arrange
            var db = Zvezdichka.Tests.Tests.GetDatabase();

            var p1 = new Product()
            {
                Name = "First",
                Description = "First Description",
                Price = 12.12m,
                Stock = 15
            };

            var p2 = new Product()
            {
                Name = "Second",
                Description = "Second Description",
                Price = 8.99m,
                Stock = 10
            };

            var products = new GenericDataService<Product>(db);

            //Act
            products.Add(p1, p2);
            var result = products.GetAllAsync().GetAwaiter().GetResult();

            //Assert
            result.Should().BeOfType<List<Product>>();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldReturnAllProducts()
        {
            //Arrange
            var db = Zvezdichka.Tests.Tests.GetDatabase();

            var p1 = new Product()
            {
                Name = "First",
                Description = "First Description",
                Price = 12.12m,
                Stock = 15
            };

            var p2 = new Product()
            {
                Name = "Second",
                Description = "Second Description",
                Price = 8.99m,
                Stock = 10
            };

            var products = new GenericDataService<Product>(db);

            products.Add(p1, p2);

            //Act
            var result = products.GetAllAsync().GetAwaiter().GetResult();

            //Assert
            result.Should().BeOfType<List<Product>>();

            result.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldUpdateProduct()
        {
            //Arrange
            var db = Zvezdichka.Tests.Tests.GetDatabase();

            var p1 = new Product()
            {
                Name = "First",
                Description = "First Description",
                Price = 12.12m,
                Stock = 15
            };

            //Act

            var products = new GenericDataService<Product>(db);
            products.Add(p1);

            p1.Name = "Updated";
            products.Update(p1);

            //Assert
            products.GetListAsync(x => x.Name == "Updated").GetAwaiter().GetResult().Count.Should().Be(1);
        }

        [Fact]
        public void ShouldDeleteProduct()
        {
            //Arrange
            var db = Zvezdichka.Tests.Tests.GetDatabase();

            var p1 = new Product()
            {
                Name = "First",
                Description = "First Description",
                Price = 12.12m,
                Stock = 15
            };


            var p2 = new Product()
            {
                Name = "Second",
                Description = "Second Description",
                Price = 8.99m,
                Stock = 10
            };


            var products = new GenericDataService<Product>(db);
            products.Add(p1, p2);

            //Act

            products.Remove(p1);
            //Assert

            products.GetAllAsync().GetAwaiter().GetResult().Count.Should().Be(1);
        }

        [Fact]
        public void ShouldGetWithNavigationProperties()
        {
            //Arrange
            var db = Zvezdichka.Tests.Tests.GetDatabase();

            var p1 = new Product()
            {
                Name = "First",
                Description = "First Description",
                Price = 12.12m,
                Stock = 15,
                Categories = new List<CategoryProduct>()
                {
                    new CategoryProduct()
                    {
                        Category = new Category()
                        {
                            Name = "Toys"
                        }
                    },
                    new CategoryProduct()
                    {
                        Category = new Category()
                        {
                            Name = "Shirts"
                        }
                    }
                }
            };

            var products = new GenericDataService<Product>(db);
            products.Add(p1);

            //Act

            var result = products.GetSingleOrDefaultAsync(x => x.Name == "First").GetAwaiter().GetResult();

            //Assert

            result.Categories.Count.Should().Be(2);
        }
    }
}