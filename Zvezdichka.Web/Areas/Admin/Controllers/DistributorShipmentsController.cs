using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Services.Contracts.Entity.Distributor;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    public class DistributorShipmentsController : AdminBaseController
    {
        private readonly IDistributorShipmentsDataService shipments;

        public DistributorShipmentsController(IDistributorShipmentsDataService shipments)
        {
            this.shipments = shipments;
        }

        // GET: Admin/DistributorShipments
        public async Task<IActionResult> Index()
        {
            return View(this.shipments.GetAll(x => x.Distributor));
        }

        // GET: Admin/DistributorShipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributorShipment = await this.shipments
                .Join(d => d.Distributor)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (distributorShipment == null)
            {
                return NotFound();
            }

            return View(distributorShipment);
        }

        // GET: Admin/DistributorShipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DistributorShipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistributorShipment distributorShipment)
        {
            if (this.ModelState.IsValid)
            {
                this.shipments.Add(distributorShipment);
                return RedirectToAction(nameof(Index));
            }

            return View(distributorShipment);
        }

        // GET: Admin/DistributorShipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributorShipment = this.shipments.GetSingle(m => m.Id == id);
            if (distributorShipment == null)
            {
                return NotFound();
            }

            return View(distributorShipment);
        }

        // POST: Admin/DistributorShipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DistributorShipment distributorShipment)
        {
            if (id != distributorShipment.Id)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.shipments.Update(distributorShipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributorShipmentExists(distributorShipment.Id))
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

            return View(distributorShipment);
        }

        // GET: Admin/DistributorShipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributorShipment = await this.shipments
                .Join(d => d.Distributor)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (distributorShipment == null)
            {
                return NotFound();
            }

            return View(distributorShipment);
        }

        // POST: Admin/DistributorShipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distributorShipment = this.shipments.GetSingle(m => m.Id == id);
            this.shipments.Remove(distributorShipment);
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorShipmentExists(int id)
        {
            return this.shipments.Any(e => e.Id == id);
        }
    }
}