using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Checkout;
using Zvezdichka.Services.Contracts.Entity.Checkout;

namespace Zvezdichka.Services.Implementations.Entity.Checkout
{
    public class PaymentOptionsDataService : GenericDataService<PaymentOption>, IPaymentOptionsDataService
    {
        public PaymentOptionsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}