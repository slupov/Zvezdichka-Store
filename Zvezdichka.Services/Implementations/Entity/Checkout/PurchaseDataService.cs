using Zvezdichka.Data;
using Zvezdichka.Data.Models.Checkout;
using Zvezdichka.Services.Contracts.Entity.Checkout;

namespace Zvezdichka.Services.Implementations.Entity.Checkout
{
    public class PurchaseDataService : GenericDataService<Purchase>, IPurchaseDataService
    {
        public PurchaseDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}