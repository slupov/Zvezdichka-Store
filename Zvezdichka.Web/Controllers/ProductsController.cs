using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Models.ProductsViewModels;
using AutoMapper;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsDataService products;

        public ProductsController(IProductsDataService products)
        {
            this.products = products;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            this.products.Add(new Product()
            {
                Name = model.Name,
                Description = model.Description,
                ImageSource = model.ImageSource,
                Price = model.Price,
                Stock = model.Stock
            });

            return RedirectToAction(nameof(Index),nameof(HomeController));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, CreateProductViewModel model)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = Mapper.Map<ShortProductViewModel>(this.products.GetSingle(p => p.Id == id));

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id, ShortProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            this.products.Remove(this.products.GetSingle(p => p.Id == id));

            return RedirectToAction(nameof(Index));
        }
    }
}