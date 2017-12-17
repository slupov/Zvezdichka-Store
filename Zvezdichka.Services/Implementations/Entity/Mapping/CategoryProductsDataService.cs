using Zvezdichka.Data;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services.Contracts.Entity.Mapping;

namespace Zvezdichka.Services.Implementations.Entity.Mapping
{
    public class CategoryProductsDataService : GenericDataService<CategoryProduct>, ICategoryProductsDataService
    {
        public CategoryProductsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
