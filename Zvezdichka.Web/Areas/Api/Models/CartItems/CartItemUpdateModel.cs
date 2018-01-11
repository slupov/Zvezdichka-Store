using System.ComponentModel.DataAnnotations;
using Zvezdichka.Common;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.ValidationAttributes;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Api.Models.CartItems
{
    public class CartItemUpdateModel : IMapFrom<CartItem>
    {
        public int Id { get; set; }

        [StockQuantity]
        public int Quantity { get; set; }
    }
}