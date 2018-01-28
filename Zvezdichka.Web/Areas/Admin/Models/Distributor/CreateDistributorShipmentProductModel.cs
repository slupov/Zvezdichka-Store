using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Admin.Models.Distributor
{
    public class CreateDistributorShipmentProductModel : IMapFrom<DistributorShipmentProduct>
    {
        public string Name { get; set; }

        public byte Quantity { get; set; }

        public double DiscountPercentage { get; set; }

        public decimal Price { get; set; }
    }
}
