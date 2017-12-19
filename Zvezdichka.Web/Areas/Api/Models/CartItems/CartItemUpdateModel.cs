using System.ComponentModel.DataAnnotations;
using Zvezdichka.Common;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Api.Models.CartItems
{
    public class CartItemUpdateModel : IMapFrom<CartItem>
    {
        public int Id { get; set; }

        [Range(1, 256, ErrorMessage = CommonConstants.WrongStockAmount)]
        public int Quantity { get; set; }
    }
}