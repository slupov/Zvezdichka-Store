using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Controllers;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Products.Controllers
{
    [Area(WebConstants.Areas.ProductsArea)]
    public abstract class ProductsBaseController : BaseController
    {
    }
}