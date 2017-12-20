using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Shopping.Models;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;

namespace Zvezdichka.Web.Areas.Shopping.Controllers
{
    public class CheckoutController : ShoppingBaseController
    {
        private readonly ICartItemsDataService cartItems;
        public CheckoutController(ICartItemsDataService cartItems)
        {
            this.cartItems = cartItems;
        }

        public IActionResult Index(List<int> id)
        {
            var checkoutItems = this.cartItems
                .Join(x => x.Product)
                .Where(x => id.Any(y => y == x.Id))
                .AsQueryable()
                .ProjectTo<CheckoutProductsModel>();

            return View(checkoutItems);
        }

        private void GenerateInvoicePdf(List<int> cartItemIds)
        {
            var checkoutItems = this.cartItems
                .Join(x => x.Product)
                .Where(x => cartItemIds.Any(y => y == x.Id))
                .AsQueryable()
                .ProjectTo<CheckoutProductsModel>();


        }
    }
}
