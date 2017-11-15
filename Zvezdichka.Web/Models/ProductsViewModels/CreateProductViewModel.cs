using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Models.ProductsViewModels
{
    public class CreateProductViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte Stock { get; set; }

        public decimal Price { get; set; }

        public string ImageSource { get; set; }
    }
}
