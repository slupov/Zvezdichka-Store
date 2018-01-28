using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Services.Contracts.Entity.Distributor;
using Zvezdichka.Services.Extensions;
using Zvezdichka.Web.Areas.Admin.Models.Distributor;

namespace Zvezdichka.Web.Areas.Admin.Controllers
{
    [Route("distributor/shipments/[action]")]
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
            return View(this.shipments.GetAll(x => x.Distributor, x => x.Products));
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

            var distributorShipment = this.shipments
                .Join(x => x.Distributor)
                .Join(x => x.Products)
                .SingleOrDefault(m => m.Id == id);

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
            var dbShipment = this.shipments
                .Join(x => x.Distributor)
                .SingleOrDefault(x => x.Id == distributorShipment.Id);

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