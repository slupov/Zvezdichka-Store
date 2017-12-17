using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductCreateModel : IMapFrom<Product>, IHaveCustomMapping
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public byte Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ThumbnailSource { get; set; }

        public ICollection<string> Categories { get; set; } = new HashSet<string>();

        public void Configure(AutoMapperProfile cfg)
        {
            cfg.CreateMap<Product, ProductCreateModel>()
                .ForMember(x => x.Categories, m => m.MapFrom(c => c.Categories.Select(x => x.Category.Name)));
        }
    }
}
