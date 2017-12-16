using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Web.Areas.Shopping.Controllers
{
    public class CartItemsController : ShoppingBaseController
    {
        private readonly ICartItemsDataService cartItems;

        public CartItemsController(ICartItemsDataService cartItems)
        {
            this.cartItems = cartItems;
        }
        
    }
}