using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Extensions.Helpers;

namespace Zvezdichka.Web.Areas.Shopping.Controllers
{
    public class HomeController : ShoppingBaseController
    {
        private readonly IProductsDataService products;
        private readonly ICartItemsDataService cartItems;
        private readonly UserManager<ApplicationUser> users;

        public HomeController(IProductsDataService products, ICartItemsDataService cartItems,
            UserManager<ApplicationUser> users)
        {
            this.products = products;
            this.cartItems = cartItems;
            this.users = users;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        /// <param name="title">Product name</param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult AddToCart(string title, byte quantity)
        {
            var productToAdd = this.products.GetSingle(p => p.Name == title);

            if (productToAdd.Stock <= 0 || productToAdd.Stock < quantity)
            {
                this.ViewData["warning"] = "Cannot add this product to cart. Insufficient stock.";
                return RedirectToRoute(WebConstants.ProductDetailsFriendlyRouteName, new {id = productToAdd.Id, title = title});
            }

            var cartProduct = new CartItem()
            {
                Product = productToAdd,
                Quantity = quantity,
                User = this.users.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult()
            };

            this.cartItems.Add(cartProduct);
            this.ViewData["success"] = $"Successfully added {quantity}x {title}!";

            return RedirectToRoute(WebConstants.ProductDetailsFriendlyRouteName,
                new
                {
                    id = productToAdd.Id,
                    title = title,
                });
        }
    }
}