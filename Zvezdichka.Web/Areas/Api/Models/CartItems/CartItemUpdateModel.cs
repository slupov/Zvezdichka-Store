using Zvezdichka.Data.Models.ValidationAttributes;
using Zvezdichka.Web.Models.Automapper;
using Zvezdichka.Web.Models.Entity;

namespace Zvezdichka.Web.Areas.Api.Models.CartItems
{
    public class CartItemUpdateModel : IMapFrom<CartItem>
    {
        public int Id { get; set; }

        [StockQuantity]
        public byte Quantity { get; set; }
    }
}