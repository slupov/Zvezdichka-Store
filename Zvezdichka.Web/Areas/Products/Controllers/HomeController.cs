using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Extensions.Cloud;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets;
using Zvezdichka.Web.Infrastructure.Extensions.SEO;

namespace Zvezdichka.Web.Areas.Products.Controllers
{
    public class HomeController : ProductsBaseController
    {
        private readonly IProductsDataService products;
        private readonly AppKeyConfig appKeys;

        public HomeController(IProductsDataService products, IOptions<AppKeyConfig> appKeys)
        {
            this.products = products;
            this.appKeys = appKeys.Value;
        }

        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            if (searchString != null)
            {
                searchString = searchString.ToLower();
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;


            var productsList = this.products.GetAll()
                .AsQueryable()
                .ProjectTo<ProductListingViewModel>()
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
                productsList = productsList
                    .Where(p => p.Name.ToLower().Contains(searchString) ||
                                p.Categories.Any(c => c.Category.Name.ToLower().Contains(searchString)))
                    .ToList();

            switch (sortOrder)
            {
                case "name_desc":
                    productsList = productsList.OrderByDescending(s => s.Name).ToList();
                    break;
                default:
                    productsList = productsList.OrderBy(p => p.Name).ToList();
                    break;
            }

            var pageSize = 20;
            return View(PaginatedList<ProductListingViewModel>.Create(productsList, page ?? 1, pageSize));
        }

        // GET: www.zvezdichka.com/big-toy-1
        public async Task<IActionResult> Details(int? id, string title)
        {
            if (id == null)
                return NotFound();

            var product =
                Mapper.Map<ProductDetailsViewModel>(this.products.GetSingle(m => m.Id == id,
                    m => m.ImageSources)); //eager loading image sources

            if (product == null)
                return NotFound();

            var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Name);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
                return RedirectToAction(nameof(Details),
                    new {id = id, title = friendlyTitle});

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
        public async Task<IActionResult> Create(Product product)
        {
            if (this.ModelState.IsValid)
            {
                this.products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id, string title)
        {
            if (id == null)
                return NotFound();

            var product =
                Mapper.Map<ProductEditViewModel>(this.products.GetSingle(m => m.Id == id, m => m.ImageSources));

            if (product == null)
                return NotFound();

            var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Name);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
                return RedirectToAction(nameof(Edit),
                    new {id = id, title = friendlyTitle});

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string oldName, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.products.Update(product);
                    RenameCloudinaryFolderAsync(oldName, product.Name);
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
            return View(Mapper.Map<ProductEditViewModel>(product));
        }

        /// <summary>
        /// Renames Cloudinary item's folder asynchronously
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        private async Task RenameCloudinaryFolderAsync(string oldName, string newName)
        {
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

            await Task.Run(() =>
            {
                var toRename = cloudinary
                    .ListResources()
                    .Resources
                    .Where(x => x.PublicId.StartsWith(oldName))
                    .ToList();

                foreach (var resource in toRename)
                {
                    cloudinary.Rename(resource.PublicId, resource.PublicId.Replace(oldName, newName));
                }
            });
        }

        private async Task DeleteCloudinaryFiles(string name)
        {
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

            await Task.Run(() =>
            {
                var toDelete = cloudinary
                    .ListResources()
                    .Resources
                    .Where(x => x.PublicId.StartsWith(name))
                    .Select(x => x.PublicId).ToArray();


                cloudinary.DeleteResources(ResourceType.Image, toDelete);
            });
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = Mapper.Map<ProductDeleteViewModel>(this.products.GetSingle(m => m.Id == id));
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

            DeleteCloudinaryFiles(product.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return this.products.Any(e => e.Id == id);
        }
    }
}