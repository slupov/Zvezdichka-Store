using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Common;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Shopping.Models;
using Zvezdichka.Web.Areas.Shopping.Models.Checkout;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Shopping.Controllers
{
    public class CheckoutController : ShoppingBaseController
    {
        private readonly ICartItemsDataService cartItems;
        private readonly IProductsDataService products;

        public CheckoutController(ICartItemsDataService cartItems, IProductsDataService products)
        {
            this.cartItems = cartItems;
            this.products = products;
        }

        public async Task<IActionResult> Index(List<int> id)
        {
            var checkoutItems = this.cartItems
                .Join(x => x.Product)
                .Where(x => id.Any(y => y == x.Id))
                .AsQueryable()
                .ProjectTo<CheckoutProductsModel>();

            return View(checkoutItems);
        }

        [HttpPost]
        public async Task<IActionResult> SecureCheckout(List<string> name, List<byte> quantity)
        {
            var dbProducts = this.products.GetList(x => name.Contains(x.Name));

            string errorMsg = string.Empty;
            bool hasErrors = false;

            //check for errors
            for (int i = 0; i < name.Count; i++)
            {
                var product = dbProducts.FirstOrDefault(x => x.Name == name[i]);

                if (product == null)
                {
                    return NotFound();
                }

                if (product.Stock < quantity[i])
                {
                    hasErrors = true;
                    errorMsg += string.Format(CommonConstants.StockAmountExceededForError, name[i]) +
                                Environment.NewLine;
                }
            }

            if (hasErrors)
            {
                Danger(errorMsg);
                return RedirectToAction(nameof(Index), new {id = dbProducts.Select(x => x.Id).ToList()});
            }

            //secure checkout

            for (int i = 0; i < name.Count; i++)
            {
                var product = dbProducts.FirstOrDefault(x => x.Name == name[i]);
                product.Stock -= quantity[i];

            }

            this.products.Update(dbProducts.ToArray());
            this.cartItems.Remove(this.cartItems.GetAll().ToArray());

            Success(WebConstants.CheckoutSecured);
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}