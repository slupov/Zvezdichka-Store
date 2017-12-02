using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    public class ProductsDataService : GenericDataService<Product>, IProductsDataService
    {
        public ProductsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
