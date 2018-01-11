using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.ValidationAttributes;
using Zvezdichka.Web.Models.Automapper;
using Zvezdichka.Web.Models.Entity;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CheckoutProductsModel : IMapFrom<Product>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [StockQuantity]
        public byte Quantity { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<Product, CheckoutProductsModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Name))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Price));
        }
    }
}