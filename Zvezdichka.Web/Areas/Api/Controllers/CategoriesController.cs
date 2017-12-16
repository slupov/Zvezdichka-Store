using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Controllers.Extensions;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CategoriesController : ApiBaseController
    {
        private ICategoriesDataService categories;

        public CategoriesController(ICategoriesDataService categories)
        {
            this.categories = categories;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = this.categories
                .GetAll(x => x.Products)
                .Select(x => new {name = x.Name, products = x.Products.Count});

            return this.OkOrNotFound(categories);
        }
    }
}