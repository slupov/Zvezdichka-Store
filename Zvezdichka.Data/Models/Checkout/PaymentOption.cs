using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models.Checkout
{
    public class PaymentOption
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
    }
}