using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Controllers
{
    [Route("faq/[action]")]
    public class FaqsController : BaseController
    {
        private readonly IGenericDataService<Faq> faqs;

        public FaqsController(IGenericDataService<Faq> faqs)
        {
            this.faqs = faqs;
        }

        // GET: Faqs
        public async Task<IActionResult> Index()
        {
            return View(await this.faqs.GetAllAsync());
        }

        // GET: Faqs/Details/5
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await this.faqs.GetSingleOrDefaultAsync(m => m.Id == id);

            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // GET: Faqs/Create
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faq faq)
        {
            if (this.ModelState.IsValid)
            {
                this.faqs.Add(faq);
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        // GET: Faqs/Edit/5
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await this.faqs.GetSingleOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public async Task<IActionResult> Edit(int id, Faq faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.faqs.Update(faq);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FaqExistsAsync(faq.Id))
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
            return View(faq);
        }

        // GET: Faqs/Delete/5
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await this.faqs.GetSingleOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WebConstants.RoleNames.AdminRole + ", " + WebConstants.RoleNames.ManagerRole)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await this.faqs.GetSingleOrDefaultAsync(m => m.Id == id);
            this.faqs.Remove(faq);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> FaqExistsAsync(int id)
        {
            return await this.faqs.AnyAsync(e => e.Id == id);
        }
    }
}