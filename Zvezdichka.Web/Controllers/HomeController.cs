using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets;
using Zvezdichka.Web.Models;

namespace Zvezdichka.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AppKeyConfig appKeys;

        public HomeController(IOptions<AppKeyConfig> appKeys)
        {
            this.appKeys = appKeys.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier});
        }
    }
}