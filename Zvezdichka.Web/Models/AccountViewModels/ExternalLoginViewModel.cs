using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Models.AccountViewModels
{
    public class ExternalLoginViewModel : IMapFrom<ApplicationUser>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
