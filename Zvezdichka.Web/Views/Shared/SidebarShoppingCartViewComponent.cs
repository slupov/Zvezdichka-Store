using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Infrastructure.Extensions;
using Zvezdichka.Web.Models.ShoppingViewModels;
using Zvezdichka.Web.Services;

namespace Zvezdichka.Web.Views.Shared
{
    public class SidebarShoppingCartViewComponent : ViewComponent
    {
        private readonly IProductsDataService products;
        private readonly IShoppingCartManager shoppingCartManager;

        public SidebarShoppingCartViewComponent(IProductsDataService products, IShoppingCartManager shoppingCartManager)
        {
            this.products = products;
            this.shoppingCartManager = shoppingCartManager;
        }

        public IViewComponentResult Invoke()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            var items = this.shoppingCartManager.GetCartItems(shoppingCartId);
            var itemIds = items.Select(x => x.ProductId).ToList();

            var itemQuantities = items.ToDictionary(i => i.ProductId, i => i.Quantity);

            var itemsWithDetails =
                this.products.GetAll()
                    .Where(x => itemIds.Contains(x.Id))
                    .AsQueryable()
                    .ProjectTo<CartItemViewModel>()
                    .ToList();

            itemsWithDetails.ForEach(x => x.Quantity = itemQuantities[x.Id]);

            return View("SidebarShoppingCart", itemsWithDetails);
        }
    }
}
