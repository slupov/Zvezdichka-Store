using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class StorehouseController : AdminBaseController
    {
        private readonly IProductsDataService products;

        public StorehouseController(IProductsDataService products)
        {
            this.products = products;
        }

        public async Task<IActionResult> Index()
        {
            return View(this.products.GetAll());
        }
    }
}
