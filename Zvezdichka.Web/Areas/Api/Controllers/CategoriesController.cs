using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Controllers.Extensions;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CategoriesController : ApiBaseController
    {
        private Zvezdichka.Services.Contracts.IGenericDataService<Category> categories;

        public CategoriesController(IGenericDataService<Category> categories)
        {
            this.categories = categories;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = (await this.categories
                .GetAllAsync())
                .Select(x => new {name = x.Name, products = x.Products.Count});

            return this.OkOrNotFound(categories);
        }
    }
}