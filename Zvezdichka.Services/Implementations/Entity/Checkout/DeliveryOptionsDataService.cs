using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Checkout;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Contracts.Entity.Checkout;

namespace Zvezdichka.Services.Implementations.Entity.Checkout
{
    public class DeliveryOptionsDataService : GenericDataService<DeliveryOption>, IDeliveryOptionsDataService
    {
        public DeliveryOptionsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}