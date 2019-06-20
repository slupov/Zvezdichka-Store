using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Infrastructure.Extensions;
using Zvezdichka.Web.Services;

namespace Zvezdichka.Web.Views.Shared
{
    public class NavbarShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartManager shoppingCartManager;

        public NavbarShoppingCartViewComponent(IShoppingCartManager shoppingCartManager)
        {
            this.shoppingCartManager = shoppingCartManager;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var items = this.shoppingCartManager.GetCartItems(shoppingCartId);

            var result = (IViewComponentResult) View("NavbarShoppingCart", items.Count());
            return Task.FromResult(result);
        }
    }
}