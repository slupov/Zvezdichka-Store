using System.Diagnostics;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets;
using Zvezdichka.Web.Models;
using Zvezdichka.Web.Infrastructure.Extensions.Cloud;

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
//            var uploadParams = new ImageUploadParams()
//            {
//                File = new FileDescription(@"https://i.ytimg.com/vi/SfLV8hD7zX4/maxresdefault.jpg")
//            };
//
//            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);
//            var uploadResult = cloudinary.Upload(uploadParams);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}