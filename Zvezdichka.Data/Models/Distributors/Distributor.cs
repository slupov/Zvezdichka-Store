using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models.Distributors
{
    public class Distributor
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}