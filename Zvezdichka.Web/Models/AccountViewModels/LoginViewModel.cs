using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Models.AccountViewModels
{
    public class LoginViewModel : IMapFrom<ApplicationUser>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
