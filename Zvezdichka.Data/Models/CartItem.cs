using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public byte Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
