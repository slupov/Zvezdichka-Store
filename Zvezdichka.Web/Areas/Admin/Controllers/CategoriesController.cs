using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Admin.Models;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoriesDataService categories;

        public CategoriesController(ICategoriesDataService categories, IProductsDataService products)
        {
            this.categories = categories;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(this.categories.GetAll());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = this.categories.GetSingle(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(EditCategoryModel category)
        {
            var dbCategory = this.categories.GetSingle(x => x.Name == category.Name);

            if (dbCategory != null)
            {
                Danger(WebConstants.SuchCategoryExists);
                return RedirectToAction(nameof(Create));
            }

            if (this.ModelState.IsValid)
            {
                var categoryToAdd = new Category()
                {
                    Name = category.Name
                };

                this.categories.Add(categoryToAdd);
                Success(string.Format(WebConstants.CategoryCreated, category.Name));
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = this.categories.GetSingle(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id, EditCategoryModel category)
        {
            var dbCategory = this.categories.GetSingle(x => x.Name == category.Name);

            if (dbCategory == null)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                dbCategory.Name = category.Name;

                this.categories.Update(dbCategory);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = this.categories.GetSingle(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = this.categories.GetSingle(m => m.Id == id);
            this.categories.Remove(category);

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return this.categories.Any(e => e.Id == id);
        }
    }
}