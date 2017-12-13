using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CartItemListingViewModel : IMapFrom<CartItem>, IHaveCustomMapping
    {
        public byte Quantity { get; set; }
        public string ProductName { get; set; }
        public string ThumbnailSource { get; set; }
        public decimal Price { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config
                .CreateMap<CartItem, CartItemListingViewModel>()
                .ForMember(civ => civ.ProductName, cfg => cfg.MapFrom(c => c.Product.Name))
                .ForMember(civ => civ.ThumbnailSource, cfg => cfg.MapFrom(c => c.Product.ThumbnailSource))
                .ForMember(civ => civ.Price, cfg => cfg.MapFrom(c => c.Product.Price));
        }
    }
}