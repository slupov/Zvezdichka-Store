using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Common;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Api.Models.CartItems;
using Zvezdichka.Web.Areas.Shopping.Models;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Shopping.Controllers
{
    public class HomeController : ShoppingBaseController
    {
        private readonly IProductsDataService products;
        private readonly ICartItemsDataService cartItems;
        private readonly UserManager<ApplicationUser> users;
        private readonly IApplicationUserDataService users2;

        public HomeController(IProductsDataService products, ICartItemsDataService cartItems,
            IApplicationUserDataService users2,
            UserManager<ApplicationUser> users)
        {
            this.products = products;
            this.cartItems = cartItems;
            this.users = users;
            this.users2 = users2;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Cart));
        }

        public async Task<IActionResult> Cart()
        {
            var user = this.users2
                .Join(u => u.CartItems)
                .ThenJoin(ci => ci.Product)
                .SingleOrDefault(u => u.UserName == this.User.Identity.Name);

            var userCartItems = user.CartItems.AsQueryable().ToList();

            //see usercartitems
            return View(userCartItems.AsQueryable().ProjectTo<CartItemListingViewModel>());
        }

        /// <summary>
        /// Handles an ajax add to cart request
        /// </summary>
        /// <param name="title"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> AddToCart(string title, byte quantity)
        {
            var productToAdd = this.products.GetSingle(p => p.Name == title);

            if (productToAdd.Stock <= 0 || productToAdd.Stock < quantity)
            {
                Warning(string.Format(CommonConstants.StockAmountExceededForError,productToAdd.Name), true);

                return RedirectToRoute(WebConstants.Routes.ProductDetails,
                    new {id = productToAdd.Id, title = title});
            }

            //already such cart product exists
            var cartItem =
                this.cartItems.GetSingle(c => c.User.UserName == this.User.Identity.Name && c.Product.Name == title,
                    c => c.User, c => c.Product);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                this.cartItems.Update(cartItem);

                return Ok();
            }

            var user = this.users.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult();
            var cartProduct = new CartItem()
            {
                Product = productToAdd,
                Quantity = quantity,
                User = user
            };

            this.cartItems.Add(cartProduct);

            return Ok();
        }

        public async Task<IActionResult> DeleteCart(int id)
        {
            var cartItem = this.cartItems.Join(x => x.Product).SingleOrDefault(x => x.Id == id);

            if (cartItem == null)
                return NotFound();

            this.cartItems.Remove(cartItem);

            Success(string.Format(CommonConstants.DeletedCartItemSuccessfully, cartItem.Product.Name));
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(CartItemUpdateModel cartItem)
        {
            var toUpdate = this.cartItems.Join(x => x.Product).SingleOrDefault(x => x.Id == cartItem.Id);

            if (toUpdate == null)
                return NotFound();

            if (toUpdate.Product.Stock < cartItem.Quantity)
            {
                Danger(CommonConstants.StockAmountExceededError);
                return RedirectToAction(nameof(Cart));
            }

            if (!this.ModelState.IsValid)
            {
                var errorMsg = string.Empty;

                foreach (var modelState in this.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errorMsg += error.ErrorMessage + "\n";

                Danger(CommonConstants.StockAmountExceededError);
                return RedirectToAction(nameof(Cart));
            }

            toUpdate.Quantity = (byte) cartItem.Quantity;
            this.cartItems.Update(toUpdate);

            Success(CommonConstants.UpdatedCartItemSuccessfully);
            return RedirectToAction(nameof(Cart));
        }
    }
}