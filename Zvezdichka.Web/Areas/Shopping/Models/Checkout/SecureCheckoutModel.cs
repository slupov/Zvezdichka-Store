//using Zvezdichka.Data.Models;
//using Zvezdichka.Web.Models.Automapper;
//
//namespace Zvezdichka.Web.Areas.Shopping.Models.Checkout
//{
//    public class SecureCheckoutModel : IMapFrom<CartItem>, IHaveCustomMapping
//    {
//        public string Name { get; set; }
//        public byte Quantity { get; set; }
//
//        public void Configure(AutoMapperProfile config)
//        {
//            config.CreateMap<CartItem, SecureCheckoutModel>()
//                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Product.Name));
//        }
//    }
//}
