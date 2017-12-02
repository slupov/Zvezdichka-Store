using System.Collections.Generic;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductListingViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailSource { get; set; }
        public decimal Price { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<CategoryProduct> Categories { get; set; }
    }
}
