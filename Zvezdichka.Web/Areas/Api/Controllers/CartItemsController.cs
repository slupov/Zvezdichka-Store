using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Zvezdichka.Common;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Api.Models;
using Zvezdichka.Web.Areas.Api.Models.CartItems;
using Zvezdichka.Web.Infrastructure.Constants;

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
        public async Task<IActionResult> Delete(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return NotFound();

            var cartItem = this.cartItems.Join(m => m.Product).FirstOrDefault(m => m.Product.Name == productName);

            if (cartItem == null)
                return NotFound();

            this.cartItems.Remove(cartItem);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItemUpdateModel cartItem)
        {
            var toUpdate = this.cartItems.Join(x => x.Product).FirstOrDefault(x => x.Id == cartItem.Id);

            if (toUpdate == null)
            {
                return NotFound();
            }

            if (toUpdate.Product.Stock < cartItem.Quantity)
            {
                return BadRequest(CommonConstants.StockAmountExceededError);
            }

            if (!this.ModelState.IsValid)
            {
                var errorMsg = string.Empty;

                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMsg += error.ErrorMessage + "\n";
                    }
                }

                return BadRequest(errorMsg);
            }

            toUpdate.Quantity = (byte)cartItem.Quantity;
            this.cartItems.Update(toUpdate);

            return Ok();
        }
    }
}