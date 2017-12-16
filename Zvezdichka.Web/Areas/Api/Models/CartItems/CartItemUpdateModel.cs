using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Api.Models.CartItems
{
    public class CartItemUpdateModel : IMapFrom<CartItem>
    {
        public int Id { get; set; }

        [Range(1, 256)]
        public byte Quantity { get; set; }
    }
}
