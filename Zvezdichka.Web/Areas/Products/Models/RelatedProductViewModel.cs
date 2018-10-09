using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class RelatedProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailSource { get; set; }
    }
}
