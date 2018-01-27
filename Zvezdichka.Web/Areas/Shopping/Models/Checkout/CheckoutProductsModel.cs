using Zvezdichka.Data.EntityConfigurations.ValidationAttributes;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models.Checkout
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