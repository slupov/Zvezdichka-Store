using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Shopping.Models;
using Zvezdichka.Web.Infrastructure.Constants;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;

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

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Checkout()
        {
            var items = this.TempData.Get<List<CheckoutProductsModel>>("items");
            return View(items);
        }

        [HttpPost]
        public IActionResult Checkout([FromBody] List<CheckoutProductsModel> items)
        {
            this.TempData.Put("items", items);

            return RedirectToAction(nameof(Checkout));
        }

        public async Task<IActionResult> Cart()
        {
            var user = this.users2
                .Join(u => u.CartItems)
                .ThenJoin(ci => ci.Product)
                .FirstOrDefault(u => u.UserName == this.User.Identity.Name);

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
        public IActionResult AddToCart(string title, byte quantity)
        {
            var productToAdd = this.products.GetSingle(p => p.Name == title);

            if (productToAdd.Stock <= 0 || productToAdd.Stock < quantity)
            {
                Warning("Cannot add this product to cart. Insufficient stock.", true);

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

            Success($"Successfully added {quantity}x {title}!");

            return Ok();
        }
    }
}