using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models
{
    public class Cart
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
    }
}
