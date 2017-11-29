using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Range(1,256)]
        public byte Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
