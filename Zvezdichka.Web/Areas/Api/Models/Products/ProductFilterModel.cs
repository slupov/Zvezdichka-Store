using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Web.Areas.Api.Models.Products
{
    public class ProductFilterModel
    {
        [Required]
        public decimal MinPrice { get; set; }

        [Required]
        public decimal MaxPrice { get; set; }
        public ICollection<string> Categories { get; set; }
        public WebConstants.OrderBy OrderBy { get; set; }
    }
}