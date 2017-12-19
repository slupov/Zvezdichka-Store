using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductEditViewModel : IMapFrom<Product>, IHaveCustomMapping
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Stock { get; set; }
        public decimal Price { get; set; }

        public ICollection<string> CloudinarySources { get; set; }
        public Cloudinary Cloudinary { get; set; }

        public ICollection<string> Categories { get; set; } = new HashSet<string>();

        public void Configure(AutoMapperProfile cfg)
        {
            cfg.CreateMap<Product, ProductCreateModel>()
                .ForMember(x => x.Categories, m => m.MapFrom(c => c.Categories.Select(x => x.Category.Name)));
        }
    }
}