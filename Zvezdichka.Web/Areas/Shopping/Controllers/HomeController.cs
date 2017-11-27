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
        private readonly ICartsDataService carts;
        private readonly UserManager<ApplicationUser> users;

        public HomeController(IProductsDataService products, ICartsDataService carts,
            UserManager<ApplicationUser> users)
        {
            this.products = products;
            this.carts = carts;
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

//        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(string productName, byte quantity)
        {
            var productToAdd = this.products.GetSingle(p => p.Name == productName);

            if (productToAdd.Stock <= 0 || productToAdd.Stock < quantity)
            {
                this.ViewData["warning"] = "Cannot add this product to cart. Insufficient stock.";
                return RedirectToRoute(WebConstants.ProductWithCategoryRoutingName, new {id = productToAdd});
            }

            var cartProduct = new Cart()
            {
                Product = productToAdd,
                Quantity = quantity,
                User = this.users.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult()
            };

//            this.carts.Add(cartProduct);
            this.ViewData["success"] = $"Successfully added {quantity}x {productName}!";

            return RedirectToRoute(WebConstants.ProductWithCategoryRoutingName,
                new
                {
                    id = productToAdd.Id,
                    productName = productName,
                    category = productToAdd.Categories.FirstOrDefault()
                });
        }
    }
}