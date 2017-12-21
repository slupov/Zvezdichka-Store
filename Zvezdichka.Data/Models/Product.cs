using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models.Mapping;

namespace Zvezdichka.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public byte Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ThumbnailSource { get; set; }

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public ICollection<CategoryProduct> Categories { get; set; } = new HashSet<CategoryProduct>();
    }
}