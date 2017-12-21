using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Admin.Models.CartItems
{
    public class AdminCartItemsModel : IMapFrom<CartItem>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public Dictionary<string,byte> ProductQuantities { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public void Configure(AutoMapperProfile config)
        {
        }
    }
}
