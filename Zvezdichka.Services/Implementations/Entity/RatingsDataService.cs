using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    public class RatingsDataService : GenericDataService<Rating>, IRatingsDataService
    {
        public RatingsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}