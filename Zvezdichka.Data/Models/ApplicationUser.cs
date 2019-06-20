using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Zvezdichka.Data.Models.Checkout;

namespace Zvezdichka.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Purchase> Purchases { get; set; }

        public bool IsEmailVerified { get; set; } = false;
    }
}