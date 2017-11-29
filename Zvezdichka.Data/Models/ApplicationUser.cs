using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Zvezdichka.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
