using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Areas.Admin.Models.Distributor;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    [Route("distributor/shipments/[action]")]
    public class DistributorShipmentsController : AdminBaseController
    {
        private readonly IGenericDataService<DistributorShipment> shipments;

        public DistributorShipmentsController(IGenericDataService<DistributorShipment> shipments)
        {
            this.shipments = shipments;
        }

        // GET: Admin/DistributorShipments
        public async Task<IActionResult> Index()
        {
            return View(await this.shipments.GetAllAsync());
        }

        // GET: Admin/DistributorShipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributorShipment = await this.shipments.GetSingleOrDefaultAsync(m => m.Id == id);

            if (distributorShipment == null)
            {
                return NotFound();
            }

            return View(distributorShipment);
        }

        // GET: Admin/DistributorShipments/Create
        public IActionResult Create()
        {
            var vm = new CreateDistributorShipmentModel()
            {
                Distributor = string.Empty,
                Products = new List<CreateDistributorShipmentProductModel>()
                {
                    new CreateDistributorShipmentProductModel()
                    {
                        DiscountPercentage = 0,
                        Name = string.Empty,
                        Price = 0,
                        Quantity = 0
                    }
                }
            };

            return View(vm);
        }

        // POST: Admin/DistributorShipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDistributorShipmentModel distributorShipment)
        {
            if (this.ModelState.IsValid)
            {
                var shipmentToAdd = new DistributorShipment()
                {
                    Date = distributorShipment.Date,
                    Distributor = new Distributor()
                    {
                        Name = distributorShipment.Distributor
                    },
                    Products = distributorShipment.Products.AsQueryable().ProjectTo<DistributorShipmentProduct>()
                        .ToList()
                };

                //todo: check how to add shipmentId
                this.shipments.Add(shipmentToAdd);

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

            var distributorShipment = await this.shipments
                .GetSingleOrDefaultAsync(m => m.Id == id);

            if (distributorShipment == null)
            {
                return NotFound();
            }

            var vm = AutoMapper.Mapper.Map<EditDistributorShipmentModel>(distributorShipment);

            return View(vm);
        }

        // POST: Admin/DistributorShipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDistributorShipmentModel distributorShipment)
        {
            var dbShipment = await this.shipments
                .GetSingleOrDefaultAsync(x => x.Id == distributorShipment.Id);

            if (dbShipment == null)
            {
                return NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    dbShipment = AutoMapper.Mapper.Map<DistributorShipment>(distributorShipment);
                    this.shipments.Update(dbShipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DistributorShipmentExists(distributorShipment.Id))
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
                .GetSingleOrDefaultAsync(m => m.Id == id);

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
            var distributorShipment = await this.shipments.GetSingleOrDefaultAsync(m => m.Id == id);

            this.shipments.Remove(distributorShipment);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DistributorShipmentExists(int id)
        {
            return await this.shipments.AnyAsync(e => e.Id == id);
        }
    }
}