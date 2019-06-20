using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Infrastructure.Extensions;
using Zvezdichka.Web.Models.ShoppingViewModels;
using Zvezdichka.Web.Services;

namespace Zvezdichka.Web.Views.Shared
{
    public class SidebarShoppingCartViewComponent : ViewComponent
    {
        private readonly IGenericDataService<Product> products;
        private readonly IShoppingCartManager shoppingCartManager;

        public SidebarShoppingCartViewComponent(IGenericDataService<Product> products, IShoppingCartManager shoppingCartManager)
        {
            this.products = products;
            this.shoppingCartManager = shoppingCartManager;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            var items = this.shoppingCartManager.GetCartItems(shoppingCartId);
            var itemIds = items.Select(x => x.ProductId).ToList();

            var itemQuantities = items.ToDictionary(i => i.ProductId, i => i.Quantity);

            var itemsWithDetails =
                (this.products.GetListAsync(x => itemIds.Contains(x.Id)).GetAwaiter().GetResult())
                    .AsQueryable()
                    .ProjectTo<CartItemViewModel>()
                    .ToList();

            itemsWithDetails.ForEach(x => x.Quantity = itemQuantities[x.Id]);

            var result = (IViewComponentResult)View("SidebarShoppingCart", itemsWithDetails);

            return Task.FromResult(result);
        }
    }
}
