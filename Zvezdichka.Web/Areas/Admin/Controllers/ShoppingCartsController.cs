using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class ShoppingCartsController : AdminBaseController
    {
        private readonly IShoppingCartsDataService shoppingCarts;

        public ShoppingCartsController(IShoppingCartsDataService shoppingCarts)
        {
            this.shoppingCarts = shoppingCarts;
        }

        // GET: Admin/ShoppingCarts
        public async Task<IActionResult> Index()
        {
            return View(this.shoppingCarts.GetAll());
        }

        // GET: Admin/ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = this.shoppingCarts.GetSingle(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: Admin/ShoppingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] ShoppingCart shoppingCart)
        {
            if (this.ModelState.IsValid)
            {
                this.shoppingCarts.Add(shoppingCart);
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCart);
        }

        // GET: Admin/ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = this.shoppingCarts.GetSingle(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            return View(shoppingCart);
        }

        // POST: Admin/ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.shoppingCarts.Update(shoppingCart);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCart);
        }

        // GET: Admin/ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = this.shoppingCarts.GetSingle(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: Admin/ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCart = this.shoppingCarts.GetSingle(m => m.Id == id);
            this.shoppingCarts.Remove(shoppingCart);
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return this.shoppingCarts.Any(e => e.Id == id);
        }
    }
}