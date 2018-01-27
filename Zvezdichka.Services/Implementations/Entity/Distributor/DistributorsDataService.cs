using Zvezdichka.Data;
using Zvezdichka.Services.Contracts.Entity.Distributor;

namespace Zvezdichka.Services.Implementations.Entity.Distributor
{
    public class DistributorsDataService : GenericDataService<Data.Models.Distributors.Distributor>,
        IDistributorsDataService
    {
        public DistributorsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}