using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.ValidationAttributes;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CheckoutProductsModel : IMapFrom<CartItem>, IHaveCustomMapping
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        [StockQuantity]
        public byte Quantity { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<CartItem, CheckoutProductsModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Product.Name))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Product.Price));
        }
    }
}