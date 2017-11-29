using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data;
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

        public async Task<IActionResult> Cart()
        {
            var userId = this.users.GetUserId(this.HttpContext.User);

            ApplicationUser user = null;

            using (var context = new ZvezdichkaDbContext(new DbContextOptions<ZvezdichkaDbContext>()))
            {
                user = context.Users.Include(c => c.CartItems).FirstOrDefault(u => u.Id == userId);
            }

            var userCartItems = user.CartItems; //bug = 0

            return View(userCartItems);
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
                return RedirectToRoute(WebConstants.ProductDetailsFriendlyRouteName,
                    new {id = productToAdd.Id, title = title});
            }

            //already such cart product exists
            var cartItem =
                this.cartItems.GetSingle(c => c.User.UserName == this.User.Identity.Name && c.Product.Name == title);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                this.cartItems.Update(cartItem);

                return RedirectToRoute(WebConstants.ProductDetailsFriendlyRouteName,
                    new
                    {
                        id = productToAdd.Id,
                        title = title,
                    });
            }

            var user = this.users.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult();
            var cartProduct = new CartItem()
            {
                Product = productToAdd,
                Quantity = quantity,
                User = user
            };

            user.CartItems.Add(cartProduct); //by what quanitty todo

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