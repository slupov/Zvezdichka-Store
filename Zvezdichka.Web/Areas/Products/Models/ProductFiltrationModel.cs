using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductFiltrationModel
    {
        [Required]
        public string PriceRange { get; set; }

        public WebConstants.OrderBy OrderBy { get; set; }
        public ICollection<string> Categories { get; set; }
    }
}
