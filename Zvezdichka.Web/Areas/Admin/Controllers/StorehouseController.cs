using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class StorehouseController : AdminBaseController
    {
        private readonly IGenericDataService<Product> products;

        public StorehouseController(IGenericDataService<Product> products)
        {
            this.products = products;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.products.GetAllAsync());
        }
    }
}