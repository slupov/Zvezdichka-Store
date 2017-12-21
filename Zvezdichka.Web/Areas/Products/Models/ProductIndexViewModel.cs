using System.Collections.Generic;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductIndexViewModel
    {
        public PaginatedList<ProductListingViewModel> Products { get; set; }
        public ICollection<string> AllCategories { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
