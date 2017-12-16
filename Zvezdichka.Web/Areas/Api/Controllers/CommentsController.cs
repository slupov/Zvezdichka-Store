using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Web.Areas.Api.Models.Comments;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CommentsController : ApiBaseController
    {
        private readonly ICommentsDataService comments;
        private readonly IProductsDataService products;
        private readonly IHtmlSanitizer html;
        private readonly UserManager<ApplicationUser> users;

        public CommentsController(ICommentsDataService comments, IProductsDataService products, IHtmlSanitizer html,
            UserManager<ApplicationUser> users)
        {
            this.comments = comments;
            this.products = products;
            this.users = users;
            this.html = html;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PostRequestCommentModel comment)
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
                Message = this.html.Sanitize(comment.Message),
                Product = product,
                User = user
            });

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = this.comments.GetSingle(x => x.Id == id, x => x.User);

            if (toDelete == null)
            {
                return NotFound();
            }

            //check if comment is of the correct user
            if (toDelete.User.UserName != this.User.Identity.Name)
            {
                //check if admin or manager
                if (this.User.IsInRole("Admin"))
                {
                    this.comments.Remove(toDelete);
                    return Ok();
                }

                return BadRequest();
            }

            this.comments.Remove(toDelete);
            return Ok();
        }
    }
}