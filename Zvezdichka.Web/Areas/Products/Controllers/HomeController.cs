using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Actions;
using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Contracts.Entity.Mapping;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Constants;
using Zvezdichka.Web.Infrastructure.Extensions.Cloud;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Html;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets;
using Zvezdichka.Web.Infrastructure.Extensions.SEO;

namespace Zvezdichka.Web.Areas.Products.Controllers
{
    public class HomeController : ProductsBaseController
    {
        private readonly IProductsDataService products;
        private readonly ICategoriesDataService categories;
        private readonly ICategoryProductsDataService categoryProducts;
        private readonly IHtmlService html;
        private readonly AppKeyConfig appKeys;

        public HomeController(IProductsDataService products, ICategoriesDataService categories,
            ICategoryProductsDataService categoryProducts, IHtmlService html,
            IOptions<AppKeyConfig> appKeys)
        {
            this.products = products;
            this.categories = categories;
            this.appKeys = appKeys.Value;
            this.categoryProducts = categoryProducts;
            this.html = html;
        }

        //try making it post with another method, get the data and redirect to httpget index
        [HttpGet]
        public async Task<IActionResult> Index(string searchString,
            int? page,
            int pageSize = 20)
        {
            this.ModelState.Clear();
            var filtered = this.TempData.Get<List<ProductListingViewModel>>("FilteredProducts");

            if (filtered == null)
            {
                filtered = this.products.Join(x => x.Categories).ThenJoin(x => x.Category)
                    .AsQueryable()
                    .ProjectTo<ProductListingViewModel>()
                    .ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                this.ViewData["SearchFilter"] = searchString;
                filtered = filtered.Where(x => x.Name.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            return View(PaginatedList<ProductListingViewModel>.Create(filtered, page ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] List<ProductListingViewModel> filtered)
        {
            this.TempData.Put("FilteredProducts", filtered);
            return RedirectToAction(nameof(Index));
        }

        // GET: www.zvezdichka.com/big-toy-1
        public async Task<IActionResult> Details(int? id, string title)
        {
            if (id == null)
                return NotFound();

            var dbProduct = this.products
                .Join(x => x.Comments).ThenJoin(x => x.User)
                .Join(x => x.ImageSources)
                .Join(x => x.Categories).ThenJoin(c => c.Category)
                .FirstOrDefault(x => x.Id == id);

            var product = Mapper.Map<ProductDetailsViewModel>(dbProduct);

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
            ProductCreateModel vm = new ProductCreateModel()
            {
                Categories = this.categories.GetAll().Select(x => x.Name).ToList()
            };

            return View(vm);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateModel product)
        {
            if (!this.ModelState.IsValid)
            {
                return View(nameof(Create));
            }

            var productToAdd = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                ThumbnailSource = product.ThumbnailSource,
                Price = product.Price,
                Stock = product.Stock,
            };

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            var categoriesToAdd = this.categories.GetList(x => product.Categories.Contains(x.Name));

            //add product to database
            this.products.Add(productToAdd);
            var dbProduct = this.products.GetSingle(x => x.Name == product.Name);

            //add categories to the database product
            foreach (var category in categoriesToAdd)
            {
                categoryProducts.Add(new CategoryProduct()
                {
                    ProductId = dbProduct.Id,
                    Category = category
                });
            }

            this.categoryProducts.Add(categoryProducts.ToArray());

            return RedirectToAction(nameof(Details), new {id = dbProduct.Id, title = dbProduct.Name});
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id, string title)
        {
            if (id == null)
                return NotFound();

            var product =
                Mapper.Map<ProductEditViewModel>(this.products.GetSingle(m => m.Id == id, m => m.ImageSources));

            product.Categories = this.categories.GetAll().Select(x => x.Name).ToList();

            //TODO
            product.CloudinarySources = await ListCloudinaryFileNamesAsync(product.Name);
            product.Cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

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
        public async Task<IActionResult> Edit(int id, string oldName, ProductEditViewModel product)
        {
            var dbProduct = this.products.GetSingle(x => x.Name == oldName);

            if (dbProduct == null || id != dbProduct.Id)
            {
                return NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return View(product);
            }

            //update model
            dbProduct.Name = product.Name;
            dbProduct.Description = this.html.Sanitize(product.Description);
            dbProduct.ImageSources = product.ImageSources;
            dbProduct.Price = product.Price;
            dbProduct.Stock = product.Stock;
            dbProduct.ThumbnailSource = product.ThumbnailSource;

            //update model categories
            var dbCategories = this.categoryProducts.Join(x => x.Category).Where(x => x.ProductId == dbProduct.Id).ToList();


            //remove unnecessary categories
            foreach (var dbCategory in dbCategories.Where(x => x.ProductId == dbProduct.Id))
            {
                //dbCategory is not in currently selected
                if (!product.Categories.Contains(dbCategory.Category.Name))
                {
                    this.categoryProducts.Remove(dbCategory);
                }
            }

            //add selected categories
            foreach (var productCategory in product.Categories)
            {
                var dbCategory = dbCategories
                    .FirstOrDefault(x => x.Category.Name == productCategory);
                
                //product is not in this category
                if (dbCategory == null)
                {
                    this.categoryProducts.Add(new CategoryProduct()
                    {
                        ProductId = dbProduct.Id,
                        CategoryId = this.categories.GetSingle(x => x.Name == productCategory).Id
                    });
                }
            }

            try
            {
                this.products.Update(dbProduct);
                RenameCloudinaryFolderAsync(oldName, product.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(dbProduct.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToRoute(WebConstants.Routes.ProductDetails, new {id = dbProduct.Id, title = dbProduct.Name});
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
                    cloudinary.Rename(resource.PublicId, resource.PublicId.Replace(oldName, newName), true);
            });
        }

        [HttpDelete]
        private async Task DeleteCloudinaryFolderAsync(string folder)
        {
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

            Task.Run(() =>
            {
                var toDelete = cloudinary
                    .ListResources()
                    .Resources
                    .Where(x => x.PublicId.StartsWith(folder))
                    .Select(x => x.PublicId)
                    .ToList();

                cloudinary.DeleteResources(new DelResParams()
                {
                    Invalidate = true,
                    PublicIds = toDelete,
                    ResourceType = ResourceType.Image
                });
            });
        }

        [HttpDelete]
        public async Task DeleteCloudinaryFileAsync(string name)
        {
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

            Task.Run(() =>
            {
                var toDelete = cloudinary
                    .ListResources()
                    .Resources
                    .Select(x => x.PublicId)
                    .FirstOrDefault(x => x == name);

                cloudinary.Destroy(new DeletionParams(toDelete)
                {
                    Invalidate = true,
                    ResourceType = ResourceType.Image
                });
            });
        }

        private async Task<List<string>> ListCloudinaryFileNamesAsync(string name)
        {
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys);

            return await Task.Run(() =>
            {
                return cloudinary
                    .ListResources()
                    .Resources
                    .Where(x => x.PublicId.StartsWith(name))
                    .Select(x => x.PublicId)
                    .ToList();
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

            DeleteCloudinaryFolderAsync(product.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return this.products.Any(e => e.Id == id);
        }
    }
}