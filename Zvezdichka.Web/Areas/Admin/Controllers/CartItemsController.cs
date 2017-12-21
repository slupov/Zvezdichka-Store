using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Controllers;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class CartItemsController : BaseController
    {
        private readonly ICartItemsDataService cartItems;

        public CartItemsController(ICartItemsDataService cartItems)
        {
            this.cartItems = cartItems;
        }

        public async Task<IActionResult> Index(string username)
        {
            var userCarts = this.cartItems.GetAll(x => x.Product, x => x.User);

            //populate list
            if (!string.IsNullOrEmpty(username))
            {
                userCarts = userCarts.Where(x => x.User.UserName.ToLower() == username.ToLower())
                    .ToList();
            }


            return View();
        }
    }
}