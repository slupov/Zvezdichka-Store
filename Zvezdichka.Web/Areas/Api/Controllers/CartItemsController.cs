using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Zvezdichka.Common;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Api.Models;
using Zvezdichka.Web.Areas.Api.Models.CartItems;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    [Authorize]
    public class CartItemsController : ApiBaseController
    {
        private readonly ICartItemsDataService cartItems;
        private readonly IProductsDataService products;
        private readonly UserManager<ApplicationUser> users;

        public CartItemsController(ICartItemsDataService cartItems, IProductsDataService products,
            UserManager<ApplicationUser> users)
        {
            this.cartItems = cartItems;
            this.products = products;
            this.users = users;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = this.cartItems.GetSingle(x => x.Id == id, x => x.User);

            if (cartItem == null)
                return NotFound();

            if (cartItem.User.UserName != this.User.Identity.Name)
            {
                return Unauthorized();
            }

            this.cartItems.Remove(cartItem);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItemUpdateModel cartItem)
        {
            var toUpdate = this.cartItems
                .Join(x => x.Product)
                .Join(x => x.User)
                .SingleOrDefault(x => x.Id == cartItem.Id);

            if (toUpdate == null)
                return NotFound();

            if (toUpdate.User.UserName != this.User.Identity.Name)
            {
                return Unauthorized();
            }

            if (toUpdate.Product.Stock < cartItem.Quantity)
                return BadRequest(CommonConstants.StockAmountExceededError);

            if (!this.ModelState.IsValid)
            {
                var errorMsg = string.Empty;

                foreach (var modelState in this.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errorMsg += error.ErrorMessage + "\n";

                return BadRequest(errorMsg);
            }

            toUpdate.Quantity = (byte) cartItem.Quantity;
            this.cartItems.Update(toUpdate);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string title, int quantity)
        {
            var productToAdd = this.products.GetSingle(p => p.Name == title);

            if (quantity <= 0)
            {
                return BadRequest(CommonConstants.WrongStockAmount);
            }

            if (productToAdd.Stock < quantity)
            {
                return BadRequest(string.Format(CommonConstants.StockAmountExceededForError, productToAdd.Name));
            }

            //already such cart product exists
            var cartItem =
                this.cartItems.GetSingle(c => c.User.UserName == this.User.Identity.Name && c.Product.Name == title,
                    c => c.User, c => c.Product);

            if (cartItem != null)
            {
                cartItem.Quantity += (byte)quantity;
                this.cartItems.Update(cartItem);

                return Ok(new
                {
                    responseText = string.Format(CommonConstants.SuccessfullyAddedMoreOfThisItem)
                });
            }

            var user = this.users.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult();
            var cartProduct = new CartItem()
            {
                Product = productToAdd,
                Quantity = (byte)quantity,
                User = user
            };

            this.cartItems.Add(cartProduct);

            return Ok(new
            {
                responseText = string.Format(CommonConstants.SuccessfullyAddedCartItem)
            });
        }
    }
}