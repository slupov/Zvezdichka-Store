using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Infrastructure.Extensions;
using Zvezdichka.Web.Services;

namespace Zvezdichka.Web.Views.Shared
{
    public class NavbarShoppingCartMobileViewComponent : ViewComponent
    {
        private readonly IShoppingCartManager shoppingCartManager;

        public NavbarShoppingCartMobileViewComponent(IShoppingCartManager shoppingCartManager)
        {
            this.shoppingCartManager = shoppingCartManager;
        }

        public IViewComponentResult Invoke()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var items = this.shoppingCartManager.GetCartItems(shoppingCartId);

            return View("NavbarShoppingCartMobile", items.Count());
        }
    }
}