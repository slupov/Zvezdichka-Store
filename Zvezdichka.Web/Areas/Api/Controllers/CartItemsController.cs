using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Api.Models.CartItems;
using Zvezdichka.Web.Areas.Shopping.Models;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CartItemsController : ApiBaseController
    {
        private readonly ICartItemsDataService cartItems;

        public CartItemsController(ICartItemsDataService cartItems)
        {
            this.cartItems = cartItems;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
                return NotFound();

            var cartItem = this.cartItems.Join(m => m.Product).FirstOrDefault(m => m.Product.Name == name);

            if (cartItem == null)
                return NotFound();

            this.cartItems.Remove(cartItem);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItemUpdateModel cartItem)
        {
            var toUpdate = this.cartItems.GetSingle(x => x.Id == cartItem.Id);

            if (toUpdate == null)
            {
                return NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            toUpdate.Quantity = cartItem.Quantity;
            this.cartItems.Update(toUpdate);

            return Ok();
        }
    }
}