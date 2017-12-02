using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CartItemListingViewModel : IMapFrom<CartItem>, IHaveCustomMapping
    {
        public byte Quantity { get; set; }
        public string ProductName { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config
                .CreateMap<CartItem, CartItemListingViewModel>()
                .ForMember(civ => civ.ProductName, cfg => cfg.MapFrom(c => c.Product.Name));
        }
    }
}