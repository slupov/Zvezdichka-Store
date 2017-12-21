using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Contracts.Entity.Mapping;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Api.Models.Products;
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
        private readonly IOptions<AppKeyConfig> appKeys;

        public HomeController(IProductsDataService products, ICategoriesDataService categories,
            ICategoryProductsDataService categoryProducts, IHtmlService html,
            IOptions<AppKeyConfig> appKeys)
        {
            this.products = products;
            this.categories = categories;
            this.appKeys = appKeys;
            this.categoryProducts = categoryProducts;
            this.html = html;
        }

        //try making it post with another method, get the data and redirect to httpget index
        [HttpGet]
        public async Task<IActionResult> Index(string searchString,
            int? page,
            int pageSize = 20)
        {
            var filter = this.TempData.Get<ProductFilterModel>("ProductFilter");

            var filtered = this.products.Join(x => x.Categories).ThenJoin(x => x.Category)
                .AsQueryable()
                .ProjectTo<ProductListingViewModel>()
                .ToList();

            ProductIndexViewModel vm = new ProductIndexViewModel()
            {
                MinPrice = filtered.Min(x => x.Price),
                MaxPrice = filtered.Max(x => x.Price)
            };

            if (filter != null)
            {
                //apply filter by price
                filtered = filtered
                    .Where(x => x.Price >= filter.MinPrice && x.Price <= filter.MaxPrice)
                    .ToList();

                //apply filter by categories
                if (filter.Categories != null && filter.Categories.Count != 0)
                {
                    filtered = filtered.Where(x => x.Categories.Any(y => filter.Categories.Contains(y))).ToList();
                }

                //apply order by filtration
                switch (filter.OrderBy)
                {
                    case WebConstants.OrderBy.NameAsc:
                        filtered = filtered.OrderBy(x => x.Name).ToList();
                        break;
                    case WebConstants.OrderBy.NameDesc:
                        filtered = filtered.OrderByDescending(x => x.Name).ToList();
                        break;
                    case WebConstants.OrderBy.PriceAsc:
                        filtered = filtered.OrderBy(x => x.Price).ToList();
                        break;
                    case WebConstants.OrderBy.PriceDesc:
                        filtered = filtered.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                this.ViewData["SearchFilter"] = searchString;
                filtered = filtered.Where(x => x.Name.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            vm.Products = PaginatedList<ProductListingViewModel>.Create(filtered, page ?? 1, pageSize);

            if (filter != null)
            {
                Success("Successfully filtered products.");
            }
            return View(vm);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost(ProductFiltrationModel filtered)
        {
            var prices = filtered.PriceRange.Split(",").Select(decimal.Parse).ToArray();

            ProductFilterModel filter = new ProductFilterModel()
            {
                Categories = filtered.Categories,
                MinPrice = prices[0],
                MaxPrice = prices[1],
                OrderBy = filtered.OrderBy
            };

            this.TempData.Put("ProductFilter", filter);
            return RedirectToAction(nameof(Index));
        }

        // GET: www.zvezdichka.com/big-toy-1
        public async Task<IActionResult> Details(int? id, string title, int? commentsPageIndex = 1)
        {
            if (id == null)
                return NotFound();

            var dbProduct = this.products
                .Join(x => x.Comments).ThenJoin(x => x.User)
                .Join(x => x.ImageSources)
                .Join(x => x.Categories).ThenJoin(c => c.Category)
                .FirstOrDefault(x => x.Id == id);

            if (dbProduct == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductDetailsViewModel>(dbProduct);
            product.Comments =
                PaginatedList<Comment>.Create(dbProduct.Comments.OrderByDescending(x => x.DateAdded).ToList(),
                    commentsPageIndex ?? 1, 7);

            product.CloudinarySources = await ListCloudinaryFileNamesAsync(product.Name);
            product.Cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

            var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Name);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
                return RedirectToAction(nameof(Details),
                    new {id = id, title = friendlyTitle, commentsPageIndex = commentsPageIndex});

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categs = this.categories.GetAll().Select(x => x.Name).ToList();

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

            product.CloudinarySources = await ListCloudinaryFileNamesAsync(product.Name);
            product.Cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

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
            dbProduct.Price = product.Price;
            dbProduct.Stock = product.Stock;

            //update model categories
            var dbCategories = this.categoryProducts.Join(x => x.Category).Where(x => x.ProductId == dbProduct.Id)
                .ToList();


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
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

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
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

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
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

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
            var cloudinary = CloudinaryExtensions.GetCloudinary(this.appKeys.Value);

            return await Task.Run(() =>
            {
                return cloudinary
                    .ListResources()
                    .Resources
                    .Where(x => x.PublicId.StartsWith(name) &&
                                x.StatusCode !=
                                HttpStatusCode.NotFound) //todo: filtration still doesnt work like that in this api
                    .Select(x => x.PublicId)
                    .ToList();
            });
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Warning("You are about to delete an item !!!");
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