using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Extensions.SEO;
using Zvezdichka.Web.Extensions.Helpers;

namespace Zvezdichka.Web.Areas.Products.Controllers
{
//    [Route("")] //could be called with no controller part supplied in route too
    //GET: Products/
    //GET: /
    public class HomeController : ProductsBaseController
    {
        private readonly IProductsDataService products;

        public HomeController(IProductsDataService products)
        {
            this.products = products;
        }

        public async Task<IActionResult> Index()
        {
            return View(this.products.GetAll());
        }

        // GET: Products/Details/5
        [HttpGet("{title}-{id}", Name =
            WebConstants.DefaultAreaControllerActionRoutingName)] // GET: products/big-shoes-20125 todo
        [HttpGet("{category}/{title}-{id}", Name =
            WebConstants.ProductWithCategoryRoutingName)] // GET: products/big-shoes-20125 todo
        public async Task<IActionResult> Details(int? id, string title, string category)
        {
            if (id == null)
                return NotFound();

            var product = this.products.GetSingle(m => m.Id == id);
            if (product == null)
                return NotFound();

            var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Name);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    return RedirectToRoutePermanent(
                        string.Format(WebConstants.ThreePartRoutingName, "products", "home", "details"),
                        new {id = id, title = friendlyTitle});
                }

                //return url with the category inside
                return RedirectToRoutePermanent(
                    WebConstants.ProductWithCategoryRoutingName,
                    new {id = id, title = friendlyTitle, category = category});
            }


            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Stock,Price,ImageSource")] Product product)
        {
            if (this.ModelState.IsValid)
            {
                this.products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = this.products.GetSingle(m => m.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Description,Stock,Price,ImageSource")] Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.products.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = this.products.GetSingle(m => m.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = this.products.GetSingle(m => m.Id == id);
            this.products.Remove(product);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return this.products.Any(e => e.Id == id);
        }
    }
}