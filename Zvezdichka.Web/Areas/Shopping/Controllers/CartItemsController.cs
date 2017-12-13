using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET:
        public async Task<IActionResult> Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
                return NotFound();

            var cartItem = this.cartItems.Join(m => m.Product).FirstOrDefault(m => m.Product.Name == name);

            if (cartItem == null)
                return NotFound();

            this.cartItems.Remove(cartItem);

            return RedirectToAction("Index", "Home", new {area = "Shopping"});
        }
    }
}