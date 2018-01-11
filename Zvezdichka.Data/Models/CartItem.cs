using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models.ValidationAttributes;

namespace Zvezdichka.Data.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [StockQuantity]
        public byte Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
