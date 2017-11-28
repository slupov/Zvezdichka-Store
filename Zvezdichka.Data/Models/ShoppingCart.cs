using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}