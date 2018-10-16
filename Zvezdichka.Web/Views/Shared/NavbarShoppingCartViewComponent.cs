using System.Linq;
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

        public IViewComponentResult Invoke()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var items = this.shoppingCartManager.GetCartItems(shoppingCartId);

            return View("NavbarShoppingCart", items.Count());
        }
    }
}