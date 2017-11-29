using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductDeleteViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }
    }
}
