using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Api.Models.Comments;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentsDataService comments;
        private readonly IProductsDataService products;
        private readonly UserManager<ApplicationUser> users;

        public CommentsController(ICommentsDataService comments, IProductsDataService products,
            UserManager<ApplicationUser> users)
        {
            this.comments = comments;
            this.products = products;
            this.users = users;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]PostRequestCommentModel comment)
        {
            //create new comment from model
            if (!this.ModelState.IsValid)
                return NotFound();

            var user = await this.users.FindByNameAsync(comment.Username);
            var product = this.products.GetSingle(x => x.Id == comment.ProductId);

            if (user == null || product == null)
                return NotFound();

            this.comments.Add(new Comment()
            {
                Message = comment.Message,
                Product = product,
                User = user
            });

            return Ok();
        }
    }
}