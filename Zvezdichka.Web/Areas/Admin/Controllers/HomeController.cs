using Microsoft.AspNetCore.Mvc;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}