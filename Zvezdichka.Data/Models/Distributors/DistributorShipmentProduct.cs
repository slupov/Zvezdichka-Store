using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models.Distributors
{
    public class DistributorShipmentProduct
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Quantity { get; set; }

        public double DiscountPercentage { get; set; }

        public decimal Price { get; set; }

        public DistributorShipment DistributorShipment { get; set; }

        [Required]
        public int DistributorShipmentId { get; set; }
    }
}