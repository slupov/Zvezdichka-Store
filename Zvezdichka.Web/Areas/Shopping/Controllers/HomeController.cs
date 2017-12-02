using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Extensions.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Shopping.Models;
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
            //            var user = await this.users.FindByNameAsync(this.User.Identity.Name, 
            //                u => u.CartItems,
            //                u => u.CartItems.Select(ci => ci.Product));

            var user = await this.users.FindByNameAsync(this.User.Identity.Name,
                u => u.CartItems);

            var userCartItems = user.CartItems.AsQueryable().ToList();

            //include the product
            //todo: too many queries, change with expression tree includes
            foreach (var cartItem in userCartItems)
            {
                cartItem.Product = this.products.GetSingle(p => p.Id == cartItem.Id);
            }


            //see usercartitems
            return View((userCartItems.AsQueryable().ProjectTo<CartItemListingViewModel>()));
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
                this.cartItems.GetSingle(c => c.User.UserName == this.User.Identity.Name && c.Product.Name == title,
                    c => c.User, c => c.Product);

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

            this.cartItems.Add(cartProduct);
//            user.CartItems.Add(cartProduct); //by what quanitty todo

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