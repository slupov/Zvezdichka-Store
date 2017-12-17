using System;
using System.ComponentModel.DataAnnotations;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Api.Models.Comments
{
    public class PostRequestCommentModel : IMapFrom<Comment>, IHaveCustomMapping
    {
        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        [Required]

        public int ProductId { get; set; }

        [Required]
        public string Username { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<Comment, PostRequestCommentModel>()
                .ForMember(pdv => pdv.Username, cfg => cfg.MapFrom(c => c.User.UserName));
        }
    }
}