using System.ComponentModel.DataAnnotations;
using Zvezdichka.Common;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Shopping.Models
{
    public class CheckoutProductsModel : IMapFrom<CartItem>, IHaveCustomMapping
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        [Range(1, 256, ErrorMessage = CommonConstants.WrongStockAmount)]
        public byte Quantity { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<CartItem, CheckoutProductsModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Product.Name))
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Product.Price));
        }
    }
}