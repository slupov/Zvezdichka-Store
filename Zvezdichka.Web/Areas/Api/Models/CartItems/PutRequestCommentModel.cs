using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Api.Models.CartItems
{
    public class PutRequestCommentModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
