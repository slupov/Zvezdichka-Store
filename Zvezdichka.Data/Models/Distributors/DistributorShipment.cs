using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models.Distributors
{
    public class DistributorShipment
    {
        [Key]
        public int Id { get; set; }

        public virtual Distributor Distributor { get; set; }

        [Required]
        public int DistributorId { get; set; }

        public DateTime Date { get; set; }
        public virtual ICollection<DistributorShipmentProduct> Products { get; set; }
    }
}