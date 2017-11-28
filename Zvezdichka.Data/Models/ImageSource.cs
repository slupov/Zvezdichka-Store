using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models
{
    public class ImageSource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string Source { get; set; }
    }
}
