using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    public class CartItemsDataService : GenericDataService<CartItem>, ICartItemsDataService
    {
        public CartItemsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
