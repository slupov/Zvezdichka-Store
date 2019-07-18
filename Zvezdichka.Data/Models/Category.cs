using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models.Mapping;

namespace Zvezdichka.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        public string Country { get; set; }

        public virtual ICollection<CategoryProduct> Products { get; set; } = new HashSet<CategoryProduct>();
    }
}