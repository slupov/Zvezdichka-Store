using Zvezdichka.Data;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Contracts.Entity.Distributor;

namespace Zvezdichka.Services.Implementations.Entity.Distributor
{
    public class DistributorShipmentsDataService : GenericDataService<DistributorShipment>, IDistributorShipmentsDataService
    {
        public DistributorShipmentsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
