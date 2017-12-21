using System.Collections.Generic;
using System.Linq;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductListingViewModel : IMapFrom<Product>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailSource { get; set; }
        public decimal Price { get; set; }
        public byte Stock { get; set; }

        public ICollection<string> Categories { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<Product, ProductListingViewModel>()
                .ForMember(x => x.Categories, cfg => cfg.MapFrom(y => y.Categories.Select(c => c.Category.Name)));
        }
    }
}