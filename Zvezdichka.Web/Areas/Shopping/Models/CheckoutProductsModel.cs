using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CheckoutProductsModel : IMapFrom<Product>
    {
        public string Name { get; set; }
        public byte Quantity  { get; set; }
    }
}
