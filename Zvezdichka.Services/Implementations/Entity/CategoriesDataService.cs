using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    public class CategoriesDataService : GenericDataService<Category>, ICategoriesDataService
    {
        public CategoriesDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
