using System.Collections.Generic;
using System.Linq;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ImageSource> ImageSources { get; set; }
        public ICollection<string> Categories { get; set; }
        public PaginatedList<Comment> Comments { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config
                .CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(pdv => pdv.IsInStock, cfg => cfg.MapFrom(c => c.Stock > 0))
                .ForMember(x => x.Categories, m => m.MapFrom(c => c.Categories.Select(x => x.Category.Name)));
//                .ForMember(x => x.Comments, m => m.MapFrom(c => PaginatedList<Comment>.Create(c.Comments.ToList(), 1, 7)));

        }
    }
}