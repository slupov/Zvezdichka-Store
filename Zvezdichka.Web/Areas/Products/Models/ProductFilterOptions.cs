using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductFilterOptions : IMapFrom<Product>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}