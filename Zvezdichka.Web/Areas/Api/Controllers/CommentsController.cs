using System;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Web.Areas.Api.Models.CartItems;
using Zvezdichka.Web.Areas.Api.Models.Comments;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Html;

namespace Zvezdichka.Web.Areas.Api.Controllers
{
    public class CommentsController : ApiBaseController
    {
        private readonly IGenericDataService<Comment> comments;
        private readonly IGenericDataService<Product> products;
        private readonly IHtmlService html;
        private readonly UserManager<ApplicationUser> users;

        public CommentsController(IGenericDataService<Comment> comments, IGenericDataService<Product> products, IHtmlService html,
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
            {
                return NotFound();
            }

            var user = await this.users.FindByNameAsync(comment.Username);
            var product = await this.products.GetSingleOrDefaultAsync(x => x.Id == comment.ProductId);

            if (user == null || product == null)
                return NotFound();

            var message = this.html.Sanitize(comment.Message); //html sanitizer removes everything

            var commentToAdd = new Comment()
            {
                Message = message,
                Product = product,
                User = user,
                DateAdded = DateTime.Now
            };

            this.comments.Add(commentToAdd);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] PutRequestCommentModel comment)
        {
            //create new comment from model
            if (!this.ModelState.IsValid)
                return NotFound();

            var dbComment = await this.comments.GetSingleOrDefaultAsync(x => x.Id == comment.Id);

            dbComment.DateEdited = DateTime.Now;
            dbComment.IsEdited = true;
            dbComment.Message = this.html.Sanitize(comment.Message);

            this.comments.Update(dbComment);
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = await this.comments.GetSingleOrDefaultAsync(x => x.Id == id);

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