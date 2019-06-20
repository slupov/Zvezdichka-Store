using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Areas.Api.Models.Products;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IGenericDataService<Product> products;

        public ProductsController(IGenericDataService<Product> products)
        {
            this.products = products;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ProductFilterModel filtration)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            var filteredProducts = (await this.products
                .GetAllAsync())
                .AsQueryable()
                .ProjectTo<ProductListingViewModel>()
                .ToList();


            //apply categories filtration
            if (filtration.Categories != null)
            {
                filteredProducts = filteredProducts
                    .Where(x => filtration.Categories.Any(y => x.Categories.Any(c => c == y)))
                    .ToList();
            }

            //apply price filtration
            filteredProducts = filteredProducts
                .Where(x => x.Price >= filtration.MinPrice && x.Price <= filtration.MaxPrice)
                .ToList();


            //apply order by filtration
            switch (filtration.OrderBy)
            {
                case WebConstants.OrderBy.NameAsc:
                    filteredProducts = filteredProducts.OrderBy(x => x.Name).ToList();
                    break;
                case WebConstants.OrderBy.NameDesc:
                    filteredProducts = filteredProducts.OrderByDescending(x => x.Name).ToList();
                    break;
                case WebConstants.OrderBy.PriceAsc:
                    filteredProducts = filteredProducts.OrderBy(x => x.Price).ToList();
                    break;
                case WebConstants.OrderBy.PriceDesc:
                    filteredProducts = filteredProducts.OrderByDescending(x => x.Price).ToList();
                    break;
            }

            return Ok(filteredProducts);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeThumbnail(string productName, string newThumbnailSource)
        {
            var product = await this.products.GetSingleOrDefaultAsync(x => x.Name == productName);

            if (product == null)
            {
                return NotFound();
            }

            product.ThumbnailSource = newThumbnailSource;
            this.products.Update(product);

            return Ok();
        }
    }
}