using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Zvezdichka.Data;
using Zvezdichka.Services.Implementations.Entity;

namespace Zvezdichka.Tests
{
    public class ProductsDataServiceTests
    {
        public ProductsDataServiceTests()
        {
            Tests.Initialize();
        }

        [Fact]
        public void FindAsyncShouldReturnCorrectResultWithFilterAndOrder()
        {
            //Arrange
            using (var db = this.GetDatabase())
            {
               var products = new ProductsDataService(db);

                Console.WriteLine();
            }

            //Act


            //Assert
        }

        private ZvezdichkaDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<ZvezdichkaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ZvezdichkaDbContext(dbOptions);
        }
    }
}
